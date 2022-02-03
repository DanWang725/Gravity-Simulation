using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//clean this up more in the future, possibly replace it
//this controls the lesser (smaller planets)
public class newPlanetController : MonoBehaviour
{
    public Planet Planet;
	//public decimal objectMass = 120;	//this is in kg
	public decimal radius = 123;

	public Vector3 initVelocity = new Vector3(0f,0f,0f); //can be set in here or in editor

//this can prob be not global..
	private decimal[] forceGrav = new decimal[3];//this should be for the current force of grav (in a for each loop)
	//private decimal[] fNet = new decimal[3]; //add the forceGrav to this

	//most of these are just for the observation values
	public float curVelMag, curAccelMag;
	public float kinEnerg;
	public float[] gravPotEnerg = new float[3]; 
	public float tempGravEnergy;
	public float distanceFromPlanet;
	public float calculatedForce;
	public float centrifugalForce;
	public bool doTangentalSpeed = false;

	private GameObject[] hugePlanets;

	private float simTime = 1f;
	private bool doSim = true;
         
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
		tempGravEnergy = (float)((G*planetMass*Planet.Mass)/(decimal)distance);

		curVelMag = Planet.Velocity.getVector().magnitude*10000;
		curAccelMag = Planet.Acceleration.getVector().magnitude*10000;
		centrifugalForce = ((curVelMag*curVelMag)/distanceFromPlanet)*(float)Planet.Mass;
	}

	//calculates the force without a vector (just force)
	public decimal calculatePureForce(GameObject pos1, decimal planetMass){
		//getting the distance between the object and the planet
		float distance = Vector3.Distance(pos1.transform.position, Planet.Position.getVector());
		decimal G = (decimal)6.67f*(decimal)Mathf.Pow(10,-11);
		
		//adjusting distance to be the correct units - in the simulation 1 unit is 10km, 10000m
		distance = distance * 10000;
		distanceFromPlanet = (float)distance;
		getGPE(distance, G, planetMass);

		float distanceSquared = distance * distance;

		//run time debug variables
		decimal force = (G*(decimal)planetMass*Planet.Mass/(decimal)distanceSquared)/10000;
		calculatedForce = (float)force*10000;
		return force;

	}
	//calculates the heading of the force
	public Vector3 calculateHeading(GameObject pos1){
		Vector3 heading = (pos1.transform.position - Planet.Position.getVector());//larger mass (huge planet tag) goes first
		return (heading/heading.magnitude);
	}

	//puts the force value and heading together into an array;
	public void calculateForce(LargeCoords force, GameObject pos1, decimal planetMass){
		decimal pureForce = calculatePureForce(pos1, planetMass);
		Vector3 forceHeading = calculateHeading(pos1);
        force.setVal((float)pureForce * forceHeading);
	}

	//update the position in the position arrray, not visual game object.
	public void updatePos(decimal time){
        Planet.OldPos = Planet.Position;
        Planet.Position = Planet.OldPos + Planet.Velocity * time + Planet.Acceleration*time*time;
        Planet.Velocity = (Planet.Position - Planet.OldPos)/time;
        /*
		for(int i = 0;i<3;i++){
			prevPos[i] = newPos[i];
			newPos[i] = prevPos[i] + velocity[i] * time + acceleration[i]*time*time; //d = v1*T + a*T^2
			velocity[i] = (newPos[i] - prevPos[i])/time;
		}*/
	}

    void setTangental(){
        hugePlanets = GameObject.FindGameObjectsWithTag("HighMass");
    	HugePlanetController pl = hugePlanets[0].GetComponent<HugePlanetController>();
    	Planet.Velocity.setVal(calculateOrbitalSpeed(hugePlanets[0], pl.objectMass));
    }
    // Start is called before the first frame update
    void Start()
    {
    	EventManager.OnPause += pauseSim;
    	//going through the array to set values, with given position and initial velocity 
        Planet.Velocity.setVal(initVelocity);
    	Planet.OldPos.setVal(transform.position);
        Planet.Position.setVal(transform.position);
        Planet.Acceleration.setVal(0,0,0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if (!doSim){
			return;
		}
		
		decimal time = (decimal)simTime;
		hugePlanets = GameObject.FindGameObjectsWithTag("HighMass");
		//Debug.Log(hugePlanets.Length);
		LargeCoords fNet = new LargeCoords(0,0,0);
        LargeCoords forceGrav = new LargeCoords(0,0,0);
		int counter = 0;

		//going through each planet with high mass and adding the gravitational force
		foreach (GameObject planet in hugePlanets)
		{
			HugePlanetController pl = planet.GetComponent<HugePlanetController>();
			//decimal pureForce = calculatePureForce(planet, planet.objectMass);

			calculateForce(forceGrav, planet, pl.objectMass);

			gravPotEnerg[counter] = tempGravEnergy;
			counter++;
			//do stuff with forcegrav here
            fNet += forceGrav;
			//do stuff here
		}

        Planet.Acceleration = fNet/Planet.Mass; //calculating the acceleration w/ fnet = ma

		updatePos(time);
		transform.position = Planet.Position.getVector();

		kinEnerg = (0.5f) * (float)Planet.Mass * curVelMag * curVelMag;
	}
    //is called when this planet is clicked on
    public void selectedThis(){
		Debug.Log("I am selected!");
    }

    //is called when this planet is unselected
    public void unSelectedThis(){
		Debug.Log("I am Unselected!");
    }

    void pauseSim() => doSim = !doSim;
    
}
