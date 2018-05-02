using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	Rigidbody2D rb;
	Animator anim;
	float dirX, moveSpeed = 5f;
	bool isDamage, isDead;
	bool facingRight = true;
	Vector3 localScale;

	public float speed;
	public Text countText;
	public Text winText;
	public Text healthText;
	private int healthPoints;
	private int count;


	public AudioClip crashSoft;
	public AudioClip crashHard;


	private AudioSource source;

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
	
	// Update is called once per frame
	void Update () {		

		if (Input.GetButtonDown ("Jump") && !isDead && rb.velocity.y == 0)
			rb.AddForce (Vector2.up * 600f);

		if (Input.GetKey (KeyCode.LeftShift))
			moveSpeed = 10f;
		else
			moveSpeed = 5f;

		SetAnimationState ();

		if (!isDead)
			dirX = Input.GetAxisRaw ("Horizontal") * moveSpeed;
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
		if (dirX == 0) {
			anim.SetBool ("isWalking", false);
		
		}

		if (rb.velocity.y == 0) {
			anim.SetBool ("isJumping", false);
			anim.SetBool ("isFalling", false);
		}

		if (Mathf.Abs(dirX) == 5 && rb.velocity.y == 0)
			anim.SetBool ("isWalking", true);

		if (rb.velocity.y > 0)
			anim.SetBool ("isJumping", true);
		
		if (rb.velocity.y < 0) {
			anim.SetBool ("isJumping", false);
			anim.SetBool ("isFalling", true);

		}
		if (isDamage == false) {
			anim.SetBool ("isDamage", false);
		}
		//if (Input.GetAxisRaw ("Vertical")<0){
	//	}

	}

	void CheckWhereToFace()
	{
		if (dirX > 0)
			facingRight = true;
		else if (dirX < 0)
			facingRight = false;

		if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
			localScale.x *= -1;

		transform.localScale = localScale;

	}

	void OnTriggerEnter2D (Collider2D col)
	{
		

		if (col.gameObject.CompareTag("Points")) {

			col.gameObject.SetActive(false);
			count = count + 1;
			SetCountText ();

		}

		if (col.gameObject.CompareTag ("Enemy")) {
			healthPoints -= 1;
			SetHealthText ();
			anim.SetTrigger ("isDamage");
			StartCoroutine ("Hurt");



		}

		/*else {
			dirX = 0;
			isDead = true;
			anim.SetTrigger ("isDead");
		}*/
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

}
