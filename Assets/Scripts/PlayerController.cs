﻿using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;

    /* Text objects */
    public Text playerGemsText;
    public Text winText;

    public Text countdownText;

    public Text playerRefillsText;

    private Rigidbody rb;

    /* Controller internal control variables */
    private int levelGems;
    private int playerGems;

    private int playerRefills;

    private string currentLevelKey = "currentLevel";

    public static int currentLevel = 1;

    private float remainingLevelTime = 0;

    private int currentLevelMod = 0;

    private int currentLevelHitRate = 0;

    private float[] currentLevelTimeouts = { 15f, 20f, 20f, 30f };

    private float[] currentLevelTimeRefill = { 5f, 5f, 5f, 5f };

    private int[] currentLevelHitRates = { 0, 5, 0, 0 };

    private int[] currentLevelTimeRefillMod = { 6, 8, 11, 10 };

    private int[] currentLevelGems = { 12, 15, 13, 15 };

    private Coroutine gameOverCoroutine;

    private Coroutine levelCompletedCoroutine;

    void Main()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // At the start of the game..
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        playerGems = 0;
        playerRefills = 0;
        levelGems = currentLevelGems[currentLevel - 1];
        currentLevelMod = currentLevelTimeRefillMod[currentLevel - 1];
        currentLevelHitRate = currentLevelHitRates[currentLevel - 1];
        remainingLevelTime = currentLevelTimeouts[currentLevel - 1];

        SetPlayerGemsText();

        winText.text = "";
        Time.timeScale = 1;
    }

    void Update()
    {
        remainingLevelTime -= Time.deltaTime;
        if (remainingLevelTime < 0 && playerGems < levelGems)
        {
            gameOverCoroutine = StartCoroutine(GameOver("You ran of time!"));
        }
        else if (remainingLevelTime >= 0 && playerGems < levelGems)
        {
            int minutes = Mathf.FloorToInt(remainingLevelTime / 60F);
            int seconds = Mathf.FloorToInt(remainingLevelTime - minutes * 60);

            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);


        }
    }

    // Each physics step..
    void FixedUpdate()
    {

        Vector3 dir = Vector3.zero;
        var reading = Input.acceleration;
    
        var y  = reading.y;

        if (y > 0) y *= 5;
        dir.z = y;
        dir.x = reading.x;
        
        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        dir *= Time.deltaTime * speed;
        rb.transform.Translate(dir);
        rb.transform.Rotate(new Vector3(0, 0, 0));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);

            playerGems = playerGems + 1;
            SetPlayerGemsText();

            if (playerGems % currentLevelMod == 0)
            {
                playerRefills += 1;
                playerRefillsText.text = "Time refills: " + playerRefills.ToString();
            }


        }

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Obstacle"))
        {
            remainingLevelTime -= currentLevelHitRate;
        }
        else if (col.gameObject.CompareTag("DeathPillar") && levelCompletedCoroutine == null)
        {
            gameOverCoroutine = StartCoroutine(GameOver("You've just died!"));
        }
    }

    public void OnClickRefill()
    {
        if (playerRefills > 0)
        {
            remainingLevelTime += currentLevelTimeRefill[currentLevel - 1];
            playerRefills -= 1;
            playerRefillsText.text = "Time refills: " + playerRefills.ToString();
        }

    }

    void SetPlayerGemsText()
    {
        playerGemsText.text = "Gems: " + playerGems.ToString() + "/" + levelGems.ToString();

        if (playerGems >= levelGems && remainingLevelTime >= 0 && gameOverCoroutine == null)
        {
            levelCompletedCoroutine = StartCoroutine(LevelComplete());
        }
    }

    IEnumerator LevelComplete()
    {

        currentLevel += 1;
        if (currentLevel > currentLevelGems.Length)
        {
            winText.text = "Game complete! Congratulations!";
            currentLevel = 1;
            playerGems = 0;
            playerRefills = 0;
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(0);
        }
        else
        {
            winText.text = "You Win! Loading next level ...";
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(currentLevel);
        }

    }

    IEnumerator GameOver(string gameOverText)
    {
        currentLevel = 1;
        playerGems = 0;
        playerRefills = 0;
        winText.text = gameOverText;
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
}