using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float damage = 18f;
    [SerializeField] private float attackDelay = 0.1f;
    [SerializeField] private float detectionDistance = 40f;
    [SerializeField] private int maxHealth = 6;
    [SerializeField] private int currentHealth = 6;

    [Header("References")]
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource walkSound;
    [SerializeField] private AudioSource attackSound;

    private GameObject player;
    private NavMeshAgent agent;
    private Animator anim;
    private PlayerActions playerActions;
    private GameManager gameManager;
    private PauseMenu pauseMenu;

    private bool isAttacking = false;
    private bool isDying = false;
    private float distance;

    private void Start()
    {        
        playerActions = FindObjectOfType<PlayerActions>();
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();        
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        pauseMenu = FindObjectOfType<PauseMenu>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);               

        if (pauseMenu.gameIsPaused)
            walkSound.enabled = false;

        if (agent.isStopped)
            walkSound.enabled = false;
        
        if (!isDying)
        {
            if (distance <= detectionDistance && !isAttacking)
            {
                walkSound.enabled = true;
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
                anim.SetBool("Walk", true);
            }
            else if (distance > detectionDistance + 5)
            {
                anim.SetBool("Walk", false);
                agent.isStopped = true;
            }

            if (distance <= agent.stoppingDistance && !isAttacking)
                StartCoroutine(Attack());
        }        
    }

    private IEnumerator Attack()
    {
        agent.isStopped = true;
        isAttacking = true;

        anim.SetLayerWeight(1, 1);
        anim.SetBool("Attack", true);

        attackSound.Play();

        yield return new WaitForSeconds(0.3f);

        if (distance <= attackRange && !isDying)
            playerActions.TakeDamage(damage);

        yield return new WaitForSeconds(0.45f);

        agent.isStopped = false;
        isAttacking = false;

        anim.SetLayerWeight(1, 0);
        anim.SetBool("Attack", false);

        yield return new WaitForSeconds(attackDelay);
    }

    public void TakeDamage()
    {
        hitSound.Play();

        currentHealth -= 1;

        if (currentHealth <= 0)
        {
            agent.isStopped = true;
            isDying = true;

            if (gameManager.eventActive)
                gameManager.killCounter += 1;

            anim.SetLayerWeight(2, 1);
            anim.SetBool("Death", true);

            Destroy(gameObject, 1f);
        }
    }
}
