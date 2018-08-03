using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour 
{

	//player controller
	Rigidbody2D rb;
	Animator anim;
	float dirX, moveSpeed = 5f;
	public float dir;
	bool isDamage, isDead;
	bool facingRight = true;
	Vector3 localScale;
	//UI
	public Text countText;
	public Text winText;
	public Text healthText;
	private int healthPoints;
	private int count;

	//audio
	public AudioClip Burp;
	public AudioClip Can;

	private AudioSource source;


	//pause
	bool isPaused = false;
	//bullet
	public GameObject bullet;
	Vector2 bulletPos;
	public float fireRate = 0.0f;
	float nextFire = 0.0f;

	void Awake () 
	{
		source = GetComponent<AudioSource>();
	}

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();


		localScale = transform.localScale;
		count = 0;
		healthPoints = 10;
		SetCountText ();
		SetHealthText ();
		winText.text = "";

		isDead = false;
	}
	public void pauseGame()
	{
		if (isPaused) 
		{
			Time.timeScale = 1;
			isPaused = false;

		} else 
		{
			Time.timeScale = 0;
			isPaused = true;
		}
	}
	// Update is called once per frame
	void Update () {


		if (Input.GetKey (KeyCode.P)) 
		{
			pauseGame ();
		}	

		if (Input.GetButtonDown ("Jump") && !isDead && rb.velocity.y == 0)
			rb.AddForce (Vector2.up * 600f);

		if (Input.GetKey (KeyCode.LeftShift))
			moveSpeed = 10f;
		else
			moveSpeed = 5f;

		SetAnimationState ();

		if (!isDead)
			dirX = Input.GetAxisRaw ("Horizontal") * moveSpeed;

		if (Input.GetButtonDown ("Fire1") && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;
			fire();
		}


	}

	void FixedUpdate()
	{
		if (!isDamage)
			rb.velocity = new Vector2 (dirX, rb.velocity.y);
	}

	void LateUpdate()
	{
		
		CheckWhereToFace();
	}

	void SetAnimationState()
	{
		if (dirX == 0) 
		{
			anim.SetBool ("isWalking", false);
		
		}

		if (rb.velocity.y == 0) 
		{
			anim.SetBool ("isJumping", false);
			anim.SetBool ("isFalling", false);
		}

		if (Mathf.Abs(dirX) == 5 && rb.velocity.y == 0)
			anim.SetBool ("isWalking", true);

		if (rb.velocity.y > 0)
			anim.SetBool ("isJumping", true);
		
		if (rb.velocity.y < 0) 
		{
			anim.SetBool ("isJumping", false);
			anim.SetBool ("isFalling", true);

		}
		if (isDamage == false) 
		{
			anim.SetBool ("isDamage", false);
		}
		//if (Input.GetAxisRaw ("Vertical")<0){
	//	}

	}

	void CheckWhereToFace()
	{
		if (dirX > 0) 
		{
			dir = 10;
			facingRight = true;
		} else if (dirX < 0) 
		{
			dir = -10;
			facingRight = false;
		}

		if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
			localScale.x *= -1;

		transform.localScale = localScale;

	}

	void OnTriggerEnter2D (Collider2D col)
	{
		

		if (col.gameObject.CompareTag("Points")) 
		{
			col.gameObject.SetActive(false);
			count = count + 1;
			SetCountText ();
			source.PlayOneShot (Can);
		}
		if (col.gameObject.CompareTag ("Enemy")&& healthPoints>0)
		{
			
			source.PlayOneShot (Burp);
			healthPoints -= 1;
			if(healthPoints>=0)
			SetHealthText ();
			anim.SetTrigger ("isDamage");
			StartCoroutine ("Hurt");
		}
		else  if(col.gameObject.CompareTag ("Enemy")&&healthPoints<0)
		{
			healthPoints -= 1;
			if(healthPoints>=0)
				SetHealthText ();
			dirX = 0;
			isDead = true;
			anim.SetTrigger ("isDead");
		}
	}

	IEnumerator Hurt()
	{
		
		isDamage = true;
		rb.velocity = Vector2.zero;

		if (facingRight)
			rb.AddForce (new Vector2(-200f, 200f));
		else
			rb.AddForce (new Vector2(200f, 200f));
		
		yield return new WaitForSeconds (2.5f);

		isDamage = false;
	}
	void SetCountText ()
	{
		countText.text = "Points: " + count.ToString ();// Points determine strength oi (of intoxication)
		if (count >= 12)
		{
			winText.text = "You Win!";
		}
	}
	void SetHealthText ()
	{

		healthText.text = "Health: " + healthPoints.ToString ();
	}
	void fire ()
	{
		bulletPos = transform.position + new Vector3 (0,.78f,0);



		if (dir== 10) 
		{

			bulletPos += new Vector2 (+1f,-0.43f);
			Instantiate (bullet, bulletPos, Quaternion.identity);
		} else if (dir== -10) 
		{
			bulletPos += new Vector2 (-1f,-0.43f);
			Instantiate (bullet, bulletPos, Quaternion.identity);
		}






	}
}
