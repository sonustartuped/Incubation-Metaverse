using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovingPoint : MonoBehaviour
{
	
    public Transform startPoint;  // Starting position
    public Transform endPoint;    // Ending position
    public float speed = 5.0f;    // Speed of the car

    void Update()
    {
        // Move the vehicles towards the end point
        transform.position = Vector3.MoveTowards(transform.position, endPoint.position, speed * Time.deltaTime);

        // Check if the car has reached the end point
        if (Vector3.Distance(transform.position, endPoint.position) < 0.1f)
        {
            // Reset the vehicles position to the start point
            transform.position = startPoint.position;
			// Change the vehicles rotation to reach point...
			
        }
    }

}
 