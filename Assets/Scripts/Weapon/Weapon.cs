using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

	public int bulletsPerMag = 30, bulletsLeft = 200, currentBullets;
	public float fireRate = 0.1f, range = 100f, damage = 20f, aodSpeed = 8f;
	public Transform shootPoint;
	public ParticleSystem muzzleFlash;
	public AudioClip shootSound;
	public GameObject hitParticles, bulletImpact;
	public Vector3 aimPosition;

	public enum ShootMode
	{
		Auto,
		Semi
	};

	public ShootMode shootingMode;
	
	private float fireTimer;
	private bool isReloading, shootInput, isAiming;
	private Animator anim;
	private AudioSource _AudioSource;
	private Vector3 originalPosition;
		
	// Use this for initialization
	void Start ()
	{
		currentBullets = bulletsPerMag;
		anim = GetComponent<Animator>();
		_AudioSource = GetComponent<AudioSource>();
		originalPosition = transform.localPosition;
		bulletsLeft = 200;
	}
	
	// Update is called once per frame
	void Update () {

		switch (shootingMode)
		{
				case ShootMode.Auto:
					shootInput = Input.GetButton("Fire1");
				break;
				
				case ShootMode.Semi:
					shootInput = Input.GetButtonDown("Fire1");
				break;
		}

		if (shootInput)
		{
			if(currentBullets > 0)
				Fire();
			else if(bulletsLeft > 0) DoReload();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			if(currentBullets < bulletsPerMag && bulletsLeft > 0)
				DoReload();
		}

		if (fireTimer < fireRate)
			fireTimer += Time.deltaTime;
		
		AimDownSights();
	}

	private void FixedUpdate()
	{
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

		isReloading = info.IsName("Reload");
		anim.SetBool("Aim", isAiming);
	}

	private void AimDownSights()
	{
		if (Input.GetButton("Fire2") && !isReloading)
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * aodSpeed);
			isAiming = true;
		}
		else
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * aodSpeed);
			isAiming = false;
		}
	}

	private void Fire()
	{
		if (fireTimer < fireRate || currentBullets <= 0 || isReloading)
			return;

		RaycastHit hit;

		if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
		{
			GameObject hitParticleEffect =
				Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
			hitParticleEffect.transform.SetParent(hit.transform);
			GameObject bulletHole =
				Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
			
			Destroy(hitParticleEffect, 1f);
			Destroy(bulletHole, 2f);

			if (hit.transform.GetComponent<SkeletonAI>())
			{
				hit.transform.GetComponent<SkeletonAI>().ApplyDamage(damage);
			}
		}

		anim.CrossFadeInFixedTime("Fire", 0.01f);
		muzzleFlash.Play();
		PlayShootSound();

		currentBullets--;
		fireTimer = 0.0f;
	}

	private void DoReload()
	{
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

		if (isReloading)
			return;
		
		anim.CrossFadeInFixedTime("Reload", 0.01f);
	}
	
	public void Reload()
	{
		if (bulletsLeft <= 0)
			return;

		int bulletsToLoad = bulletsPerMag - currentBullets;
		int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

		bulletsLeft -= bulletsToDeduct;
		currentBullets += bulletsToDeduct;
	}

	private void PlayShootSound()
	{
		AudioSource.PlayClipAtPoint(shootSound, gameObject.transform.position);
		//_AudioSource.clip = shootSound;
		//_AudioSource.Play();
	}
}
