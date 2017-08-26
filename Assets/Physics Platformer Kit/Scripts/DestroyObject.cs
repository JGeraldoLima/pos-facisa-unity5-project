using UnityEngine;
using System.Collections;

//basic utility class to destroy objects on startup
public class DestroyObject : MonoBehaviour 
{
	public AudioClip destroySound;	//sound to play when object is destroyed
	public float delay;				//delay before object is destroyed
	public bool destroyChildren;	//should the children be detached (and kept alive) before object is destroyed?
	public float pushChildAmount;	//push children away from centre of parent
	
	
	void Start()
	{
		//get list of children
		Transform[] children = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
			children[i] = transform.GetChild(i);
		
		//detach children
		if (!destroyChildren)
			transform.DetachChildren();
		
		//add force to children (and a bit of spin)
		foreach (Transform child in children)
		{
			Rigidbody rigid = child.GetComponent<Rigidbody>();
			if(rigid && pushChildAmount != 0)
			{
				Vector3 pushDir = child.position - transform.position;
				rigid.AddForce(pushDir * pushChildAmount, ForceMode.Force);
				rigid.AddTorque(Random.insideUnitSphere, ForceMode.Force);
			}
		}
		
		//destroy  parent
		if(destroySound)
			AudioSource.PlayClipAtPoint(destroySound, transform.position);
		Destroy (gameObject, delay);
	}
}