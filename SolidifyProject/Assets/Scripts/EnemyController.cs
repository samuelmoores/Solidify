using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public GameObject Destination;
    public GameObject SnowBallRef;
    public Transform ThrowRef;
    public GameObject Player;
    public PlayerController PlayerController;
    bool hitPlayer;
    bool hasThrown;
    public float throwSpeed;
    float health;
    public HealthBar healthBar;

    NavMeshAgent agent;
    Animator animator;
    float attackCooldown;

    public int attackRadius;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        PlayerController = Player.GetComponent<PlayerController>();

        agent.speed = 1.0f;
        agent.isStopped = true;
        animator.SetBool("canThrow", false);
        animator.SetBool("isWalking", false);
        attackCooldown = 4.0f;
        health = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = Destination.transform.position;

        if (agent.remainingDistance < attackRadius && !agent.pathPending)
        {
            attackCooldown -= Time.deltaTime;

            //Stop the yeti and play throw animation
            agent.isStopped = true;
            animator.SetBool("isWalking", false);

            //Make sure the yeti faces the player
            agent.updateRotation = true;
            Vector3 aim = transform.position - agent.destination;
            Quaternion aimRotation = Quaternion.LookRotation(aim);
            aimRotation *= Quaternion.Euler(0, 180, 0);
            transform.rotation = aimRotation;

            //Start animation
            animator.SetBool("canThrow", true);

            //Throw snow ball
            if (attackCooldown < 3.25f && !hasThrown && !PlayerController.isFrozen)
                ThrowSnowBall();

            //Transition out of animation
            if (attackCooldown < 2 && attackCooldown > 0)
            {
                animator.SetBool("canThrow", false);
            }

            //Wait for next throw
            if (attackCooldown < 0)
            {
                animator.SetBool("canThrow", true);
                attackCooldown = 4;
                hasThrown = false;
            }


        }
        else if(agent.remainingDistance > attackRadius && !agent.pathPending)
        {
            agent.isStopped = false;
            animator.SetBool("isWalking", true);
            animator.SetBool("canThrow", false);
        }
    }

    void ThrowSnowBall()
    {
        //Throw Snow Ball
        GameObject SnowBall = Instantiate(SnowBallRef, ThrowRef.position, Quaternion.identity);
        Destroy(SnowBall, 10f);
        SnowBall.GetComponent<Rigidbody>().AddForce(ThrowRef.transform.forward * throwSpeed, ForceMode.Impulse);
        hasThrown = true;
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            health -= 0.010f;
            healthBar.SetHealth(health);
        }
    }
}
