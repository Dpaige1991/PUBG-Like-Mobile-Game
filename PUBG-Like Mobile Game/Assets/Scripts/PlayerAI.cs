using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerAI : MonoBehaviour
{
    [Header("Player Health And Damage")]
    private float playerHealth = 120f;
    private float presentHealth;
    public float giveDamage = 5f;
    public float playerSpeed;

    [Header("Player Things")]
    public NavMeshAgent playerAgent;
    public Transform lookPoint;
    public GameObject shootingRaycastArea;
    public Transform enemyBody;
    public LayerMask enemyLayer;
    public Transform spawn;
    public Transform playerCharacter;

    [Header("Player Shooting Var")]
    public float timebtwShoot;
    bool previouslyShoot;

    public ScoreManager scoreManager;

    [Header("Player Animation and Spark Effect")]
    public Animator animator;
    public ParticleSystem muzzleSpark;

    [Header("Player State")]
    public float visionRadius;
    public float shootingRadius;
    public bool enemyInvisionRadius;
    public bool enemyInshootingRadius;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip shootingSound;

    private void Awake()
    {
        playerAgent = GetComponent<NavMeshAgent>();
        presentHealth = playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        enemyInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, enemyLayer);
        enemyInshootingRadius = Physics.CheckSphere(transform.position, shootingRadius, enemyLayer);

        if (enemyInvisionRadius && !enemyInshootingRadius)
        {
            PursingEnemy();
        }

        if (enemyInshootingRadius && enemyInshootingRadius)
        {
            ShootEnemy();
        }
    }

    private void PursingEnemy()
    {
        if (playerAgent.SetDestination(enemyBody.position))
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

    private void ShootEnemy()
    {
        playerAgent.SetDestination(transform.position);

        transform.LookAt(lookPoint);

        if (!previouslyShoot)
        {
            muzzleSpark.Play();
            audioSource.PlayOneShot(shootingSound);

            RaycastHit hit;

            if (Physics.Raycast(shootingRaycastArea.transform.position, shootingRaycastArea.transform.forward, out hit, shootingRadius))
            {
                Debug.Log("Shooting" + hit.transform.name);

                Enemy enemy = hit.transform.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.enemyHitDamage(giveDamage);
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

    public void PlayerAIHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;

        if (presentHealth <= 0)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        playerAgent.SetDestination(transform.position);
        playerSpeed = 0f;
        shootingRadius = 0f;
        visionRadius = 0f;
        enemyInvisionRadius = false;
        enemyInshootingRadius = false;
        animator.SetBool("Die", true);
        animator.SetBool("Running", false);
        animator.SetBool("Shooting", false);

        Debug.Log("Dead");

        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        scoreManager.enemyKills += 1;

        yield return new WaitForSeconds(5f);

        gameObject.GetComponent<CapsuleCollider>().enabled = true;

        presentHealth = 120f;
        playerSpeed = 3f;
        shootingRadius = 10f;
        visionRadius = 100f;
        enemyInvisionRadius = true;
        enemyInshootingRadius = false;

        animator.SetBool("Die", false);
        animator.SetBool("Running", true);

        playerCharacter.transform.position = spawn.transform.position;
        PursingEnemy();
    }
}
