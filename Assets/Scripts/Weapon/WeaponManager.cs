using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

	[SerializeField] private GameObject[] weapons;
	[SerializeField] private float switchDelay = 1f;

	private int index;
	private bool isSwitching;
	
	// Use this for initialization
	private void Start () {
		InitializeWeapons();
	}
	
	// Update is called once per frame
	private void Update () {

		if (Input.GetAxis("Mouse ScrollWheel") > 0 && !isSwitching)
		{
			index++;

			if (index >= weapons.Length)
				index = 0;
			StartCoroutine(switchAfterDelay(index));
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0 && !isSwitching)
		{
			index--;

			if (index < 0)
				index = weapons.Length - 1;
			StartCoroutine(switchAfterDelay(index));
		}
	}

	private IEnumerator switchAfterDelay(int newIndex)
	{
		isSwitching = true;
		
		yield return new WaitForSeconds(switchDelay);

		isSwitching = false;
		SwitchWeapons(newIndex);
	}
	
	private void InitializeWeapons()
	{
		foreach (GameObject weapon in weapons)
		{
			weapon.SetActive(false);
		}
		
		weapons[0].SetActive(true);
	}

	private void SwitchWeapons(int newIndex)
	{
		foreach (GameObject weapon in weapons)
		{
			weapon.SetActive(false);
		}
		
		weapons[newIndex].SetActive(true);
	}
}
