using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarMoving : MonoBehaviour
{
    public static CarMoving instance;



    public float stopDuration = 2.0f; 
    private bool isColliding = false;

    public Transform[] waypoints; 
    public float moveSpeed = 5f; 
    private int currentWayPointIndex = 0;
    

    void Awake()
    {
        instance = this;
    }

  //  private void WayPoint()
  //  {
  //      if(waypoints.Length == 0)
  //      {
  //          enabled = true;
  //      }
  //
  //  }
    private void Update()
    {
       // if(Vector3.Distance(transform.position, waypoints[currentWayPointIndex].position)< 0.1f)
       // {
       //     currentWayPointIndex = (currentWayPointIndex + 1) % waypoints.Length;
       // }
       // if (waypoints.Length == 0)
       // return;
        MoveToWayPoint();
    }

    private void MoveToWayPoint()
    {
       // Vector3 direction = (waypoints[currentWayPointIndex].position - transform.position).normalized;
       // transform.position += direction * moveSpeed * Time.deltaTime;

         if (currentWayPointIndex < waypoints.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWayPointIndex].position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, waypoints[currentWayPointIndex].position) < 0.1f)
            {
                currentWayPointIndex++;
                if (currentWayPointIndex >= waypoints.Length)
                {
                    currentWayPointIndex = 0;
                }
            }
        }
    }
   // private void RotateToNextWaypoint()
   // {
   //     Vector3 nextWaypointDirection = waypoints[(currentWayPointIndex + 1) % waypoints.Length].position - transform.position;
   //     Quaternion targetRotation = Quaternion.LookRotation(nextWaypointDirection);
   //     transform.rotation = targetRotation;
   // }
    private void OnCollisionEnter(Collision collision)
    {
        if(!isColliding)
        {
            isColliding = true;
            StartCoroutine(StopForDuration());
        }
    }

    private IEnumerator StopForDuration()
    {
       
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null)
        rb.velocity = Vector3.zero;

       
        yield return new WaitForSeconds(stopDuration);

        if(rb != null)
        rb.velocity = Vector3.forward; 
        isColliding = false; 
    }
}
