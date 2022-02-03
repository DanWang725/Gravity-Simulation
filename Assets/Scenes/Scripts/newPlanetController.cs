using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this controls the lesser (smaller planets)
public class newPlanetController : MonoBehaviour
{
    public Planet Planet;
	public decimal radius = 123;

	public Vector3 initVelocity = new Vector3(0f,0f,0f); //can be set in here or in editor

	//most of these are just for the observation values
	public float curVelMag, curAccelMag;
	public float kinEnerg;
	public float distanceFromPlanet;
	public float calculatedForce;
	public float centrifugalForce;
	public bool doTangentalSpeed = false;

	private GameObject[] hugePlanets;

	private float simTime = 1f;
	private bool doSim = true;
         
	//start up function, calculates the binding speed to orbit the planet
	Vector3 calculateOrbitalSpeed(GameObject pos1, decimal planetMass){

		float distance = Vector3.Distance(pos1.transform.position,transform.position);
		distance = distance * 10000;

		float G = 6.67f*Mathf.Pow(10,-11);

		float speed = Mathf.Sqrt((G*(float)planetMass)/distance);
		speed = speed/10000;
		return new Vector3(speed,0f,0f);
	}


    //calculating the gravitational energy for the current gravitational force
	void getGPE(float distance, decimal G, decimal planetMass){
		//tempGravEnergy = (float)((G*planetMass*Planet.Mass)/(decimal)distance); never use this!!

		curVelMag = Planet.Velocity.getVector().magnitude*10000;
		curAccelMag = Planet.Acceleration.getVector().magnitude*10000;
		centrifugalForce = ((curVelMag*curVelMag)/distanceFromPlanet)*(float)Planet.Mass;
	}

	//calculates the force without a vector (just force)
	decimal calculatePureForce(GameObject hugePlanetPos, decimal planetMass){
		//getting the distance between the object and the planet
		float distance = Vector3.Distance(hugePlanetPos.transform.position, Planet.Position.getVector());
		decimal G = (decimal)6.67f*(decimal)Mathf.Pow(10,-11);
		
		//adjusting distance to be the correct units - in the simulation 1 unit is 10km, 10000m
		distance = distance * 10000;
		distanceFromPlanet = (float)distance;

		getGPE(distance, G, planetMass); //only calculates stats for viewing

		float distanceSquared = distance * distance;

		//run time debug variables
		decimal force = (G*(decimal)planetMass*Planet.Mass/(decimal)distanceSquared)/10000;
		calculatedForce = (float)force*10000;
		return force;

	}
	//calculates the heading of the force
	public Vector3 calculateHeading(GameObject hugePlanetPos){
		Vector3 heading = (hugePlanetPos.transform.position - Planet.Position.getVector());//larger mass (huge planet tag) goes first
		return (heading/heading.magnitude);
	}

	//puts the force value and heading together into an array;
	void calculateForce(LargeCoords forceGrav, GameObject pos1, decimal planetMass){
		decimal pureForce = calculatePureForce(pos1, planetMass);
		Vector3 forceHeading = calculateHeading(pos1);
        forceGrav.setVal((float)pureForce * forceHeading);
	}

	//update the position in the Planet struct, not visual game object.
	void updatePos(decimal time){
        Planet.OldPos = Planet.Position;
        Planet.Position = Planet.OldPos + Planet.Velocity * time + Planet.Acceleration*time*time;
        Planet.Velocity = (Planet.Position - Planet.OldPos)/time;
	}

    public void setTangental(){

    	HugePlanetController pl = hugePlanets[0].GetComponent<HugePlanetController>();
    	Planet.Velocity.setVal(calculateOrbitalSpeed(hugePlanets[0], pl.objectMass));
    }
    // Start is called before the first frame update
    void Start()
    {
		hugePlanets = GameObject.FindGameObjectsWithTag("HighMass"); //assembling the large planets mass

    	EventManager.OnPause += pauseSim;
    	
		//setting the initial values of the planet
        Planet.Velocity.setVal(initVelocity);
    	Planet.OldPos.setVal(transform.position);
        Planet.Position.setVal(transform.position);
        Planet.Acceleration.setVal(0,0,0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if (!doSim){	//end here if sim is paused
			return;
		}
		
		decimal time = (decimal)simTime;
		
		LargeCoords fNet = new LargeCoords(0,0,0);
        LargeCoords forceGrav = new LargeCoords(0,0,0);

		//going through each planet with high mass and adding the gravitational force
		foreach (GameObject planet in hugePlanets)
		{
			HugePlanetController pl = planet.GetComponent<HugePlanetController>();
			calculateForce(forceGrav, planet, pl.objectMass);	//output is sent to forceGrav

            fNet += forceGrav; //adding results to fNet
			//do stuff here
		}

        Planet.Acceleration = fNet/Planet.Mass; //calculating the acceleration w/ fnet = ma

		updatePos(time);
		transform.position = Planet.Position.getVector();

		kinEnerg = (0.5f) * (float)Planet.Mass * curVelMag * curVelMag;
	}
    public void selectedThis(){
		Debug.Log("I am selected!");
    }

    public void unSelectedThis(){
		Debug.Log("I am Unselected!");

    }

    void pauseSim() => doSim = !doSim;
    
}
