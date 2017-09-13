using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOnClick : MonoBehaviour
{

    public void Pause()
    {
        // make main menu appears ('start' comes 'continue')
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

    }
}
