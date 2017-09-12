using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameOnClick : MonoBehaviour
{

    public void LoadSceneByIndex(int index)
    {
		/* NOT WORKING, MAYBE THERE SOMETHING TRANSPARENT OVER THE BUTTONS */
		Debug.Log("LOADING");
		SceneManager.LoadScene(index);
    }
}
