using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
	public float BASE_SPEED = 1.0f;					
	public float CROSSHAIR_DIST = 1.0f;

	public Vector2 movementDirection;
	public float movementSpeed;
	public float PotionCoolDownReducer;				//Amount of time to reduce the fire rate

	public Rigidbody2D rb;							//For moving the character
	public Animator animator;						//Controls character animation
	public GameObject crosshair;					//crosshair
    public GameObject[] weapons;					//list of attacks


	private double NextFire;						//times for cool downs between attacks
	private double NextIce;
	private double NextWind;
    private double NextLightning;

	public double SpeedBoostTime;					//Amount of time for player speed boost
	public double PotionCoolDownReducerTime;		//Amount of time for increased fire rate

	public double DamageIncreaseTime;				//Amount of time for DamageMultiplier effect
	public float DamageMultiplier;					//Increase damage done to enemies

    public AudioClip fireSound;
    public AudioClip iceSound;
    public AudioClip lightningSound;
    public AudioClip windSound;
    private AudioSource speaker;

    public float num = 0;							//To change weapons using spacebar and mouse cursor

    Vector3 aim;									//Controlls aiming postion of crosshair

    void Start() {
        speaker = gameObject.GetComponent<AudioSource>();
        TextSpawner.Initialize();
    }

	void Update(){									//Runs all functions 
        ProcessInputs ();
		Move ();
		Animate ();
		Aim ();
		PotionEffects ();
	}
		
	void PotionEffects(){							//Handles the amount of time for potion effects 
		if (SpeedBoostTime < Time.time)
			BASE_SPEED = 1;
		if (PotionCoolDownReducerTime < Time.time)
			PotionCoolDownReducer = 1;
	}

	void ProcessInputs(){
		Vector3 mouseMovement = new Vector3 (Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0.0f); //moves crosshair with mouse
		movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));		  //for moving player
		movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);								  //players speed
		movementDirection.Normalize ();	

		aim = aim + mouseMovement;
		if (aim.magnitude > 1.0f || aim.magnitude < 1.0f) {							//restricts mouse distance
			aim.Normalize ();
		}
	}

	void Move(){
		rb.velocity = movementDirection * movementSpeed * BASE_SPEED;				
	}

    void Animate(){
		animator.SetFloat ("Horizontal", Input.GetAxis("Horizontal"));
		animator.SetFloat ("Vertical", Input.GetAxis("Vertical"));
    }
 
    void Aim() {
        Vector2 shootingDirection = crosshair.transform.localPosition;		//Direction we'll be firing projectiles
        crosshair.transform.localPosition = aim * 1.0f;						//sets position of the crosshair object

		num += Input.GetAxisRaw ("Mouse ScrollWheel");						//Increase our spell attack number
		if (Input.GetButtonDown ("Jump"))
			num += 1;
        if (num > 3)
            num = 0;
        else if (num < 0)
            num = 3;

        if (Input.GetButton ("Fire1")) {									//checks for  mouse click
			if((num == 1 && Time.time > NextIce) || (num == 0 && Time.time > NextFire) || (num == 2 && Time.time > NextWind) || (num == 3 && Time.time > NextLightning)){  //makes sure that we're not in a cool down period for each specified attack
				GameObject projectile = Instantiate (weapons[(int)num], transform.position, Quaternion.identity);  //Instantiates the projectile
				projectile.GetComponent<Rigidbody2D> ().velocity = shootingDirection * 3.0f;					   //Moves the projectile at a set speed in a specified direction
				projectile.transform.Rotate (0, 0, Mathf.Atan2 (shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);		//Orients the projects rotation.
				

				if (projectile.name == "fireball(Clone)") {							//destroys projectile after certain time and sets timer for cooldown.
                    speaker.PlayOneShot(fireSound, 1.0f);
                    Destroy(projectile, 2.0f);
					NextFire = Time.time + (1.25 * PotionCoolDownReducer);
				} 
				else if (projectile.name == "iceball(Clone)" ) {
                    speaker.PlayOneShot(iceSound, 1.0f);
                    Destroy(projectile, 2.0f);
					NextIce = Time.time + (0.75 * PotionCoolDownReducer);
				} 
				else if (projectile.name == "wind(Clone)" ) {
                    speaker.PlayOneShot(windSound, 1.0f);
                    Destroy(projectile, 2.0f);
					NextWind = Time.time + (0.5 * PotionCoolDownReducer);
				}
                else if(projectile.name == "Lightning(Clone)")
                {
                    speaker.PlayOneShot(lightningSound, 1.0f);
                    Destroy(projectile, 1.5f);
					NextLightning = Time.time + (3 * PotionCoolDownReducer);
                }
			}
		}
	}



}
