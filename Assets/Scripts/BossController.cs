using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {
	public Transform target; 						//Players postion
	public Animator animator;						
	public GameObject[] potions;					//List of potions for enemy drops
	public GameObject textMaker;

	public float health = 100;
	public float speed = 3f;
	public float distance = 1f;
	public float spawnTime = 3f;
	public float shotRate = 2f;

	public float freezeTime;						//status affects time amount
	public float burnTime;

	public double DamageIncreaseTime;				//Amount of time for DamageMultiplier effect
	public float DamageMultiplier;					//Increase damage done to enemies

	private AudioSource speaker;
	public AudioClip damage;

	public GameObject[] MonsterArray;
	public GameObject projectile;

	public AudioClip entSpawn;
	public AudioClip entDie;
    public AudioClip rockThrow;

	void Start() {
		speaker = gameObject.GetComponent<AudioSource>();
		target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		InvokeRepeating ("Spawn", spawnTime, spawnTime);		//Repetitive spawning function
		InvokeRepeating("Shoot", shotRate, shotRate);
		if (gameObject.name == "Ent(Clone)") {
			speaker.PlayOneShot (entSpawn, 1.0f);
		}
	}
		

	void Update() {
		Animate ();

		transform.position = Vector2.MoveTowards (transform.position, target.position, speed * Time.deltaTime);
		DamageIncreaseTime = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterMovement> ().DamageIncreaseTime;
		DamageMultiplier = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterMovement> ().DamageMultiplier;
	
		if (DamageIncreaseTime < Time.time)
			DamageMultiplier = 1;

		if (Time.time > freezeTime && Time.time > burnTime) 					//if not burned or frozen, reset color
			GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
		
		else if (Time.time == 1 + burnTime || Time.time == 0.5 + burnTime) { 	//deal damage at start of burn time and halfway through
			health -= 0.25f;
			//make damage text appear
			textMaker.GetComponent<TextSpawner>().makeText("-0.25", new Color(1, 0.549f, 0, 1), transform);
		}

		if (health <= 0) {
            if (gameObject.name == "Ent(Clone)") {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().PlayOneShot(entDie, 1.0f);
            }
            //For Potion drops on enemies death and destroying the enemy object.
            int dropChance = Random.Range(0,100);           
			if(dropChance < 10){
				int dropChoice = Random.Range(0,4);
				Instantiate(potions[dropChoice], transform.position, Quaternion.identity);
			}

			GameObject.FindGameObjectWithTag ("GameController").GetComponent<EnemySpawner>().enemiesInWave.Remove(gameObject);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemySpawner>().BossDead();
            Destroy(gameObject);
		}
	}

	void Shoot(){
        speaker.PlayOneShot(rockThrow, 1f);
		Instantiate (projectile, transform.position, Quaternion.identity);
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
		} else {
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

	void Spawn () {
		if (health > 0) {											//Spawns as long as the pool number isn't 0
			int num = Random.Range(0, 4);
			Instantiate(MonsterArray[num], transform.position, Quaternion.identity); // instantiates enemies.
		}
	}

	private void OnCollisionEnter2D(Collision2D collisionData) {
		OnTriggerEnter2D(collisionData.collider);
	}

	private void OnTriggerEnter2D(Collider2D collisionData) {
		if (collisionData.gameObject.CompareTag ("Projectile")) {
			if (collisionData.gameObject.name == "fireball(Clone)"){
				health -= (2 * DamageMultiplier);
				textMaker.GetComponent<TextSpawner>().makeText("-" + 2 * DamageMultiplier, new Color(1, 0.549f, 0, 1), transform);  				//make damage text appear
				Destroy(collisionData.gameObject);
				GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0, 1);
				burnTime = 1 + Time.time;
				freezeTime = Time.time; 				//thaw yourself if frozen
			}

			if (collisionData.gameObject.name == "iceball(Clone)") {
				health -= (1 * DamageMultiplier);
				textMaker.GetComponent<TextSpawner>().makeText("-" + 1 * DamageMultiplier, new Color(0.176f, 0.392f, 0.971f, 1), transform);  		//make damage text appear
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
			if (collisionData.gameObject.name == "Lightning(Clone)") {
				health -= (2 * DamageMultiplier);
				textMaker.GetComponent<TextSpawner>().makeText("-" + 2 * DamageMultiplier, new Color(0.98f, 0.855f, 0.369f, 1), transform); 				//make damage text appear
			}
		}
	}
}
	
