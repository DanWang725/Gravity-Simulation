using System.Collections;
using System.Collections.Generic;
using DanWang725.Planets;
using UnityEngine;

namespace DanWang725
{
    public class CreatePlanet : MonoBehaviour
    {
        public GameObject planetTemplate;

        public int circleRange = 100;

        public float power = 0.001f;
        
        // Start is called before the first frame update
        void Start()
        {
            EventManager.OnCreate += createPlanet;
        }

        private void createPlanet()
        {
            GameObject temp = Instantiate(planetTemplate, Random.insideUnitSphere * circleRange, planetTemplate.transform.rotation);
            temp.SetActive(true);
            temp.GetComponent<newPlanetController>().initVelocity = (Random.insideUnitSphere*power);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
