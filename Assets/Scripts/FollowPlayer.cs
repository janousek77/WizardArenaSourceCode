using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public Transform target; 
    public float speed = 3f;
    public float distance = 1f;

	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform>();
	}
		
	void Update() {
		transform.position = Vector2.MoveTowards (transform.position, target.position, speed * Time.deltaTime);
    }
}
