using System.Collections;
using System.Collections.Generic;
using DanWang725.Planets;
using DanWang725.UI;
using UnityEngine;

namespace DanWang725
{
    public class CreatePlanet : MonoBehaviour
    {
        public GameObject planetTemplate;
        public PlanetScrollList planetButtonManager;

        public int circleRange = 100;
        private bool _isPaused = true;
        private float _simSpeed = 1f;
        public float power = 1000f;

        // Start is called before the first frame update
        void Start()
        {
            EventManager.OnCreate += createPlanet;
            EventManager.OnPause += pauseSim;
            EventManager.OnChange += simChange;
        }

        void pauseSim() => _isPaused = !_isPaused;

        void simChange(float val)
        {
            _simSpeed = val;
        }

        private void createPlanet()
        {
            GameObject temp = Instantiate(planetTemplate, Random.insideUnitSphere * circleRange, planetTemplate.transform.rotation);
            planetButtonManager.CreateButtonForPlanet(temp);
            temp.SetActive(true);
            newPlanetController tempScript = temp.GetComponent<newPlanetController>();
            tempScript.IsPaused = _isPaused;
            tempScript.SimSpeed = _simSpeed;
            tempScript.initVelocity = (Random.insideUnitSphere/power);
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
