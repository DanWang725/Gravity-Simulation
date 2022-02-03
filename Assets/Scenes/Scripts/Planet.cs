using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    decimal mass = 123;

    LargeCoords _velocity = new LargeCoords();
    LargeCoords _acceleration = new LargeCoords();
    LargeCoords _position = new LargeCoords();
    LargeCoords _oldPos = new LargeCoords();

    public LargeCoords Position { get => _position; set => _position = value; }
    public LargeCoords OldPos { get => _oldPos; set => _oldPos = value; }
    public LargeCoords Velocity { get => _velocity; set => _velocity = value; }
    public LargeCoords Acceleration { get => _acceleration; set => _acceleration = value; }
    public decimal Mass { get => mass; set => mass = value; }


}
