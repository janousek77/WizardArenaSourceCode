using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveTextBehavior : MonoBehaviour {
    int wave;

    void Start() {
        wave = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemySpawner>().wave;
    }

	// Update is called once per frame
	void Update () {
		wave = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemySpawner>().wave;
		if(wave == 6)
			gameObject.GetComponent<Text>().text = "Boss Battle";
		else
        	gameObject.GetComponent<Text>().text = "Wave " + wave.ToString();
	}
}
