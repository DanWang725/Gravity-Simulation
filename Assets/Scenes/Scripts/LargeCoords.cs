using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stores the vector in a larger variable size to store more digits
public class LargeCoords
{
    public setVal(decimal forward, decimal up, decimal side){
        x = forward;
        y = up;
        z = side;
    }
    public setVal(Vector3 vec){
        x = (decimal)vec[0];
        y = (decimal)vec[1];
        z = (decimal)vec[2];
    }
    decimal x;
    decimal y;
    decimal z;

    public LargeCoords(decimal forward, decimal up, decimal side) => setVal(forward,up,side);
    public LargeCoords(Vector3 vec) => setVal(vec);
    


}
