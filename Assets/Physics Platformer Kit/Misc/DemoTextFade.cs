using UnityEngine;
using System.Collections;

//you can delete this, its just to make the demo scene nicer to play in
public class DemoTextFade : MonoBehaviour 
{
	void Awake()
	{
		foreach(Transform child in transform)
				child.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = false;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.transform.tag == "Player")
			foreach(Transform child in transform)
				child.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = true;
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.transform.tag == "Player")
			foreach(Transform child in transform)
				child.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = false;
	}
}

//NOTE: usually getComponent should be reserved for Awake as its quite expensive, but this is just a demo scene..