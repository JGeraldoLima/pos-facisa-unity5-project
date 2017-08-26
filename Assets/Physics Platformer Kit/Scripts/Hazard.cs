using UnityEngine;
using System.Collections;

//add this class to hazards such as lava or spikes, use "effectedTags" to choose which objects can be hurt by this hazard
[RequireComponent(typeof(DealDamage))]
[RequireComponent(typeof(AudioSource))]
public class Hazard : MonoBehaviour 
{
	public float pushForce = 25f;							//how far away from this object to push the victim when they hit this hazard
	public float pushHeight = 6f;							//how high to push victim when they are hit by this hazard
	public int damage = 1;									//damage to deal to victim when they hit this hazard
	public bool triggerEnter;								//are we checking for a trigger collision? (ie: hits a child trigger symbolising area of effect)
	public bool collisionEnter = true;						//are we checking for collider collision? (ie: hits the actual collider of the object)
	public string[] effectedTags = {"Player"};				//which objects are vulnerable to this hazard (tags)
	public AudioClip hitSound;								//sound to play when an object is hurt by this hazard
	
	private DealDamage dealDamage;
	private AudioSource aSource;

	//setup
	void Awake()
	{
		aSource = GetComponent<AudioSource>();
		aSource.playOnAwake = false;
		dealDamage = GetComponent<DealDamage>();
	}
	
	//if were checking for a physical collision, attack what hits this object
	void OnCollisionEnter(Collision col)
	{
		if(!collisionEnter)
			return;
		foreach(string tag in effectedTags)
			if(col.transform.tag == tag)
			{
				dealDamage.Attack (col.gameObject, damage, pushHeight, pushForce);
				if (hitSound)
				{
					aSource.clip = hitSound;
					aSource.Play();
				}
			}
	}
	
	//if were checking for a trigger enter, attack what enters the trigger
	void OnTriggerEnter(Collider other)
	{
		if(!triggerEnter)
			return;
		foreach(string tag in effectedTags)
			if(other.transform.tag == tag)
				dealDamage.Attack (other.gameObject, damage, pushHeight, pushForce);
	}
}

/* NOTE: a nice feature of unity is that the trigger enter check works with a child object trigger
 * so you might have a physical collider on the actual object, then a child trigger for the damage area
 * for example: a lawnmower which the player can stand on, and a blade on the front which damages objects */