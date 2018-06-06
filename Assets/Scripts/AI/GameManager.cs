using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Text cWave;
	[SerializeField] private GameObject enemy;
	[SerializeField] private Transform[] spawnPoints;
	private float spawnTime = 3.0f;
	private SkeletonAI sAI;
	private NavMeshAgent agent;
	private enum Wave
	{
		Wave1,
		Wave2,
		Wave3,
		Wave4
	}
	private Wave currentWave;
	private float maxEnemies; 
	public float currentEnemies;

	// Use this for initialization
	void Awake ()
	{
		agent = enemy.GetComponent<NavMeshAgent>();
		setWave(Wave.Wave1);
	}

	void Start()
	{
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void Update () {
		CheckEnemies();
	}
	
	private void setWave(Wave wave)
	{
		switch (wave)
		{
			case Wave.Wave1: agent.speed = 1.0f;
				currentWave = Wave.Wave1;
				cWave.text = "Wave: 1";
				maxEnemies = 10;
				currentEnemies = maxEnemies;
				break;
				
			case Wave.Wave2: agent.speed = 1.5f;
				currentWave = Wave.Wave2;
				cWave.text = "Wave: 2";
				maxEnemies = 20;
				currentEnemies = maxEnemies;
				break;
				
			case Wave.Wave3: agent.speed = 2.0f;
				currentWave = Wave.Wave3;
				cWave.text = "Wave: 3";
				maxEnemies = 30;
				currentEnemies = maxEnemies;
				break;
					
			case Wave.Wave4: agent.speed = 3.0f;
				currentWave = Wave.Wave4;
				cWave.text = "Wave: 4";
				maxEnemies = 40;
				currentEnemies = maxEnemies;
				break;
		}
	}

	private void CheckEnemies()
	{
		if(maxEnemies == 10f && currentEnemies <= 0)
			setWave(Wave.Wave2);
		else if(maxEnemies == 20f && currentEnemies <= 0)
			setWave(Wave.Wave3);
		else if(maxEnemies == 30f && currentEnemies <= 0)
			setWave(Wave.Wave3);
		else return;
	}
	private void Spawn()
	{
		if (currentEnemies > 0)
		{
			currentEnemies--;
			// Find a random index between zero and one less than the number of spawn points.
			int spawnPointIndex = Random.Range (0, spawnPoints.Length);

			// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
			var skeleton = Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
			skeleton.SetActive(true);
		}
		
	}
}
