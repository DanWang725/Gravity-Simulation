using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineDebugRenderer : MonoBehaviour
{
    //this automatically destroys the line that is rendered after 5000 frames
    void Start()
    {
        Destroy(gameObject, 5000);
    }
}
