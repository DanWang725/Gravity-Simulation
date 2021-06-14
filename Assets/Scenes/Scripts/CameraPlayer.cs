using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*camera movement was used from this post in the unity forums: https://answers.unity.com/questions/1344322/free-mouse-rotating-camera.html
*/

public class CameraPlayer : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject player;
    private GameObject go;
    private PlanetController speedController;
    private Vector3 courrentSpeed;
    private Vector3 offset = new Vector3(0,5,-7);
    private float sensitivity = 1.0f;
    private float movementSensitivity = 1.0f;

    public float maxYAngle = 80f;
    private Vector2 currentRotation;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        go = GameObject.Find("Planet");
        speedController = go.GetComponent<PlanetController>();
        courrentSpeed = speedController.planetRigidbody.position;
        //offset.z = -5-courrentSpeed.y/20;
       // offset.y = -courrentSpeed.y*0.0001f;
        transform.position = player.transform.position +offset;

        if(Input.GetButton("MoveCamera")){

            transform.Rotate(0, Input.GetAxis("Mouse X")*sensitivity,0);
            transform.Rotate(-Input.GetAxis("Mouse Y")*sensitivity,0,0);

            currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
            currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
            currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
            currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
            currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
            Camera.main.transform.rotation = Quaternion.Euler(currentRotation.y,currentRotation.x,0);
        }

        float xMovement = Input.GetAxis("Vertical")*movementSensitivity;
        float yMovement = Input.GetAxis("Vertical Y")*movementSensitivity;
        float zMovement = Input.GetAxis("Horizontal")*movementSensitivity;

        transform.position = transform.position + new Vector3(zMovement,yMovement,xMovement);

    }
}

//class to hold any basic planet stuff
/*
public abstract class PlanetV{
    private float mass;
    private float radius;
    private Vector3 planetPosition;

    //constructor
    public BasicPlanetStatistics(float mass, float radius, Vector3 planetPosition){
        this.mass = mass;
        this.radius = radius;
        this.planetPosition = planetPosition;
    }
    //these are getter methods to get values
    public float getM(){
        return mass;
    }
    public float getR(){
        return radius;
    }
    public Vector3 getP(){
        return planetPosition;
    }
}

public class MovingPlanet : PlanetV{
    private decimal[] ogPos = new decimal[3];
    private decimal[] newPos = new decimal[3];
    private decimal[] plAccel = new decimal[3];
    private decimal[] plVel = new decimal[3];

    public MovingPlanet(decimal[] velocity)
    : base(mass, radius, planetPosition)
    {
        plVel = velocity;
    }

    private void setPosToArr(){
        ogPos[0] = getP().x;
        ogPos[1] = getP().y;
        ogPos[2] = getP().z;
    }

    public void setAccel(decimal[] acceleration){
        plAccel = acceleration;
    }

    public decimal[] getAccel(){
        return plAccel;
    }

    public void setVel(decimal[] velocity){
        plVel = velocity;
    }

    public decimal[] getVel(){
        return plVel;
    }

}*/