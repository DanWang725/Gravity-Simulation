using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this controls the lesser (smaller planets)
public class ObjectController : MonoBehaviour
{
	public decimal objectMass = 120;
	public decimal radius = 123;

	public Vector3 initVelocity = new Vector3(0f,0f,0f);

	private decimal[] forceGrav = new decimal[3];//this should be for the current force of grav (in a for each loop)
	//private decimal[] fNet = new decimal[3]; //add the forceGrav to this

	private decimal[] velocity = new decimal[3];
	private decimal[] prevPos = new decimal[3];
	private decimal[] newPos = new decimal[3];

	public float curVelMag, curAccelMag;
	public float kinEnerg;
	public float[] gravPotEnerg = new float[3]; 
	public float tempGravEnergy;
	public float distanceFromPlanet;
	public float calculatedForce;
	public float centrifugalForce;
	public bool doTangentalSpeed = false;

	private decimal[] acceleration = new decimal[3];

	private GameObject[] hugePlanets;
         
	//start up function, calculates the binding speed to orbit the planet
	public Vector3 calculateOrbitalSpeed(GameObject pos1, decimal planetMass){

		float distance = Vector3.Distance(pos1.transform.position,transform.position);
		distance = distance * 10000;

		float G = 6.67f*Mathf.Pow(10,-11);

		float speed = Mathf.Sqrt((G*(float)planetMass)/distance);
		speed = speed/10000;
		return new Vector3(speed,0f,0f);
	}


    //calculating the gravitational energy for the current gravitational force
	public void getGPE(float distance, decimal G, decimal planetMass){
		tempGravEnergy = (float)((G*planetMass*objectMass)/(decimal)distance);

		curVelMag = new Vector3((float)velocity[0],(float)velocity[1],(float)velocity[2]).magnitude*10000;
		curAccelMag = new Vector3((float)acceleration[0],(float)acceleration[1],(float)acceleration[2]).magnitude*10000;
		centrifugalForce = ((curVelMag*curVelMag)/distanceFromPlanet)*(float)objectMass;
	}

	//calculates the force without a vector (just force)
	public decimal calculatePureForce(GameObject pos1, decimal planetMass){
		//getting the distance between the object and the planet
		float distance = Vector3.Distance(pos1.transform.position, transform.position);
		decimal G = (decimal)6.67f*(decimal)Mathf.Pow(10,-11);
		
		//adjusting distance to be the correct units - in the simulation 1 unit is 10km, 10000m
		distance = distance * 10000;
		distanceFromPlanet = (float)distance;
		getGPE(distance, G, planetMass);

		float distanceSquared = distance * distance;

		//run time debug variables
		decimal force = (G*(decimal)planetMass*(decimal)objectMass/(decimal)distanceSquared)/10000;
		calculatedForce = (float)force*10000;
		return force;

	}
	//calculates the heading of the force
	public Vector3 calculateHeading(GameObject pos1){
		Vector3 heading = (pos1.transform.position - transform.position);//larger mass (huge planet tag) goes first
		return (heading/heading.magnitude);
	}

	//puts the force value and heading together into an array;
	public void calculateForce(decimal[] force, GameObject pos1, decimal planetMass){
		decimal pureForce = calculatePureForce(pos1, planetMass);
		Vector3 forceHeading = calculateHeading(pos1);
		for(int i = 0; i < 3;i++){
			force[i] = pureForce*(decimal)forceHeading[i];
		}
	}

	//update the position in the position arrray, not visual game object.
	public void updatePos(decimal time){
		for(int i = 0;i<3;i++){
			prevPos[i] = newPos[i];
			newPos[i] = prevPos[i] + velocity[i] * time + acceleration[i]*time*time; //d = v1*T + a*T^2
			velocity[i] = (newPos[i] - prevPos[i])/time;
		}
		
	}

    // Start is called before the first frame update
    void Start()
    {

    	if(doTangentalSpeed){
    		hugePlanets = GameObject.FindGameObjectsWithTag("HighMass");
    		HugePlanetController pl = hugePlanets[0].GetComponent<HugePlanetController>();
    		initVelocity = calculateOrbitalSpeed(hugePlanets[0], pl.objectMass);
    	}
    	//going through the array to set values, with given position and initial velocity 
    	for(int i = 0; i<3;i++){
    		newPos[i] = prevPos[i] = (decimal)transform.position[i];
    		velocity[i] = (decimal)initVelocity[i];
    	}
    	
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    	decimal time = (decimal)1f;
    	hugePlanets = GameObject.FindGameObjectsWithTag("HighMass");
    	//Debug.Log(hugePlanets.Length);
    	decimal[] fNet = new decimal[3];
    	int counter = 0;
    	
    	//going through each planet with high mass and adding the gravitational force
    	foreach(GameObject planet in hugePlanets) {
    		HugePlanetController pl = planet.GetComponent<HugePlanetController>();
    		//decimal pureForce = calculatePureForce(planet, planet.objectMass);

    		calculateForce(forceGrav, planet, pl.objectMass);

    		gravPotEnerg[counter] = tempGravEnergy;
    		counter++;
    		//do stuff with forcegrav here
    		for(int i = 0; i< 3; i++){
    			fNet[i] += forceGrav[i];
    		}

        	//do stuff here
    	}
    	for(int i = 0; i<3;i++){
    		acceleration[i] = (fNet[i]/objectMass);
    	}
    	
    	updatePos(time);
    	transform.position = new Vector3((float)newPos[0],(float)newPos[1],(float)newPos[2]);

    	kinEnerg = (0.5f)*(float)objectMass*curVelMag*curVelMag;


    }
}
