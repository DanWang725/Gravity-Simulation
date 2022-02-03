using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Planet : MonoBehaviour
{
    decimal mass = 123;
    [SerializeField]
    LargeCoords _velocity;
    LargeCoords _position;
    LargeCoords _oldPos;
}
