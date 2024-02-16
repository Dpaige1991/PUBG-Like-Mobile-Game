using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Health And Damage")]
    private float enemyHealth = 120f;
    private float presentHealth;
    public float giveDamage = 5f;
    public float enemySpeed;

    [Header("Enemy Things")]
    public NavMeshAgent enemyAgent;
    public Transform lookPoint;
    public GameObject shootingRaycastArea;
    public Transform playerBody;
    public LayerMask playerLayer;
    public Transform spawn;
    public Transform enemyCharacter;

    [Header("Enemy Shooting Var")]
    public float timebtwShoot;
    bool previouslyShoot;

    [Header("Enemy Animation and Spark Effect")]
    public Animator animator;
    public ParticleSystem muzzleSpark;

    [Header("Enemy State")]
    public float visionRadius;
    public float shootingRadius;
    public bool playerInvisionRadius;
    public bool playerInshootingRadius;
    public bool isPlayer = false;

    public ScoreManager scoreManager;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip shootingSound;

    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        presentHealth = enemyHealth;
    }

    // Update is called once per frame
    void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        playerInshootingRadius = Physics.CheckSphere(transform.position, shootingRadius, playerLayer);

        if(playerInvisionRadius && !playerInshootingRadius)
        {
            PursingPlayer();
        }

        if(playerInshootingRadius && playerInshootingRadius)
        {
            ShootPlayer();
        }
    }

    private void PursingPlayer()
    {
        if(enemyAgent.SetDestination(playerBody.position))
        {
            animator.SetBool("Running", true);
            animator.SetBool("Shooting", false);
        }
        else
        {
            animator.SetBool("Running", false);
            animator.SetBool("Shooting", false);
        }
    }

    private void ShootPlayer()
    {
        enemyAgent.SetDestination(transform.position);

        transform.LookAt(lookPoint);

        if(!previouslyShoot)
        {
            muzzleSpark.Play();
            audioSource.PlayOneShot(shootingSound);

            RaycastHit hit;

            if(Physics.Raycast(shootingRaycastArea.transform.position, shootingRaycastArea.transform.forward, out hit, shootingRadius))
            {
                Debug.Log("Shooting" + hit.transform.name);

                if (isPlayer == true)
                {
                    PlayerScript playerBody = hit.transform.GetComponent<PlayerScript>();

                    if (playerBody != null)
                    {
                        playerBody.playerHitDamage(giveDamage);
                    }
                }
                else
                {
                    PlayerAI playerAI = hit.transform.GetComponent<PlayerAI>();
                    if (playerAI != null)
                    {
                        playerAI.PlayerAIHitDamage(giveDamage);
                    }
                }

                animator.SetBool("Running", false);
                animator.SetBool("Shooting", true);
            }

            previouslyShoot = true;
            Invoke(nameof(ActiveShooting), timebtwShoot);
        }

    }

    private void ActiveShooting()
    {
        previouslyShoot = false;
    }

    public void enemyHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;

        if(presentHealth <= 0)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        enemyAgent.SetDestination(transform.position);
        enemySpeed = 0f;
        shootingRadius = 0f;
        visionRadius = 0f;
        playerInvisionRadius = false;
        playerInshootingRadius = false;
        animator.SetBool("Die", true);
        animator.SetBool("Running", false);
        animator.SetBool("Shooting", false);

        Debug.Log("Dead");

        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        scoreManager.kills += 1;

        yield return new WaitForSeconds(5f);

        gameObject.GetComponent<CapsuleCollider>().enabled = true;

        presentHealth = 120f;
        enemySpeed = 3f;
        shootingRadius = 10f;
        visionRadius = 100f;
        playerInvisionRadius = true;
        playerInshootingRadius = false;

        animator.SetBool("Die", false);
        animator.SetBool("Running", true);

        enemyCharacter.transform.position = spawn.transform.position;
        PursingPlayer();
    }
}
