using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour {

	public GameObject[] levelPreFabs;

	public GameObject player;

	private GameObject currentLevel;

	private GameObject nextLevel;

	// key:: level_name, value :: level_end_x
	private Dictionary<string, double> levelsLimits = new Dictionary<string, double>();

	// Use this for initialization
	void Start () {
		levelPreFabs [0] = GameObject.FindGameObjectWithTag ("Level1");
		levelPreFabs [1] = GameObject.FindGameObjectWithTag ("Level2");
		levelPreFabs [2] = GameObject.FindGameObjectWithTag ("Level3");
		levelPreFabs [3] = GameObject.FindGameObjectWithTag ("Level4");

		levelsLimits["Level1"] = 78;
		levelsLimits["Level2"] = -68;
		levelsLimits["Level3"] = 31;
		levelsLimits["Level4"] = 328;
	}
	
	// Update is called once per frame
	void Update () {
		// TODO: make the proper calc: it must consider all the distance travelled
//		if (player.transform.position.x >= levelsLimits[currentLevel.tag]) {
//			GameObject newLevel = (GameObject) Instantiate ( levelPreFab, position, Quaternion.identity );
			// set difficult variables
//		}
	}
}
