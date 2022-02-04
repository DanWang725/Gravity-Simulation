using DanWang725.Planets;
using TMPro;
using UnityEngine;

namespace DanWang725.UI
{
    public class PlanetStat : MonoBehaviour
    {
        //public GameObject camera;
        public newPlanetController planetScript;
        public TextMeshProUGUI tmp;

        private bool displayValue = false;

        // Start is called before the first frame update
        void Start()
        {
            tmp = GetComponent<TextMeshProUGUI>();
            tmp.SetText("");
        }

        // Update is called once per frame
        void Update()
        {
            if(displayValue){
                tmp.SetText("Velocity {0:5}m/s\nAcceleration - {1:4}m/s2", planetScript.curVelMag, planetScript.curAccelMag);
            }
        }
        void disableTextDisplayPlanet(){
            displayValue = false;
            planetScript.unSelectedThis();
            tmp.SetText("");
        }
        //set the new planet to get values from
        void followThis(newPlanetController s){
            Debug.Log(s);
            planetScript = s;
            s.setTangental();
            displayValue = true;
        }
    }
}
