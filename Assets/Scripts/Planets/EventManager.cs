using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//handles the button event system

public class EventManager : MonoBehaviour
{
    public delegate void PauseButton();
    public static event PauseButton OnPause;

    public void buttonPause(){
        OnPause();
    }
}
