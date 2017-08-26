using UnityEngine;

//this class holds movement functions for a rigidbody character such as player, enemy, npc..
//you can then call these functions from another script, in order to move the character
[RequireComponent(typeof(Rigidbody))]
public class CharacterMotor : MonoBehaviour 
{
	public bool sidescroller;		//freezes Z movement if true
	[HideInInspector]
	public Vector3 currentSpeed;
	[HideInInspector]
	public float DistanceToTarget;

	private Rigidbody rigid;

	void Awake()
	{
		rigid = GetComponent<Rigidbody>();
		//set up rigidbody constraints
		rigid.interpolation = RigidbodyInterpolation.Interpolate;
		if(sidescroller)
			rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
		else
			rigid.constraints = RigidbodyConstraints.FreezeRotation;
		//add frictionless physics material
		if(GetComponent<Collider>().material.name == "Default (Instance)")
		{
			PhysicMaterial pMat = new PhysicMaterial();
			pMat.name = "Frictionless";
			pMat.frictionCombine = PhysicMaterialCombine.Multiply;
			pMat.bounceCombine = PhysicMaterialCombine.Multiply;
			pMat.dynamicFriction = 0f;
			pMat.staticFriction = 0f;
			GetComponent<Collider>().material = pMat;
			Debug.LogWarning("No physics material found for CharacterMotor, a frictionless one has been created and assigned", transform);
		}
	}
	
	//move rigidbody to a target and return the bool "have we arrived?"
	public bool MoveTo(Vector3 destination, float acceleration, float stopDistance, bool ignoreY)
	{
		Vector3 relativePos = (destination - transform.position);
		if(ignoreY)
			relativePos.y = 0;
		
		DistanceToTarget = relativePos.magnitude;
		if (DistanceToTarget <= stopDistance)
			return true;
		else
			rigid.AddForce(relativePos.normalized * acceleration * Time.deltaTime, ForceMode.VelocityChange);
			return false;
	}
	
	//rotates rigidbody to face its current velocity
	public void RotateToVelocity(float turnSpeed, bool ignoreY)
	{	
		Vector3 dir;
		if(ignoreY)
			dir = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);
		else
			dir = rigid.velocity;
		
		if (dir.magnitude > 0.1)
		{
			Quaternion dirQ = Quaternion.LookRotation (dir);
			Quaternion slerp = Quaternion.Slerp (transform.rotation, dirQ, dir.magnitude * turnSpeed * Time.deltaTime);
			rigid.MoveRotation(slerp);
		}
	}
	
	//rotates rigidbody to a specific direction
	public void RotateToDirection(Vector3 lookDir, float turnSpeed, bool ignoreY)
	{
		Vector3 characterPos = transform.position;
		if(ignoreY)
		{
			characterPos.y = 0;
			lookDir.y = 0;
		}
		
		Vector3 newDir = lookDir - characterPos;
		Quaternion dirQ = Quaternion.LookRotation (newDir);
		Quaternion slerp = Quaternion.Slerp (transform.rotation, dirQ, turnSpeed * Time.deltaTime);
		rigid.MoveRotation (slerp);
	}
	
	// apply friction to rigidbody, and make sure it doesn't exceed its max speed
	public void ManageSpeed(float deceleration, float maxSpeed, bool ignoreY)
	{	
		currentSpeed = rigid.velocity;
		if (ignoreY)
			currentSpeed.y = 0;
		
		if (currentSpeed.magnitude > 0)
		{
			rigid.AddForce ((currentSpeed * -1) * deceleration * Time.deltaTime, ForceMode.VelocityChange);
			if (rigid.velocity.magnitude > maxSpeed)
				rigid.AddForce ((currentSpeed * -1) * deceleration * Time.deltaTime, ForceMode.VelocityChange);
		}
	}
}

/* NOTE: ManageSpeed does a similar job to simply increasing the friction property of a rigidbodies "physics material"
 * but this is unpredictable and can result in sluggish controls and things like gripping against walls as you walk/falls past them
 * it's not ideal for gameplay, and so we use 0 friction physics materials and control friction ourselves with the ManageSpeed function instead */

/* NOTE: when you use MoveTo, make sure the stopping distance is something like 0.3 and not 0
 * if it is 0, the object is likely to never truly reach the destination, and it will jitter on the spot as it
 * attempts to move toward the destination vector but overshoots it each frame
 */