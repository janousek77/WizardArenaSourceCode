using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconBehavior : MonoBehaviour {
    public Sprite fireIcon;
    public Sprite iceIcon;
    public Sprite windIcon;
    public Sprite lightningIcon;
    public Sprite earthIcon;
	
	// Update is called once per frame
	void Update () {
		if(GameObject.Find("wizard").GetComponent<CharacterMovement>().num == 0) {
            //num == 0 is fire, so show that
            GetComponent<Image>().sprite = fireIcon;
        }
        else if(GameObject.Find("wizard").GetComponent<CharacterMovement>().num == 1) {
            //num == 1 is ice, so show that
            GetComponent<Image>().sprite = iceIcon;
        }
        else if(GameObject.Find("wizard").GetComponent<CharacterMovement>().num == 2) {
            //num == 2 is wind, so show that
            GetComponent<Image>().sprite = windIcon;
        }
        else if(GameObject.Find("wizard").GetComponent<CharacterMovement>().num == 3) {
            //num == 3 is lightning, so show that
            GetComponent<Image>().sprite = lightningIcon;
        }
        else {
            //num == 4 is earth, but we also want it to display earth as a failsafe (in case we get an invalid num)
            GetComponent<Image>().sprite = earthIcon;
        }
	}
}
