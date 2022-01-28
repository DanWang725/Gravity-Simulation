using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//depracated script
public class PlanetController : MonoBehaviour
{
	public GameObject planet1;
	public GameObject planet2;
	public Rigidbody planetRigidbody;
	private Vector3 planetPosition1;
	private Vector3 planetPosition2;

	private Vector3 originalPosition;
	private Vector3 newPosition;
	private decimal[] ogPos = new decimal[3];
	private decimal[] newPos = new decimal[3];
	private decimal[] plAccel = new decimal[3];
	private decimal[] plVel = new decimal[3];

	public float velUp;
	public float calculatedEV;
	public Vector3 newPosTest;
	public Vector3 plVelocity = new Vector3(0f,5f,0f);
	public Vector3 plAcceleration;

	public float planet1Radius = 340;

	public bool subTangentalSpeed = false;

	private decimal pureForce;
	public float accelerationFromGravity;
	//all down by a factor of 3
	public float planet1Mass = 1.37f*Mathf.Pow(10,20);
	public float planet2Mass = 0.12f;
	//public Vector3 initialVel = new Vector3(0f,5f,0f);
	public Vector3 forceResult;


	//will return a velocity that will be on the x direction
	public Vector3 calculateOrbitalSpeed(){
		planetPosition1 = planet1.transform.position;
		planetPosition2 = planet2.transform.position;

		float distance = Vector3.Distance(planetPosition1,planetPosition2);
		distance = distance * 10000;

		float G = 6.67f*Mathf.Pow(10,-11);

		float speed = Mathf.Sqrt((G*planet1Mass)/distance);
		speed = speed/10000;
		return new Vector3(speed,0f,0f);

	}


//gets the heading of the force of gravity
	public Vector3 calculateHeading(){
		Vector3 heading = (planet1.transform.position - planet2.transform.position);
		return (heading/heading.magnitude);
	}

	//gets the force of gravity, total force
	public decimal calculatePureForce(){
		planetPosition1 = planet1.transform.position;
		planetPosition2 = planet2.transform.position;

		float distance = Vector3.Distance(planetPosition1,planetPosition2);
		distance = distance * 10000;
		float distanceSquared = distance*distance;

		decimal G = (decimal)6.67f*(decimal)Mathf.Pow(10,-11);
		decimal force = (G*(decimal)planet1Mass*(decimal)planet2Mass/(decimal)distanceSquared)/10000;

		//if(distance < 3403000){
		//	force = -force;
		//}
		return force;
	}

//depracted, old function
	public Vector3 calculateForce(){
		planetPosition1 = planet1.transform.position;
		planetPosition2 = planet2.transform.position;

		float distance = Vector3.Distance(planetPosition1,planetPosition2);
		distance = distance * 10000;
		float distanceSquared = distance*distance;
		float G = 6.67f*Mathf.Pow(10,-11);
		float force = G*planet1Mass*planet2Mass/distanceSquared;

		Vector3 heading = (planetPosition1- planetPosition2);
		Vector3 forceVector = (force*(heading/heading.magnitude));

		return (forceVector);
	}

	void updatePos(int i,decimal time){
		ogPos[i] = newPos[i];
		newPos[i] = ogPos[i] + plVel[i]*time + plAccel[i];
		plVel[i] = ((newPos[i]-ogPos[i])/time);
	}

    // Start is called before the first frame updat 
    void Start()
    {

    	if(subTangentalSpeed){
    		plVelocity = calculateOrbitalSpeed();
    	}
    	plVel[0] = (decimal)plVelocity.x;
    	plVel[1] = (decimal)plVelocity.y;
    	plVel[2] = (decimal)plVelocity.z;
    	originalPosition = planet2.transform.position;

    	ogPos[0] = (decimal)originalPosition.x;
    	ogPos[1] = (decimal)originalPosition.y;
    	ogPos[2] = (decimal)originalPosition.z;

    	newPosition = planet2.transform.position;
    	newPos[0] = (decimal)newPosition.x;
    	newPos[1] = (decimal)newPosition.y;
    	newPos[2] = (decimal)newPosition.z;
        //planetRigidbody.AddForce(initialVel,ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    	decimal time = (decimal)1.0f;
    	/*
    	originalPosition = planet2.transform.position;

    	plAcceleration = calculateForce()/planet2Mass;
    	newPosTest = 0.5f*plAcceleration*time*time;

    	planet2.transform.position += (plVelocity*time+newPosTest);
    	newPosition = planet2.transform.position;
    	
    	plVelocity = (newPosition- originalPosition)/time;
		*/
		//originalPosition = newPosition;

    	/*
		plAcceleration = calculateForce();
		plAccel[0] = (decimal)0.5f * (decimal)plAcceleration.x/(decimal)planet2Mass*time*time;
		plAccel[1] = (decimal)0.5f * (decimal)plAcceleration.y/(decimal)planet2Mass*time*time;
		plAccel[2] = (decimal)0.5f * (decimal)plAcceleration.z/(decimal)planet2Mass*time*time;
		*/

		pureForce = calculatePureForce();
		accelerationFromGravity = (float)pureForce;
		plAccel[0] = (decimal)1f * (( (decimal)calculateHeading().x*pureForce) /(decimal)planet2Mass) *time*time;
		plAccel[1] = (decimal)1f * (( (decimal)calculateHeading().y*pureForce) /(decimal)planet2Mass) *time*time;
		plAccel[2] = (decimal)1f * (( (decimal)calculateHeading().z*pureForce) /(decimal)planet2Mass) *time*time;

		updatePos(0,time);
		updatePos(1,time);
		updatePos(2,time);
		velUp = (float)plVel[1];
    	//newPosTest = 0.5f*plAcceleration*time*time;
    	//newPosition = originalPosition + (plVelocity*time+newPosTest)/10000;

    	planet2.transform.position= new Vector3((float)newPos[0],(float)newPos[1],(float)newPos[2]);

    	//plVelocity = (newPosition- originalPosition)/time;


    	//forceResult = calculateForce()*0.1f; 
        //planetRigidbody.AddForce(forceResult,ForceMode.Impulse);
    }
}
