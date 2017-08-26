using UnityEngine;

//attach this to any object which needs to deal damage to another object
public class DealDamage : MonoBehaviour 
{
	private Health health;
	
	//remove health from object and push it
	public void Attack(GameObject victim, int dmg, float pushHeight, float pushForce)
	{
		health = victim.GetComponent<Health>();		
		//push
		Vector3 pushDir = (victim.transform.position - transform.position);
		pushDir.y = 0f;
		pushDir.y = pushHeight * 0.1f;
		if (victim.GetComponent<Rigidbody>() && !victim.GetComponent<Rigidbody>().isKinematic)
		{
			victim.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
			victim.GetComponent<Rigidbody>().AddForce (pushDir.normalized * pushForce, ForceMode.VelocityChange);
			victim.GetComponent<Rigidbody>().AddForce (Vector3.up * pushHeight, ForceMode.VelocityChange);
		}
		//deal dmg
		if(health && !health.flashing)
			health.currentHealth -= dmg;
	}
}

/* NOTE: if you just want to push objects you could use this script but set damage to 0. (ie: a bouncepad)
 * if you want to restore an objects health, set the damage to a negative number (ie: a healing bouncepad!) */