using UnityEngine;

//simple class to add to checkpoint triggers
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class Checkpoint : MonoBehaviour 
{
	public Color activeColor = Color.green;	//color when checkpoint is activated
	public float activeColorOpacity = 0.4f;	//opacity when checkpoint is activated
	
	private Health health;
	private Color defColor;
	private GameObject[] checkpoints;
	private Renderer render;
	private AudioSource aSource;

	//setup
	void Awake()
	{
		render = GetComponent<Renderer>();
		aSource = GetComponent<AudioSource>();
		if(tag != "Respawn")
		{
			tag = "Respawn";
			Debug.LogWarning ("'Checkpoint' script attached to object without the 'Respawn' tag, tag has been assigned automatically", transform);	
		}
		GetComponent<Collider>().isTrigger = true;
		
		if(render)
			defColor = render.material.color;
		activeColor.a = activeColorOpacity;
	}
	
	//more setup
	void Start()
	{
		checkpoints = GameObject.FindGameObjectsWithTag("Respawn");
		health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
		if(!health)
			Debug.LogError("For Checkpoint to work, the Player needs 'Health' script attached", transform);
	}
	
	//set checkpoint
	void OnTriggerEnter(Collider other)
	{
		if(other.transform.tag == "Player" && health)
		{
			//set respawn position in players health script
			health.respawnPos = transform.position;
			
			//toggle checkpoints
			if(render.material.color != activeColor)
			{
				foreach (GameObject checkpoint in checkpoints)
					checkpoint.GetComponent<Renderer>().material.color = defColor;
				aSource.Play();
				render.material.color = activeColor;
			}
		}
	}
}