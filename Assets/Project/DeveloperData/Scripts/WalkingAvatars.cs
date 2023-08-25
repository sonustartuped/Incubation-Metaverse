 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalkingAvatars : MonoBehaviour
{
    //private UnityEngine.AI.NavMeshAgent navMeshAgent;
    //private Vector3 randomDestination;
    //private float timeBetweenDestinations = 3f;
    //private float timer;

    //void Start()
    //{
    //    navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    //    timer = timeBetweenDestinations;
    //    SetRandomDestination();
    //}

    //void Update()
    //{
    //    timer -= Time.deltaTime;

    //    if (timer <= 0f)
    //    {
    //        SetRandomDestination();
    //        timer = timeBetweenDestinations;
    //    }
    //}

    //void SetRandomDestination()
    //{
    //    randomDestination = RandomNavMeshLocation(10f);
    //    navMeshAgent.SetDestination(randomDestination);
    //}

    //Vector3 RandomNavMeshLocation(float radius)
    //{
    //    Vector3 randomDirection = Random.insideUnitSphere * radius;
    //    randomDirection += transform.position;
    //    UnityEngine.AI.NavMeshHit hit;
    //    UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, radius, UnityEngine.AI.NavMesh.AllAreas);
    //    return hit.position;
    //}
}
