using UnityEngine;
using UnityEngine.Windows.WebCam;

//handles the button event system

namespace DanWang725.Planets
{
    public class EventManager : MonoBehaviour
    {
        public delegate void ButtonEv();

        public delegate void SliderValue(float val);
        public static event ButtonEv OnPause;
        public static event SliderValue OnChange;
        public static event ButtonEv OnCreate;
        public static event ButtonEv OnFollow;
    
        public void buttonPause()
        {
            OnPause();
        }

        public void sliderChange(float val)
        {
            OnChange(val);
        }

        public void buttonCreate()
        {
            OnCreate();
        }

        public void buttonFollow()
        {
            OnFollow();
        }
    }
}
