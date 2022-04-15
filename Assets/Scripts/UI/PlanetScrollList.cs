using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanWang725.UI
{
    public class PlanetScrollList : MonoBehaviour
    {
        //each button is 30 high, with buffer of 5 between top and bottom
        private List<GameObject> planetButtons = new List<GameObject>();
        public GameObject templateButton;
        public RectTransform scrollView;
        public RectTransform contentBox;

        public void RemoveButton(GameObject pl)
        {
            Debug.Log("We were called to remove a button");
            int index = -1;
            foreach (var tempBttn in planetButtons)
            {
                index++;
                if (pl.Equals(tempBttn.GetComponent<PlanetViewer>().ReferencedObject))
                {
                    break;
                }
            }
            if (index != -1)
            {
                GameObject tempBttn = planetButtons[index];
                planetButtons.RemoveAt(index);
                Destroy(tempBttn);
                RearrangeButtons();
            }
        }

        private void RearrangeButtons()
        {
            int newHeight = planetButtons.Count * 40;
            int anchorY = -10;
            contentBox.sizeDelta = new Vector2(contentBox.sizeDelta.x,newHeight);
            foreach (var bttn in planetButtons)
            {
                bttn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, anchorY);
                anchorY += 40;
            }
        }

        public void CreateButtonForPlanet(GameObject planet)
        {
            GameObject tempBttn = Instantiate(templateButton);
            tempBttn.transform.SetParent(contentBox.transform);
            tempBttn.SetActive(true);
            
            tempBttn.GetComponent<PlanetViewer>().ReferencedObject = planet;
            planetButtons.Add(tempBttn);
            RearrangeButtons();
        }

        // Start is called before the first frame update
        void Start()
        {
            contentBox = transform.gameObject.GetComponent<RectTransform>();
            GameObject[] tempPlanets = GameObject.FindGameObjectsWithTag("SmallerMass");
            foreach (var pl  in tempPlanets)
            {
                CreateButtonForPlanet(pl);
            }
        }
    
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
