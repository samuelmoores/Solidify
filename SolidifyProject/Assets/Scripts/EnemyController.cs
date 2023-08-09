using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public GameObject Destination;
    public GameObject SnowBallRef;
    public Transform ThrowRef;

    NavMeshAgent agent;
    Animator animator;
    float attackCooldown;
    bool isThrowing;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 1.0f;
        agent.isStopped = true;
        animator.SetBool("canThrow", false);
        animator.SetBool("isWalking", false);
        attackCooldown = 5.0f;
        isThrowing = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = Destination.transform.position;

        if (agent.remainingDistance < 20 && !agent.pathPending)
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
            if (attackCooldown < 4.25f && !isThrowing)
                ThrowSnowBall();

            //Transition out of animation
            if(attackCooldown < 3 && attackCooldown > 0)
            {
                animator.SetBool("canThrow", false);
            }

            //Wait for next throw
            if (attackCooldown < 0)
            {
                animator.SetBool("canThrow", true);
                attackCooldown = 5;
                isThrowing = false;
            }

        }
        else if(agent.remainingDistance > 20 && !agent.pathPending)
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
        SnowBall.GetComponent<Rigidbody>().AddForce(ThrowRef.transform.forward * 450, ForceMode.Impulse);
        isThrowing = true;
       

    }
}
