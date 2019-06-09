using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

//modified from https://answers.unity.com/questions/478155/2d-when-player-gets-hit-make-invulnerable-for-a-fe.html
public class Damage : MonoBehaviour
{
    bool invincible = false;
    public int health = 100;
    public int damageFromEnemy = 5;
    public GameObject textMaker;
    public float invincibilityTime = 3.0f;
    public AudioClip damageSound;
    private AudioSource speaker;
    float timer;
    bool contact = false;
    // Use this for initialization
    void Start() {
        speaker = gameObject.GetComponent<AudioSource>();
    }

	IEnumerator Wait() {
		yield return new WaitForSeconds(invincibilityTime);
	}

    // This function gets called everytime this object collides with another trigger
    private void OnTriggerEnter2D(Collider2D collisionData) {
        //did we collide with an enemy?
		if (collisionData.gameObject.tag == "Enemy" || collisionData.gameObject.tag == "Rock") {
            contact = true;
			Hurt ();
        }
    }

    private void OnTriggerExit2D(Collider2D collisionData) {
		if(collisionData.gameObject.tag == "Enemy" || collisionData.gameObject.tag == "Rock") {
            contact = false;
        }
    }

    private void Hurt() {
        if (contact) {
            if (!invincible) { //if we're vulnerable
                //play damage sound
                speaker.PlayOneShot(damageSound, 1.0f);
                //take damage
                health -= damageFromEnemy;
                //make damage numbers appear
                textMaker.GetComponent<TextSpawner>().makeText("-" + damageFromEnemy.ToString(), new Color(1, 0, 0, 1), transform);
                //add seconds to the timer (which determines invincibility)
                timer = Time.time + invincibilityTime;
                //become vulnerable again
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (Time.time > timer)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            invincible = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.75f);
            invincible = true;
        }
        //if we ran out of health,
        if (health <= 0) {
            //go to the game over screen
            SceneManager.LoadScene("GameOver");
        }
    }
}