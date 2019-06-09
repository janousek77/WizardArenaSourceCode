using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour {

	public List<GameObject> enemiesInWave = new List<GameObject>();				//Keeps track of monsters on screen
	public GameObject[] MonsterArray;											//The monsters that will be instantiated
	public GameObject Boss;
    public Transform target;													//Our Character
	bool bossOnField = false;
    public float spawnTime = 3f;												//How quickly a monster spawns
	public float pool = 20;													//How many monsters spawn per wave
	public int wave = 1;														//What wave we're in															

	Vector3 GeneratedPosition() {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();		//Players position
		float x, y;
		x = Random.Range(13,-13);															//Randomly chooses a location for spawning
		y = Random.Range(-12,0);
        if ( Mathf.Sqrt(Mathf.Pow(target.position.x - x, 2) + Mathf.Pow(target.position.y - y, 2)) < 2) { //Distance formula to make sure they are spawned at least 2 spaces away
            x = Random.Range(13, -13);
            y = Random.Range(-12, 0);
        }
        return new Vector3(x,y,0);
	}

	void Start () {
        InvokeRepeating ("Spawn", spawnTime, spawnTime);		//Repetitive spawning function
	}

	void waveHandler(){											//When we go to a new wave we set up a new pool count
		if(wave == 2)
			pool = 30;
		if(wave == 3)
			pool = 40;
		if(wave == 4 || wave == 5)
			pool = 50;
	}

	void Spawn () {
		if (pool > 0 && wave < 6) {											//Spawns as long as the pool number isn't 0
            int num = Random.Range(0, 4);
			enemiesInWave.Add( (GameObject)Instantiate(MonsterArray[num], GeneratedPosition(), Quaternion.identity)); // adds enemies to list and instantiates them.
            pool -= 1;											//decrease pool size
        } else if (pool == 0 && enemiesInWave.Count == 0) {            //Makes sure all monsters in wave are defeated
			if(wave < 6)
				wave++;													//increments to the next wave
			if(wave == 6 && bossOnField == false){
				Instantiate (Boss, new Vector3 (12.5f, -0.5f, 0f), Quaternion.identity);
				bossOnField = true;
			}
			waveHandler ();
		}
	}

    public void BossDead() {
        Invoke("Transition", 3.0f); //in three seconds, move to the victory screen
    }

    void Transition() {
        SceneManager.LoadScene("Victory");
    }
}
