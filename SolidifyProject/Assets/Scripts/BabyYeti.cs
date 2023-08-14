using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BabyYeti : MonoBehaviour
{
    public List<Transform> PatrolRoute;
    NavMeshAgent agent;
    int index= 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = PatrolRoute[index].transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(agent.pathPending);

        if (agent.pathPending && index < 2)
        {
            agent.destination = PatrolRoute[index++].transform.position;

        }
        else
        {
            agent.destination = PatrolRoute[0].transform.position;

        }
    }
}
