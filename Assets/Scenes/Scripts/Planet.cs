using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Planet : MonoBehaviour
{
    decimal mass = 123;
    [SerializeField]
    LargeCoords _velocity;
    LargeCoords _acceleration;

    public LargeCoords Position { get => _position; set => _position = value; }
    public LargeCoords OldPos { get => _oldPos; set => _oldPos = value; }
    public LargeCoords Velocity { get => _velocity; set => _velocity = value; }
    public LargeCoords Acceleration { get => _acceleration; set => _acceleration = value; }
    public decimal Mass { get => mass; set => mass = value; }

    LargeCoords _position;

    LargeCoords _oldPos;
}
