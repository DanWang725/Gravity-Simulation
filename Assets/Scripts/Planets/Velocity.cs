using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Velocity : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject followThis;
    public GameObject textHere;
    public TextMesh countText;
    private GameObject go;
    private PlanetController speedController;
    private float courrentSpeed;
    private Vector3 offset = new Vector3(1,0,0);

    void Start()
    {
        
    }

    // Update is called once per frame 
    void Update()
    {
        go = GameObject.Find("Planet");
		speedController = go.GetComponent<PlanetController>();
		courrentSpeed = speedController.velUp;

		countText.text = "Velocity: " + courrentSpeed.ToString() + "x10^-1 km/s";

		transform.position = followThis.transform.position +offset;

    }
}
