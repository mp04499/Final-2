using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour {

	public float restartDelay = 5f;         // Time to wait before restarting the level


	Animator anim;                          // Reference to the animator component.
	float restartTimer;

	void Start()
	{
		anim = GetComponent<Animator>();
	}

	public void GameOver()
	{

		// If the player has run out of health...

		// ... tell the animator the game is over.
		anim.SetTrigger("GameOver");

		// .. increment a timer to count up to restarting.
		restartTimer += Time.deltaTime;

		// .. if it reaches the restart delay...
		if (restartTimer >= restartDelay)
		{
			// .. then reload the currently loaded level.
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
