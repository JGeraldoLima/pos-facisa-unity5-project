using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private bool stop = false;

	void Stop()
	{
		stop = true;
	}

	// Use this for initialization
	void Start ()
	{
		
//		AvatarController.OnGameOver += Stop;

//		StartCoroutine (Spawn ());

	}

	// Update is called once per frame
	void Update () 
	{

	}

}
