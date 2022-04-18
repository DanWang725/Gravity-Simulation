using DanWang725.Planets;
using DanWang725.UI;
using UnityEngine;

/*camera movement was used from this post in the unity forums: https://answers.unity.com/questions/1344322/free-mouse-rotating-camera.html
*/

namespace DanWang725
{
    public class CameraPlayer : MonoBehaviour
    {
        public PlanetStat display;
        // Start is called before the first frame update
        public GameObject player;
        private Vector3 offset = new Vector3(0,0,-7);
        private Vector3 oldPos;
        private Quaternion lookRot;

        private float sensitivity = 1.5f;
        private float movementSensitivity = 0.01f;

        public float maxYAngle = 80f;
        public Vector2 currentRotation;
        public Transform cameraFollow;
        public bool isFollowing = false;
        public bool isMovingTowards = false;
        private float dist;
        public GameObject debugLine;

        public GameObject pCanvas;

        public void stopFollowing()
        {
            isFollowing = false;
            isMovingTowards = false;
            display.disableTextDisplayPlanet();
        }
        
        public void FollowThis(Transform target)
        {
            if (cameraFollow != null && cameraFollow.Equals(target) && isFollowing)
            {
                isFollowing = false;
                isMovingTowards = false;
                //pCanvas.SendMessage("disableTextDisplayPlanet");
                display.disableTextDisplayPlanet();
            }
            else
            {
                var targetPos = target.position;
                Debug.Log("We want to move to " + targetPos + " We are at " + transform.position + " The vector between us is " + (targetPos - transform.position) +
                          " and");
                cameraFollow = target;
                isFollowing = true;
                isMovingTowards = true;
                oldPos = targetPos;
                
                transform.rotation = Quaternion.LookRotation(targetPos - transform.position, Vector3.up);
                lookRot = Quaternion.LookRotation(targetPos - transform.position, Vector3.up);
                dist = Vector3.Distance(transform.position,targetPos);
                display.followThis(target.gameObject.GetComponent<newPlanetController>());
                currentRotation.x = transform.rotation.eulerAngles.x;
                currentRotation.y = transform.rotation.eulerAngles.y;
            }
            
            
        }
        
        void Start()
        {
            transform.position = player.transform.position +offset;
            oldPos = cameraFollow.position;
            
        }

        // Update is called once per frame
        void Update()
        {
        
            //handling camera rotation when the right mouse button is held down
            if(Input.GetButton("MoveCamera"))
            {
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
            float xMovement = Input.GetAxis("Vertical")*movementSensitivity * (2 * ((Input.GetKey(KeyCode.LeftShift)) ? 5:1));
            float yMovement = Input.GetAxis("Vertical Y")*movementSensitivity * (2 * ((Input.GetKey(KeyCode.LeftShift)) ? 5:1));
            float zMovement = Input.GetAxis("Horizontal")*movementSensitivity * (2 * ((Input.GetKey(KeyCode.LeftShift)) ? 5:1));
            transform.Translate(Vector3.forward * xMovement);
            transform.Translate(Vector3.up * yMovement);
            transform.Translate(Vector3.right * zMovement);
            //transform.position = transform.position + new Vector3(zMovement,yMovement,xMovement);

            //raycast for planet selection
            if ( Input.GetMouseButtonDown (0)){
                Debug.Log("click");
                RaycastHit hit; 
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 

                //detecting if anything is selected/ if the ray collides with any object
                if ( Physics.Raycast (ray,out hit)) {

                    //making sure it is the correct tag on the collided object
                    if(hit.transform.CompareTag("SmallerMass")){
                        Vector3 direction = hit.point - ray.origin;
                        GameObject temp = Instantiate(debugLine, hit.point, Quaternion.LookRotation(direction, Vector3.up));
                        lookRot = Quaternion.LookRotation(direction, Vector3.up);
                        temp.SetActive(true);
                        cameraFollow = hit.transform;
                    
                        Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
                        FollowThis(hit.transform);
                        //if the camera is following a planet already, stop following
                        /*if(isFollowing){
                            isFollowing = false;
                            isMovingTowards = false;
                            //pCanvas.SendMessage("disableTextDisplayPlanet");
                            display.disableTextDisplayPlanet();
                        } else {    //if the camera is not following a planet currently, then start following it
                            FollowThis(hit.transform);
                            //pCanvas.SendMessage("followThis", hit.transform.gameObject.GetComponent<newPlanetController>());
                        }*/
                    
                    }
             
                }
            }

            
            //transition to following the planet
            
            if (cameraFollow == null && isFollowing)
            {
                stopFollowing();
            }
            
            if(isMovingTowards){
                float step = dist * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, cameraFollow.position + lookRot * offset, step);
                if (Vector3.Distance(transform.position, cameraFollow.position + lookRot * offset) < 0.001f)
                {
                    // Swap the position of the cylinder.
                    isMovingTowards = false;
                }
            } 

            //if the camera is supposed to be following the selected planet
            if(isFollowing){
                
                //moving the camera to the same coordinates it was in the previous position relative to the selected planet
                transform.position += cameraFollow.position - oldPos;
                oldPos = cameraFollow.position;
            }


        }
    }
    
    
}