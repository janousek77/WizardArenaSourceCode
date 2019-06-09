using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	public Transform target; 						//Players postion
	public Animator animator;						
	public GameObject[] potions;					//List of potions for enemy drops
    public GameObject textMaker;


	public float health = 10;
	public float speed = 3f;
	public float distance = 1f;

	public float freezeTime;						//status affects time amount
    public float burnTime;

	public double DamageIncreaseTime;				//Amount of time for DamageMultiplier effect
	public float DamageMultiplier;					//Increase damage done to enemies

    public AudioClip damage;
    public AudioClip freeze;
    public AudioClip ghostSpawn;
    public AudioClip ghostDie;
    public AudioClip skeletonSpawn;
    public AudioClip skeletonDie;
    public AudioClip wolfSpawn;
    public AudioClip wolfDie;
    public AudioClip zombieSpawn;
    public AudioClip zombieDie;

    private AudioSource speaker;

    void Start()
    {
        speaker = gameObject.GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (gameObject.name == "ghost(Clone)") {
            speaker.PlayOneShot(ghostSpawn, 1.0f);
        }
        else if(gameObject.name == "Skeleton(Clone)") {
            speaker.PlayOneShot(skeletonSpawn, 1.0f);
        }
        else if(gameObject.name == "Wolf(Clone)") {
            speaker.PlayOneShot(wolfSpawn, 1.0f);
        }
        else {
            speaker.PlayOneShot(zombieSpawn, 1.0f);
        }
    }

	void SetSpeed(){					//Increase enemy speed with each wave
		switch(GameObject.FindGameObjectWithTag ("GameController").GetComponent<EnemySpawner>().wave) {  
		case 2:
			speed += .0001f;
			break;
		case 3:
			speed += .00025f;
			break;
		case 4:
			speed += .0004f;
			break;
		case 5:
			speed += .0005f;
			break;
		case 6:
			speed += .0006f;
			break;
		default:
			break;
		}

	}

	void Update() {
		Animate ();
		SetSpeed ();

		DamageIncreaseTime = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterMovement> ().DamageIncreaseTime;
		DamageMultiplier = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterMovement> ().DamageMultiplier;

		if (DamageIncreaseTime < Time.time)
			DamageMultiplier = 1;

        if (Time.time > freezeTime && Time.time > burnTime) { 					//if not burned or frozen, reset color
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            CancelInvoke(); //stop dealing burn damage if we are no longer burned
        }

        if (Time.time > freezeTime) { //if not frozen
			transform.position = Vector2.MoveTowards (transform.position, target.position, speed * Time.deltaTime);        
		}

		if (health <= 0) {                                                  //For Potion drops on enemies death and destroying the enemy object.
            if (gameObject.name == "ghost(Clone)")
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().PlayOneShot(ghostDie, 1.0f);
            }
            else if (gameObject.name == "Skeleton(Clone)")
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().PlayOneShot(skeletonDie, 1.0f);
            }
            else if (gameObject.name == "Wolf(Clone)")
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().PlayOneShot(wolfDie, 1.0f);
            }
            else
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().PlayOneShot(zombieDie, 1.0f);
            }
            int dropChance = Random.Range(0,100);           
			if(dropChance < 10){
				int dropChoice = Random.Range(0,4);
				Instantiate(potions[dropChoice], transform.position, Quaternion.identity);
			}

			GameObject.FindGameObjectWithTag ("GameController").GetComponent<EnemySpawner>().enemiesInWave.Remove(gameObject);
            Destroy (gameObject);
		}
			
    }

	void Animate(){
		float yDir = target.position.y - transform.position.y;
		float xDir = target.position.x - transform.position.x;

		if (Mathf.Abs (xDir) > Mathf.Abs (yDir)) {
			if (xDir > 0) {
				animator.SetBool ("Right", true);
				animator.SetBool ("Left", false);
				animator.SetBool ("Up", false);
				animator.SetBool ("Down", false);
			} else {
				animator.SetBool ("Left", true);
				animator.SetBool ("Right", false);
				animator.SetBool ("Up", false);
				animator.SetBool ("Down", false);
			}
		}
        else {
			if (yDir > 0) {
				animator.SetBool ("Up", true);
				animator.SetBool ("Left", false);
				animator.SetBool ("Right", false);
				animator.SetBool ("Down", false);
			}
            else {
				animator.SetBool ("Down", true);
				animator.SetBool ("Right", false);
				animator.SetBool ("Up", false);
				animator.SetBool ("Left", false);
			}
		}
			
	}

    void BurnDamage() {
        speaker.PlayOneShot(damage, 1.0f);
        health -= 0.25f;
        //make damage text appear
        textMaker.GetComponent<TextSpawner>().makeText("-0.25", new Color(1, 0.549f, 0, 1), transform);
    }

	private void OnCollisionEnter2D(Collision2D collisionData) {
		OnTriggerEnter2D(collisionData.collider);
	}

	private void OnTriggerEnter2D(Collider2D collisionData) {
		if (collisionData.gameObject.CompareTag ("Projectile")) {
			if (collisionData.gameObject.name == "fireball(Clone)"){
                speaker.PlayOneShot(damage, 1.0f);
				health -= (2 * DamageMultiplier);
                //make damage text appear
                textMaker.GetComponent<TextSpawner>().makeText("-" + 2 * DamageMultiplier, new Color(1, 0.549f, 0, 1), transform);
                Destroy(collisionData.gameObject);
               GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0, 1);
               burnTime = 1 + Time.time;
               InvokeRepeating("BurnDamage", 0.25f, 0.5f);
               freezeTime = Time.time; 				//thaw yourself if frozen
            }
			if (collisionData.gameObject.name == "iceball(Clone)") {
                speaker.PlayOneShot(freeze, 1.0f);
                health -= (1 * DamageMultiplier);
                //make damage text appear
                textMaker.GetComponent<TextSpawner>().makeText("-" + 1 * DamageMultiplier, new Color(0.176f, 0.392f, 0.971f, 1), transform);
                freezeTime = 1 + Time.time;
                burnTime = Time.time; 				//extinguish burns
                GetComponent<SpriteRenderer>().color = new Color(0, 0, 0.5f, 1);
                Destroy(collisionData.gameObject);
            }
			if (collisionData.gameObject.name == "wind(Clone)") {
				Vector2 direction = (transform.position - collisionData.transform.position).normalized;
				GetComponent<Rigidbody2D>().AddRelativeForce(direction * 200);  //pushes enemies back
                Destroy(collisionData.gameObject);
            }
            if (collisionData.gameObject.name == "Lightning(Clone)")
            {
                speaker.PlayOneShot(damage, 1.0f);
                health -= (2 * DamageMultiplier);
                textMaker.GetComponent<TextSpawner>().makeText("-" + 2 * DamageMultiplier, new Color(0.98f, 0.855f, 0.369f, 1), transform);
            }
        }
	}
}