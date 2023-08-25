using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovingPoint : MonoBehaviour
{
  public CharacterController EnemyController;
  private Vector3 moveDirection = Vector3.zero;
  public float gravity = 100.0f;

  private float speed;

  public GameObject target1;
  public GameObject target2;
  public GameObject target3;
  public GameObject target4;
  public GameObject target5;
  public GameObject target6;

void Start()
{
	EnemyController = GetComponent<CharacterController>();
}

void Update()
{
	if(EnemyController.isGrounded == true)
	{
		moveDirection =transform.TransformDirection(Vector3.forward);
	}
	//  moveDirection.y -=gravity * Time.time;
	//  EnemyController>Move(moveDirection * speed * Time.deltaTime);
}

void OnTriggerEnter(Collider other)
{
	if(other.gameObject.tag =="point1")
	{
		transform.LookAt(target2.transform.position);
	}
	if(other.gameObject.tag == "point2")
	{
		transform.LookAt(target3.transform.position);
	}
	if(other.gameObject.tag == "point3")
	{
		transform.LookAt(target4.transform.position);
	}
	if(other.gameObject.tag == "point4")
	{
		transform.LookAt(target5.transform.position);
	}
	if(other.gameObject.tag == "point5")
	{
		transform.LookAt(target6.transform.position);
	}
}

}
 