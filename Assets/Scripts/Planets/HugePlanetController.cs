using DanWang725.UI;
using UnityEngine;

namespace DanWang725.Planets
{
    public class HugePlanetController : MonoBehaviour
    {
        // Start is called before the first frame update
        public decimal objectMass = (decimal)6.37*(decimal)Mathf.Pow(10,23);
        public Planet thisPlanet;
        void Start()
        {
            thisPlanet.Mass = objectMass;
            thisPlanet.Position.setVal(transform.position);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
