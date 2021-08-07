using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetStat : MonoBehaviour
{
    //public GameObject camera;
    private Text statText;
    private string textValue = "";
    public ObjectController planetScript;
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
        tmp.SetText("");
    }
    //set the new planet to get values from
    void followThis(ObjectController s){
        Debug.Log(s);
        planetScript = s;
        displayValue = true;
    }
}
