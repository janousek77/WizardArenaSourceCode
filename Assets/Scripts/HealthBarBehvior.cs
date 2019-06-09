using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehvior : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        GetComponent<Image>().fillAmount = (float) GameObject.Find("wizard").GetComponent<Damage>().health / 100.0f;
    }
}
