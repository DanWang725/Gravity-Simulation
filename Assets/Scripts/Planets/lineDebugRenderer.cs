using UnityEngine;

namespace DanWang725.Planets
{
    public class lineDebugRenderer : MonoBehaviour
    {
        //this automatically destroys the line that is rendered after 5000 frames
        void Start()
        {
            Destroy(gameObject, 1);
        }
    }
}
