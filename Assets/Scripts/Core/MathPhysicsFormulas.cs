using System.Collections;
using System.Collections.Generic;
using DanWang725.Planets;
using UnityEngine;

namespace DanWang725.Core
{
    public static class MathPhysicsFormulas
    {

        //puts the force value and heading together into an array;
        /**
         * @param Planet1 should be the larger one
         */
        public static Vector3 CalculateGravitationalForce(Planet planet1, Planet planet2){
            Vector3 heading = planet1.Position.getVector() - planet2.Position.getVector();
            
            float distance = Vector3.Distance(planet1.Position.getVector(), planet2.Position.getVector()) * 100000;
            float distanceSquared = distance * distance;
            
            decimal force = (Constants.Physics.G*(decimal)planet1.Mass*planet2.Mass/(decimal)distanceSquared)/100000;
            
            return (float)force * heading/heading.magnitude;
        }
        
        public static LargeCoords CalculateGravitationalForceLargeCoord(Planet planet1, Planet planet2){
            Vector3 heading = planet1.Position.getVector() - planet2.Position.getVector();
            
            float distance = Vector3.Distance(planet1.Position.getVector(), planet2.Position.getVector()) * 100000;
            float distanceSquared = distance * distance;
            
            decimal force = (Constants.Physics.G*(decimal)planet1.Mass*planet2.Mass/(decimal)distanceSquared)/100000;
            
            return new LargeCoords((float)force * heading/heading.magnitude);
        }
    }
}
