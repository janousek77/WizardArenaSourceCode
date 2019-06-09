using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController : MonoBehaviour {
	private double potionTimeOnScreen;				//Amount of time the potion object stays on screen before being destroyed
	Renderer r;
    public GameObject textMaker;
    public AudioClip potionSound;
    private AudioSource speaker;

	void Start () {
        speaker = gameObject.GetComponent<AudioSource>();
		potionTimeOnScreen = Time.time + 10;		//Timer starts upon instatiation.
		r = GetComponent<Renderer>();
	}

	void Update () {
		if(potionTimeOnScreen < Time.time+3)
			StartCoroutine(Flasher());
		if (potionTimeOnScreen < Time.time)			//Once timer runs out destroys object
			Destroy (gameObject);
	}

	IEnumerator Flasher() {
		for (int i = 0; i < 5; i++) {
			r.enabled = true;
			yield return new WaitForSeconds(1f);
			r.enabled= false;
			yield return new WaitForSeconds(.1f);
		}
	}


	private void OnTriggerEnter2D(Collider2D otherCollider) {		//when potion collides with player
		string playerTag = otherCollider.gameObject.tag;
		if(playerTag == "Player") {									//verifies its collided with the player
			
			if (gameObject.name == "Health Potion(Clone)") {        //Adds health to player
                speaker.PlayOneShot(potionSound);
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<Damage>().health + 25 > 100) {
                    //show healing text equal to how much you were just healed
                    textMaker.GetComponent<TextSpawner>().makeText("+" + (100 - GameObject.FindGameObjectWithTag("Player").GetComponent<Damage>().health), new Color(0, 1, 0, 1), transform);
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Damage>().health = 100;
                }
                else
                {
                    //show healing text
                    textMaker.GetComponent<TextSpawner>().makeText("+25", new Color(0, 1, 0, 1), transform);
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Damage>().health += 25;
                }
			}

			if (gameObject.name == "Speed Up Potion(Clone)") {		//speeds up player for a set amount of seconds
                speaker.PlayOneShot(potionSound);
                //show speed up text
                textMaker.GetComponent<TextSpawner>().makeText("Speed Up!", new Color(1, 1, 0, 1), transform);
                GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterMovement> ().BASE_SPEED = 1.5f;
				GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterMovement> ().SpeedBoostTime = Time.time+5;
			}
			
			if (gameObject.name == "Cool down Potion(Clone)") {     //speeds up attack interval.
                speaker.PlayOneShot(potionSound);
                //show cool down text
                textMaker.GetComponent<TextSpawner>().makeText("Increased Fire Rate!", new Color(0, 1, 1, 1), transform);
                GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterMovement> ().PotionCoolDownReducerTime = Time.time + 5;
				GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterMovement> ().PotionCoolDownReducer = 0.5f;
			}

			if (gameObject.name == "Power Up Potion(Clone)" && GameObject.FindGameObjectsWithTag("Enemy") != null) {  //Checks to make sure enemies exist in screen and increase the damage done by attacks.
                speaker.PlayOneShot(potionSound);
                textMaker.GetComponent<TextSpawner>().makeText("Power Up!", new Color(1, 0, 1, 1), transform);
				GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterMovement> ().DamageIncreaseTime = Time.time + 5;
				GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterMovement> ().DamageMultiplier = 2;
			}
				
			Destroy(gameObject);
		}
	}
}
