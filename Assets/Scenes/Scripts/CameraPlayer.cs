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
    private Vector3 oldPos;

    private float sensitivity = 1.0f;
    private float movementSensitivity = 0.5f;

    public float maxYAngle = 80f;
    private Vector2 currentRotation;
    public Transform cameraFollow;
    public bool isFollowing = false;
    private bool isMovingTowards = false;
    private float dist;

    void Start()
    {
        transform.position = player.transform.position +offset;
        oldPos = cameraFollow.position;
    }



    // Update is called once per frame
    void Update()
    {
        go = GameObject.Find("Planet");
        speedController = go.GetComponent<PlanetController>();
        courrentSpeed = speedController.planetRigidbody.position;
        //offset.z = -5-courrentSpeed.y/20;
       // offset.y = -courrentSpeed.y*0.0001f;
        
        //handling camera rotation when the right mouse button is held down
        if(Input.GetButton("MoveCamera")){

            //doing the initial rotation based on mouse X and Y values
            transform.Rotate(0, Input.GetAxis("Mouse X")*sensitivity,0);
            transform.Rotate(-Input.GetAxis("Mouse Y")*sensitivity,0,0);

            currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
            currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
            currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
            currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
            currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
            Camera.main.transform.rotation = Quaternion.Euler(currentRotation.y,currentRotation.x,0);
        }

        //calculating the movement vectors based on movement keys
        float xMovement = Input.GetAxis("Vertical")*movementSensitivity;
        float yMovement = Input.GetAxis("Vertical Y")*movementSensitivity;
        float zMovement = Input.GetAxis("Horizontal")*movementSensitivity;
        transform.Translate(Vector3.forward * xMovement);
        transform.Translate(Vector3.up * yMovement);
        transform.Translate(Vector3.right * zMovement);
        //transform.position = transform.position + new Vector3(zMovement,yMovement,xMovement);

        //raycast for planet selection
        if ( Input.GetMouseButtonDown (0)){ 
           RaycastHit hit; 
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 

           //detecting if anything is selected/ if the ray collides with any object
           if ( Physics.Raycast (ray,out hit)) {

                //making sure it is the correct tag on the collided object
                if(hit.transform.tag == "SmallerMass"){
                    cameraFollow = hit.transform;
                    
                    Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object

                    if(isFollowing){
                        isFollowing = false;
                        isMovingTowards = false;
                    } else {
                        isFollowing = true;
                        isMovingTowards = true;
                        dist = Vector3.Distance(transform.position,cameraFollow.position);
                        //transform.position = cameraFollow.position + offset;
                        oldPos = cameraFollow.position;
                    }
                    
                }
             
           }
        }

        if(isMovingTowards){
            float step = dist * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, cameraFollow.position + offset, step);

            if (Vector3.Distance(transform.position, cameraFollow.position + offset) < 0.001f)
            {
                // Swap the position of the cylinder.
                isMovingTowards = false;
            }
        } 
        
        if(isFollowing){
            transform.position += cameraFollow.position - oldPos;
            oldPos = cameraFollow.position;
        }


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