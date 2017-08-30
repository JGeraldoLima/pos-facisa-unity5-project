using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LevelEnd : MonoBehaviour{

	void Awake () {
		GetComponent<BoxCollider>().isTrigger = true;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("collided: " + transform.parent.tag, transform);
	}
}
