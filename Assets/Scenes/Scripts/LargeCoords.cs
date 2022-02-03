using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stores the vector in a larger variable size to store more digits
public class LargeCoords
{
    //Sets the values of the coordinates to the input
    public void setVal(decimal forward, decimal up, decimal side){
        x = forward;
        y = up;
        z = side;
    }
    //Sets the values of the coordinates with the given vector3
    public void setVal(Vector3 vec){
        x = (decimal)vec[0];
        y = (decimal)vec[1];
        z = (decimal)vec[2];
    }
    public Vector3 getVector(){
        return new Vector3((float)x,(float)y,(float)z);
    }
    
    decimal x;
    decimal y;
    decimal z;

    public LargeCoords(decimal forward, decimal up, decimal side) => setVal(forward,up,side);
    public LargeCoords(Vector3 vec) => setVal(vec);
    


}
