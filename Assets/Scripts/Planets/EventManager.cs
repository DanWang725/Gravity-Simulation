using UnityEngine;

//handles the button event system

namespace DanWang725.Planets
{
    public class EventManager : MonoBehaviour
    {
        public delegate void PauseButton();
        public static event PauseButton OnPause;

        public void buttonPause()
        {
            OnPause();
        }
    }
}
