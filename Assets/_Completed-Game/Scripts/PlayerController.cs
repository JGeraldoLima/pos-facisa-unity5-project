using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;

public class PlayerController : MonoBehaviour
{

    public bool isFlat = true;



    /*------------- */
    private Vector3 zeroAc; 
     private Vector3 curAc;
     public float sensH = 10;
     public float sensV = 10;
     public float smooth = 0.5f;
     private float GetAxisH = 0;
     private float GetAxisV = 0;
    /* --------- */




    // Create public variables for player speed, and for the Text UI game objects
    public float speed;
    public Text countText;
    public Text winText;

    // Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
    private Rigidbody rb;
    private int count;

    void Main()
    {
        // Preventing mobile devices going in to sleep mode 
        //(actual problem if only accelerometer input is used)
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // At the start of the game..
    void Start()
    {
        // Assign the Rigidbody component to our private rb variable
        rb = GetComponent<Rigidbody>();

        // Set the count to zero 
        count = 0;

        // Run the SetCountText function to update the UI (see below)
        SetCountText();

        // Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
        winText.text = "";

         ResetAxes();
    }

    // Each physics step..
    void FixedUpdate()
    {
        // float moveHorizontal = Input.acceleration.x/*Input.GetAxis("Horizontal")*/;
        // float moveVertical = -Input.acceleration.z/*Input.GetAxis("Vertical")*/;

        // Debug.Log("horizontal: " + moveHorizontal);
        // Debug.Log("vertical: " + moveVertical);

        // Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // rb.AddForce (Input.acceleration/*movement * speed*/);

        // Vector3 deltaEulerAngles = Input.gyro.attitude.eulerAngles - startGyroAttitudeToEuler;
        // deltaEulerAngles.x = 0.0f;
        // deltaEulerAngles.z = 0.0f;

        //  transform.eulerAngles = startEulerAngles - deltaEulerAngles;

        // Vector3 movement = new Vector3(Input.acceleration.x, 0.0f, Input.acceleration.y);
        // rb.AddForce(movement * speed * Time.deltaTime);

        // curAc = Vector3.Lerp(curAc, Input.acceleration-zeroAc, Time.deltaTime/smooth);
             
        //  GetAxisH = Mathf.Clamp(curAc.x * sensH, -1, 1);
        //  GetAxisV = Mathf.Clamp(-curAc.z * sensV, -1, 1);
             
        //  Vector3 movement = new Vector3 (GetAxisH, 0.0f, GetAxisV);
        //  rb.AddForce(movement * speed);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // transform.Translate(Input.acceleration.x, 0f, -Input.acceleration.z);

        // Vector3 tilt = Input.acceleration;
        // if (isFlat)
        // {
        //     tilt = Quaternion.Euler(90, 0, 0) * tilt;
        // }

        // rb.AddForce(tilt * speed);

        Vector3 dir = Vector3.zero;
        dir.x = Input.acceleration.x;
        dir.z = -Input.acceleration.z;
        if (dir.sqrMagnitude > 1)
            dir.Normalize();
        
        dir *= Time.deltaTime;
        rb.transform.Translate(dir * speed);
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

        // Check if our 'count' is equal to or exceeded 12
        if (count >= 12)
        {
            // Set the text value of our 'winText'
            winText.text = "You Win!";
        }
    }

     void ResetAxes(){
         zeroAc = Input.acceleration;
         curAc = Vector3.zero;
     }
 
}