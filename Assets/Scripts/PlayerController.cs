using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;

    private Rigidbody rb;
    private int count;

    private string currentLevelKey = "currentLevel";

    public static int currentLevel = 1;

    private int[] currentLevelGems = { 12, 15 };

    void Main()
    {
        // Preventing mobile devices going in to sleep mode 
        //(actual problem if only accelerometer input is used)
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        PlayerPrefs.SetInt(currentLevelKey, currentLevel);
    }

    // At the start of the game..
    void Start()
    {
        // Assign the Rigidbody component to our private rb variable
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Set the count to zero 
        count = 0;

        // Run the SetCountText function to update the UI (see below)
        SetCountText();

        // Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
        winText.text = "";
    }

    // Each physics step..
    void FixedUpdate()
    {
        Vector3 dir = Vector3.zero;
        dir.x = Input.acceleration.x;
        dir.z = -Input.acceleration.z;
        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        dir *= Time.deltaTime * speed;
        rb.transform.Translate(dir);
        rb.transform.Rotate(new Vector3(0, 0, 0));
    }

    // When this game object intersects a collider with 'is trigger' checked, 
    // store a reference to that collider in a variable named 'other'..
    void OnTriggerEnter(Collider other)
    {
        // ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
        if (other.gameObject.CompareTag("Pick Up"))
        {
            // Make the other game object (the pick up) inactive, to make it disappear
            other.gameObject.SetActive(false);

            // Add one to the score variable 'count'
            count = count + 1;

            // Run the 'SetCountText()' function (see below)
            SetCountText();
        }
    }

    // Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
    void SetCountText()
    {
        // Update the text field of our 'countText' variable
        countText.text = "Count: " + count.ToString();

        currentLevel = PlayerPrefs.GetInt(currentLevelKey);
        if (count >= currentLevelGems[currentLevel - 1])
        {
            // Set the text value of our 'winText'
            StartCoroutine(LevelComplete());
        }
    }

    IEnumerator LevelComplete()
    {

        currentLevel = currentLevel + 1;
        if (currentLevel > currentLevelGems.Length)
        {
            winText.text = "Game complete! Exiting ...";
            yield return new WaitForSeconds(3);
            Application.Quit();
        }
        else
        {
            winText.text = "You Win! Loading next level ...";
            yield return new WaitForSeconds(3);
            PlayerPrefs.SetInt(currentLevelKey, currentLevel);
            SceneManager.LoadScene(currentLevel);
        }

    }
}