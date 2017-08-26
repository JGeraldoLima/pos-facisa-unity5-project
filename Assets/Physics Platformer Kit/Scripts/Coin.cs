using UnityEngine;

//class to add to collectible coins
[RequireComponent(typeof(SphereCollider))]
public class Coin : MonoBehaviour 
{
	public AudioClip collectSound;							//sound to play when coin is collected
	public Vector3 rotation = new Vector3(0, 80, 0);		//idle rotation of coin
	public Vector3 rotationGain = new Vector3(10, 20, 10);	//added rotation when player gets near coin 
	public float startSpeed = 3f;							//how fast coin moves toward player when they get near
	public float speedGain = 0.2f;							//how fast coin accelerates toward player when they're near
	
	private bool collected;
	private Transform player;
	private TriggerParent triggerParent;	//this is a utility class, that lets us check if the player is close to the coins "bounds sphere trigger"
	private GUIManager gui;
	
	//setup
	void Awake()
	{
		gui = FindObjectOfType(typeof(GUIManager)) as GUIManager ;
		if(tag != "Coin")
		{
			tag = "Coin";
			Debug.LogWarning ("'Coin' script attached to object not tagged 'Coin', tag added automatically", transform);
		}
		GetComponent<Collider>().isTrigger = true;
		triggerParent = GetComponentInChildren<TriggerParent>();
		//if no trigger bounds are attached to coin, set them up
		if(!triggerParent)
		{
			GameObject bounds = new GameObject();
			bounds.name = "Bounds";
			bounds.AddComponent<SphereCollider>();
			bounds.GetComponent<SphereCollider>().radius = 7f;
			bounds.GetComponent<SphereCollider>().isTrigger = true;
			bounds.transform.parent = transform;
			bounds.transform.position = transform.position;
			bounds.AddComponent<TriggerParent>();
			triggerParent = GetComponentInChildren<TriggerParent>();
			triggerParent.tagsToCheck = new string[1];
			triggerParent.tagsToCheck[0] = "Player";
			Debug.LogWarning ("No pickup radius 'bounds' trigger attached to coin: " + transform.name + ", one has been added automatically", bounds);
		}
	}
	
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	//move coin toward player when he is close to it, and increase the spin/speed of the coin
	void Update()
	{
		transform.Rotate (rotation * Time.deltaTime, Space.World);
		
		if(triggerParent.collided)
			collected = true;
		
		if (collected)
		{
			startSpeed += speedGain;
			rotation += rotationGain;
			transform.position = Vector3.Lerp (transform.position, player.position, startSpeed * Time.deltaTime);
		}	
	}
	
	//give player coin when it touches them
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
			CoinGet();
	}
	
	void CoinGet()
	{
		if(collectSound)
			AudioSource.PlayClipAtPoint(collectSound, transform.position);
		if (gui)
			gui.coinsCollected ++;
		Destroy(gameObject);
	}
}
