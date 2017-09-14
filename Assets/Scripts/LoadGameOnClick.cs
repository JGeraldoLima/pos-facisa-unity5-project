using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameOnClick : MonoBehaviour
{

    public void LoadSceneByIndex(int index)
    {
        Debug.Log("LOADING");
        SceneManager.LoadScene(index);
    }
}
