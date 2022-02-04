using System;
using UnityEngine;

//stores the vector in a larger variable size to store more digits
namespace DanWang725.Planets
{
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
    
        public decimal x;
        public decimal y;
        public decimal z;

        //constructors
        public LargeCoords() => setVal(0,0,0);
        public LargeCoords(decimal forward, decimal up, decimal side) => setVal(forward,up,side);
        public LargeCoords(Vector3 vec) => setVal(vec);
    
        //operator overrides
        public static LargeCoords operator +(LargeCoords a, LargeCoords b){
            LargeCoords largeCoords = new LargeCoords(a.x + b.x, a.y + b.y, a.z + b.z);
            return largeCoords;
        }
        public static LargeCoords operator -(LargeCoords a, LargeCoords b){
            LargeCoords largeCoords = new LargeCoords(a.x - b.x, a.y - b.y, a.z - b.z);
            return largeCoords;
        }

        public static LargeCoords operator *(LargeCoords a, decimal b){
            a.x *= b;
            a.y *= b;
            a.z *= b;
            return a;
        }

        public static LargeCoords operator /(LargeCoords a, decimal b){
            if(b == 0){
                throw new DivideByZeroException();
            }
            a.x /= b;
            a.y /= b;
            a.z /= b;
            return a;
        }


    }
}
