using System.Collections;
using System.Collections.Generic;
using DanWang725.Planets;
using UnityEngine;

namespace DanWang725.UI
{
    public class PlanetViewer : MonoBehaviour
    {
        public GameObject _referencedObject;
        public GameObject ReferencedObject { get => _referencedObject; set => _referencedObject = value; }

        public CameraPlayer cameraScript;
        // Start is called before the first frame update
        void Start()
        {
            EventManager.OnFollow += FollowThis;
        }

        void FollowThis()
        {
            Debug.Log("My position is " + gameObject.GetComponent<RectTransform>().anchoredPosition);
            cameraScript.FollowThis(_referencedObject.transform);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
