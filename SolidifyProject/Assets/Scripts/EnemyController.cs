using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public GameObject PatrolLoc;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = PatrolLoc.transform.position;

    }
}
