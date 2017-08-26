using UnityEngine;
using System.Collections;

//simple "platformer enemy" AI
[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(DealDamage))]
public class EnemyAI : MonoBehaviour 
{
	public float acceleration = 35f;					//acceleration of enemy movement
	public float deceleration = 8f;						//deceleration of enemy movement
	public float rotateSpeed = 0.7f;					//how fast enemy can rotate
	public float speedLimit = 10f;						//how fast enemy can move
	public Vector3 bounceForce = new Vector3(0, 13, 0);	//force to apply to player when player jumps on enemies head
	public AudioClip bounceSound;						//sound when you bounce on enemies
	public float pushForce = 10f;						//how far away to push the player when they are attacked by the enemy
	public float pushHeight = 7f;						//how high to push the player when they are attacked by the enemy
	public int attackDmg = 1;							//how much damage to deal to the player when theyre attacked by this enem
	public bool chase = true;							//should this enemy chase objects inside its sight?
	public bool ignoreY = true;							//ignore Y axis when chasing? (this would be false for say.. a flying enemy)
	public float chaseStopDistance = 0.7f;				//stop this far away from object when chasing it
	public GameObject sightBounds;						//trigger for sight bounds
	public GameObject attackBounds;						//trigger for attack bounds (player is hurt when they enter these bounds)
	public Animator animatorController;					//object which holds the animator for this enem	
	public MoveToPoints moveToPointsScript;				//if you've attached this script, drag the component here
	
	private TriggerParent sightTrigger;
	private TriggerParent attackTrigger;
	private PlayerMove playerMove;
	private CharacterMotor characterMotor;
	private DealDamage dealDamage;
	
	
	//setup
	void Awake()
	{		
		characterMotor = GetComponent<CharacterMotor>();
		dealDamage = GetComponent<DealDamage>();
		//avoid setup errors
		if(tag != "Enemy")
		{
			tag = "Enemy";
			Debug.LogWarning("'EnemyAI' script attached to object without 'Enemy' tag, it has been assign automatically", transform);
		}
		
		if(sightBounds)
		{
			sightTrigger = sightBounds.GetComponent<TriggerParent>();
			if(!sightTrigger)
				Debug.LogError("'TriggerParent' script needs attaching to enemy 'SightBounds'", sightBounds);
		}
		if(!sightBounds)
			Debug.LogWarning("Assign a trigger with 'TriggerParent' script attached, to 'SightBounds' or enemy will not be able to see", transform);
		
		if(attackBounds)
		{
			attackTrigger = attackBounds.GetComponent<TriggerParent>();
			if(!attackTrigger)
				Debug.LogError("'TriggerParent' script needs attaching to enemy 'attackBounds'", attackBounds);
		}
		else
			Debug.LogWarning("Assign a trigger with 'TriggerParent' script attached, to 'AttackBounds' or enemy will not be able to attack", transform);
	}
	
	void Update()
	{
		//chase
		if (sightTrigger && sightTrigger.colliding && chase && sightTrigger.hitObject != null && sightTrigger.hitObject.activeInHierarchy)
		{
			characterMotor.MoveTo (sightTrigger.hitObject.transform.position, acceleration, chaseStopDistance, ignoreY);
			//nofity animator controller
			if(animatorController)
				animatorController.SetBool("Moving", true);
			//disable patrol behaviour
			if(moveToPointsScript)
				moveToPointsScript.enabled = false;
		}
		else
		{	
			//notify animator
			if(animatorController)
				animatorController.SetBool("Moving", false);
			//enable patrol behaviour
			if(moveToPointsScript)
				moveToPointsScript.enabled = true;
		}
		
		//attack
		if (attackTrigger && attackTrigger.collided)
		{
			dealDamage.Attack(attackTrigger.hitObject, attackDmg, pushHeight, pushForce);
			//notify animator controller
			if(animatorController)
				animatorController.SetBool("Attacking", true);	
		}
		else if(animatorController)
			animatorController.SetBool("Attacking", false);
	}
	
	void FixedUpdate()
	{
		characterMotor.ManageSpeed(deceleration, speedLimit, ignoreY);
		characterMotor.RotateToVelocity (rotateSpeed, ignoreY);
	}
	
	//bounce player when they land on this enemy
	public void BouncedOn()
	{	
		if(!playerMove)
			playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
		if (bounceSound)
			AudioSource.PlayClipAtPoint(bounceSound, transform.position);
		if(playerMove)
		{
			Vector3 bounceMultiplier = new Vector3(0f, 1.5f, 0f) * playerMove.onEnemyBounce;
			playerMove.Jump (bounceForce + bounceMultiplier);
		}
		else
			Debug.LogWarning("'Player' tagged object landed on enemy, but without playerMove script attached, is unable to bounce");
	}
}