using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonAI : MonoBehaviour
{

	public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
	
	[SerializeField] private Transform target;

	private NavMeshAgent agent;

	private Animator anim;
	private float damage = 10.0f, playerHealth, health = 100f, currentHealth, timer;
	public float restartDelay = 5f; 
	float restartTimer;  
	private bool isAttacking = false, playerInRange, isDead = false, isSinking;
	[SerializeField] private GameObject _player, gm, canvas;
	private PlayerManager player;
	private GameManager gameManager;
	private GameOverManager gameOverManager;

	// Use this for initialization
	void Start ()
	{
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		currentHealth = health;
		player = _player.GetComponent<PlayerManager>();
		gameManager = gm.GetComponent<GameManager>();
		canvas = GameObject.Find("Canvas");
		gameOverManager = canvas.GetComponent<GameOverManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (currentHealth > 0 && player.currentHealth > 0)
			FollowTarget(target);
		else agent.enabled = false;
		
		timer += Time.deltaTime;

		// If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
		if(timer >= timeBetweenAttacks && playerInRange && currentHealth > 0)
		{
			// ... attack.
			Attack (damage);
		}

		// If the player has zero or less health...
		if(player.currentHealth <= 0)
		{
			gameOverManager.GameOver();
		}
	}

	private void FollowTarget(Transform target)
	{
		agent.SetDestination(target.position);
		anim.SetBool("isWalking", true);
	}

	private void Attack(float damage)
	{
		// Reset the timer.
		timer = 0f;

		// If the player has health to lose...
		if(player.currentHealth > 0)
		{
			anim.Play("Attack1h1");
			// ... damage the player.
			player.TakeDamage(damage);
		}
	}

	public void ApplyDamage(float damage)
	{
		if(isDead)
			return;

		anim.Play("Hit1");
		currentHealth -= damage;

		if (currentHealth <= 0)
			Death();
	}

	private void Death()
	{
		isDead = true;
		
		anim.SetTrigger("Death");
		StartSinking();
	}
	
	public void StartSinking ()
	{
		// Find and disable the Nav Mesh Agent.
		GetComponent <NavMeshAgent> ().enabled = false;

		// Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
		GetComponent <Rigidbody> ().isKinematic = true;

		// The enemy should no sink.
		isSinking = true;

		// Increase the score by the enemy's score value.
		gameManager.currentEnemies--;

		// After 2 seconds destory the enemy.
		Destroy (gameObject, 2f);
	}
	
	void OnTriggerEnter (Collider other)
	{
		// If the entering collider is the player...
		if(other.gameObject.CompareTag("Player"))
		{
			// ... the player is in range.
			playerInRange = true;
		}
	}
	
	void OnTriggerExit (Collider other)
	{
		// If the exiting collider is the player...
		if(other.gameObject.CompareTag("Player"))
		{
			// ... the player is no longer in range.
			playerInRange = false;
		}
	}

}
