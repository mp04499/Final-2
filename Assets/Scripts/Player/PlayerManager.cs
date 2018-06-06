using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

	private float health = 100f; 
	public float currentHealth;

	private GameObject currentWeapon;
	[SerializeField] private Slider currentHealthSlider;
	
	// Use this for initialization
	void Awake ()
	{
		currentHealth = health;
		currentHealthSlider.value = health;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TakeDamage(float damage)
	{
		currentHealth -= damage;
		currentHealthSlider.value -= damage;
	}

}
