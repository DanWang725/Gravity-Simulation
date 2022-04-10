using UnityEngine;

//this controls the lesser (smaller planets)
namespace DanWang725.Planets
{
	public class newPlanetController : MonoBehaviour
	{
		public Planet thisPlanet;
		public decimal radius = 123;

		public Vector3 initVelocity = new Vector3(0f,0f,0f); //can be set in here or in editor

		public LineRenderer linePlanet;

		public LineRenderer lineVel;
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
		void getGPE(float distance, decimal planetMass){
			//tempGravEnergy = (float)((G*planetMass*Planet.Mass)/(decimal)distance); never use this!!

			curVelMag = thisPlanet.Velocity.getVector().magnitude*10000;
			curAccelMag = thisPlanet.Acceleration.getVector().magnitude*10000;
			centrifugalForce = ((curVelMag*curVelMag)/distanceFromPlanet)*(float)thisPlanet.Mass;
		}

		//calculates the force without a vector (just force)
		decimal calculatePureForce(GameObject hugePlanetPos, decimal planetMass){
			//getting the distance between the object and the planet
			float distance = Vector3.Distance(hugePlanetPos.transform.position, thisPlanet.Position.getVector());

			//adjusting distance to be the correct units - in the simulation 1 unit is 10km, 10000m
			distance = distance * 10000;
			distanceFromPlanet = (float)distance;

			getGPE(distance, planetMass); //only calculates stats for viewing

			float distanceSquared = distance * distance;

			//run time debug variables
			decimal force = (Constants.Physics.G*(decimal)planetMass*thisPlanet.Mass/(decimal)distanceSquared)/10000;
			calculatedForce = (float)force*10000;
			return force;

		}
		//calculates the heading of the force
		public Vector3 calculateHeading(GameObject hugePlanetPos){
			Vector3 heading = (hugePlanetPos.transform.position - thisPlanet.Position.getVector());//larger mass (huge planet tag) goes first
			return (heading/heading.magnitude);
		}

		//puts the force value and heading together into an array;
		void calculateForce(LargeCoords forceGrav, GameObject pos1, decimal planetMass){
			decimal pureForce = calculatePureForce(pos1, planetMass);
			Vector3 forceHeading = calculateHeading(pos1);
			linePlanet.SetPosition(1, forceHeading * ((float)pureForce * 100));
			forceGrav.setVal((float)pureForce * forceHeading);
		}

		//update the position in the Planet struct, not visual game object.
		void updatePos(decimal time){
			thisPlanet.OldPos = thisPlanet.Position;
			thisPlanet.Position = thisPlanet.OldPos + thisPlanet.Velocity * time + thisPlanet.Acceleration*time*time;
			thisPlanet.Velocity = (thisPlanet.Position - thisPlanet.OldPos)/time;
			lineVel.SetPosition(1, thisPlanet.Velocity.getVector()*100);
		}

		public void setTangental(){

			HugePlanetController pl = hugePlanets[0].GetComponent<HugePlanetController>();
			thisPlanet.Velocity.setVal(calculateOrbitalSpeed(hugePlanets[0], pl.objectMass));
		}
		// Start is called before the first frame update
		void Start()
		{
			hugePlanets = GameObject.FindGameObjectsWithTag("HighMass"); //assembling the large planets mass

			EventManager.OnPause += pauseSim;
			EventManager.OnChange += simChange;
			
			//setting the initial values of the planet
			thisPlanet.Velocity.setVal(initVelocity);
			thisPlanet.OldPos.setVal(transform.position);
			thisPlanet.Position.setVal(transform.position);
			thisPlanet.Acceleration.setVal(0,0,0);
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

			thisPlanet.Acceleration = fNet/thisPlanet.Mass; //calculating the acceleration w/ fnet = ma

			updatePos(time);
			transform.position = thisPlanet.Position.getVector();

			kinEnerg = (0.5f) * (float)thisPlanet.Mass * curVelMag * curVelMag;
		}
		public void selectedThis(){
			Debug.Log("I am selected!");
		}

		public void unSelectedThis(){
			Debug.Log("I am Unselected!");

		}

		void pauseSim() => doSim = !doSim;

		void simChange(float val)
		{
			simTime = val;
		}

	}
}
