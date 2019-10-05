using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#pragma warning disable 0649
public class SetAgentDestination : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    public void CalculateValidPath(Transform endPoint)
    {
        // Make a new path and calculate that path.
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(endPoint.position, path);

        // If the path is partial or invalid destroy this agent.
        // Otherwise set the destination.
        if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
        {
            Destroy(gameObject);
        }
        else
        {
            agent.SetDestination(endPoint.position);
        }
    }
}
