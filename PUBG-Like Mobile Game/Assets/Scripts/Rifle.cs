using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Rifle")]
    public Camera cam;
    public float giveDamage = 10f;
    public float shootingRange = 100f;
    public float fireChange = 15f;
    public PlayerScript player;
    public Animator animator;

    [Header("Rifle Ammunition and shooting")]
    private float nextTimeToShoot = 0f;
    private int maximumAmmunition = 20;
    private int mag = 15;
    private int presentAmmunition;
    public float reloadingTime = 1.3f;
    private bool setReloading = false;

    [Header("Rifle Effect")]
    public ParticleSystem muzzleSpark;
    public GameObject WoodedEffect;
    public GameObject goreEffect;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip shootingSound;
    public AudioClip reloadingSound;

    private void Awake()
    {
        presentAmmunition = maximumAmmunition;
    }

    // Update is called once per frame
    void Update()
    {
        if (setReloading)
        {
            return;
        }

        if (presentAmmunition <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (player.mobileInputs == true)
        {
            if (CrossPlatformInputManager.GetButton("Shoot") && Time.time >= nextTimeToShoot)
            {
                animator.SetBool("Fire", true);
                animator.SetBool("Idle", false);
                nextTimeToShoot = Time.time + 1f / fireChange;
                Shoot();
            }
            else if (CrossPlatformInputManager.GetButton("Shoot") && player.currentPlayerSpeed > 0)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("FireWalk", true);
            }
            else if (CrossPlatformInputManager.GetButton("Shoot") && CrossPlatformInputManager.GetButton("Aim"))
            {
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", true);
                animator.SetBool("FireWalk", true);
                animator.SetBool("Walk", true);
                animator.SetBool("Reloading", false);
            }
            else
            {
                animator.SetBool("Fire", false);
                animator.SetBool("Idle", true);
                animator.SetBool("FireWalk", false);
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
            {
                animator.SetBool("Fire", true);
                animator.SetBool("Idle", false);
                nextTimeToShoot = Time.time + 1f / fireChange;
                Shoot();
            }
            else if (Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                animator.SetBool("Idle", false);
                animator.SetBool("FireWalk", true);
            }
            else if (Input.GetButton("Fire1") && Input.GetButton("Fire2"))
            {
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", true);
                animator.SetBool("FireWalk", true);
                animator.SetBool("Walk", true);
                animator.SetBool("Reloading", false);
            }
            else
            {
                animator.SetBool("Fire", false);
                animator.SetBool("Idle", true);
                animator.SetBool("FireWalk", false);
            }
        }
    }

    void Shoot()
    {
        if(mag == 0)
        {

        }

        presentAmmunition--;

        if(presentAmmunition == 0)
        {
            mag--;
        }

        AmmoCount.occurence.UpdateAmmoText(presentAmmunition);
        AmmoCount.occurence.UpdateMagText(mag);

        muzzleSpark.Play();
        audioSource.PlayOneShot(shootingSound);

        RaycastHit hitInfo;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);

            Objects objects = hitInfo.transform.GetComponent<Objects>();

            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();

            if (objects != null)
            {
                objects.objectHitDamage(giveDamage);
                GameObject woodGo = Instantiate(WoodedEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(woodGo, 1f);
            }
            else if(enemy != null)
            {
                enemy.enemyHitDamage(giveDamage);
                GameObject goreGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreGo, 1f);
            }
        }
    }

    IEnumerator Reload()
    {
        player.playerSpeed = 0f;
        player.playerSprint = 0f;
        setReloading = true;
        Debug.Log("Reloading...");
        animator.SetBool("Reloading", true);
        audioSource.PlayOneShot(reloadingSound);
        yield return new WaitForSeconds(reloadingTime);
        animator.SetBool("Reloading", false);
        presentAmmunition = maximumAmmunition;
        player.playerSpeed = 1.0f;
        player.playerSprint = 3f;
        setReloading = false;
    }
}
