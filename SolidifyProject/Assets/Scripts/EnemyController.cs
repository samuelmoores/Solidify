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

    NavMeshAgent agent;
    Animator animator;
    float attackCooldown;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        PlayerController= Player.GetComponent<PlayerController>();

        agent.speed = 1.0f;
        agent.isStopped = true;
        animator.SetBool("canThrow", false);
        animator.SetBool("isWalking", false);
        attackCooldown = 10.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = Destination.transform.position;

        Debug.Log(attackCooldown);

        if (agent.remainingDistance < 75 && !agent.pathPending)
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
            if (attackCooldown < 9.25f && !hasThrown)
                ThrowSnowBall();

            //Transition out of animation
            if (attackCooldown < 8 && attackCooldown > 0)
            {
                animator.SetBool("canThrow", false);
            }

            //Wait for next throw
            if (attackCooldown < 0)
            {
                animator.SetBool("canThrow", true);
                attackCooldown = 10;
                hasThrown = false;
            }


        }
        else if(agent.remainingDistance > 75 && !agent.pathPending)
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
        SnowBall.GetComponent<Rigidbody>().AddForce(ThrowRef.transform.forward * 4150, ForceMode.Impulse);
        hasThrown = true;
       
    }
}
