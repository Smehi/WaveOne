using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#pragma warning disable 0649
public class SetAgentDestination : MonoBehaviour
{
    private NavMeshAgent agent;
    private List<Transform> localEndPoints = new List<Transform>();
    private bool endPointsCopied = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void CalculateValidPath(List<Transform> endPoints, int presetIndex = -1)
    {
        if (!endPointsCopied)
        {
            localEndPoints = endPoints;
            endPointsCopied = true;
        }

        // If we have a preset index we want to use that otherwise get a random point
        int index = presetIndex == -1 ? Random.Range(0, localEndPoints.Count) : presetIndex;

        // Make a new path and calculate that path.
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(localEndPoints[index].position, path);

        // If the path is partial or invalid destroy this agent.
        // Otherwise set the destination.
        if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
        {
            // Try again but remove the current end point from the list because the path is not valid for that one.
            if (localEndPoints.Count > 1)
            {
                localEndPoints.RemoveAt(index);
                CalculateValidPath(localEndPoints);
            }
            else
            {
                // We have tried every end point and couldn't get a path so we destroy this object.
                Destroy(gameObject);
            }
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(localEndPoints[index].position);
            agent.SetDestination(localEndPoints[index].position);
        }
    }
}
