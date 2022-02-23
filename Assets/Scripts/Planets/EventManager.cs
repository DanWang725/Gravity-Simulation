using UnityEngine;

//handles the button event system

namespace DanWang725.Planets
{
    public class EventManager : MonoBehaviour
    {
        public delegate void PauseButton();

        public delegate void SliderValue(float val);
        public static event PauseButton OnPause;
        public static event SliderValue OnChange;
    
        public void buttonPause()
        {
            OnPause();
        }

        public void sliderChange(float val)
        {
            OnChange(val);
        }
    }
}
