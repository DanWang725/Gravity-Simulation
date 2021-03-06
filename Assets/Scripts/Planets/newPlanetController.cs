using System.Collections.Generic;
using DanWang725.Core;
using DanWang725.UI;
using UnityEngine;

//this controls the lesser (smaller planets)
namespace DanWang725.Planets
{
	public class newPlanetController : MonoBehaviour
	{
		public Planet thisPlanet;
		public decimal radius = 123;

		public Vector3 initVelocity = new Vector3(0f,0f,0f); //can be set in here or in editor

		List<LineRenderer> linePlanets = new List<LineRenderer>();
		public GameObject linePlanetTemplate;

		public LineRenderer lineVel;
		//most of these are just for the observation values
		public float curVelMag, curAccelMag;
		public float kinEnerg;
		public float distanceFromPlanet;
		public float calculatedForce;
		public float centrifugalForce;
		public bool doTangentalSpeed = false;

		private GameObject[] hugePlanets;

		private float _simSpeed = 1f;
		private bool _isPaused = true;

		public float SimSpeed
		{
			get => _simSpeed;
			set => _simSpeed = value;
		}

		public bool IsPaused
		{
			get => _isPaused;
			set => _isPaused = value;
		}
		
		

		//start up function, calculates the binding speed to orbit the planet
		Vector3 calculateOrbitalSpeed(GameObject pos1, decimal planetMass){

			float distance = Vector3.Distance(pos1.transform.position,transform.position);
			distance = distance * 100000;

			float G = 6.67f*Mathf.Pow(10,-11);

			float speed = Mathf.Sqrt((G*(float)planetMass)/distance);
			speed = speed/100000;
			return new Vector3(speed,0f,0f);
		}


		//calculating the gravitational energy for the current gravitational force
		void getGPE(float distance, decimal planetMass){
			//tempGravEnergy = (float)((G*planetMass*Planet.Mass)/(decimal)distance); never use this!!

			curVelMag = thisPlanet.Velocity.getVector().magnitude*100000;
			curAccelMag = thisPlanet.Acceleration.getVector().magnitude*100000;
			centrifugalForce = ((curVelMag*curVelMag)/distanceFromPlanet)*(float)thisPlanet.Mass;
		}

		//update the position in the Planet struct, not visual game object.
		void updatePos(decimal time){
			thisPlanet.OldPos = thisPlanet.Position;
			thisPlanet.Position = thisPlanet.OldPos + thisPlanet.Velocity * time + thisPlanet.Acceleration*time*time;
			thisPlanet.Velocity = (thisPlanet.Position - thisPlanet.OldPos)/time;
			lineVel.SetPosition(1, thisPlanet.Velocity.getVector()*1000);
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
			thisPlanet.Mass = 34;
			thisPlanet.Velocity.setVal(initVelocity);
			thisPlanet.OldPos.setVal(transform.position);
			thisPlanet.Position.setVal(transform.position);
			thisPlanet.Acceleration.setVal(0,0,0);
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			while (linePlanets.Count != hugePlanets.Length)
			{
				GameObject tempLine = Instantiate(linePlanetTemplate, transform.position, transform.rotation);
				tempLine.transform.parent = transform;
				tempLine.SetActive(true);
				linePlanets.Add(tempLine.GetComponent<LineRenderer>());
			}

			int lineIndex = 0;
			if (!_isPaused){	//end here if sim is paused
				return;
			}
		
			decimal time = (decimal)_simSpeed;
		
			LargeCoords fNet = new LargeCoords(0,0,0);
			
			//going through each planet with high mass and adding the gravitational force
			foreach (GameObject planet in hugePlanets)
			{
				Planet pl = planet.GetComponent<HugePlanetController>().thisPlanet;
				if (Vector3.Distance(thisPlanet.Position.getVector(), pl.Position.getVector()) < 34)
				{
					GameObject.FindObjectOfType<PlanetScrollList>().RemoveButton(gameObject);
					
					Destroy(gameObject);
				}

				//calculateForce(forceGrav, planet, pl.objectMass);	//output is sent to forceGrav
				LargeCoords tempForce = MathPhysicsFormulas.CalculateGravitationalForceLargeCoord(pl, thisPlanet);
				linePlanets[lineIndex++].SetPosition(1,tempForce.getVector()*10000);
				
				fNet += tempForce; //adding results to fNet
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

		void pauseSim() => _isPaused = !_isPaused;

		void simChange(float val)
		{
			_simSpeed = val;
		}

	}
}
