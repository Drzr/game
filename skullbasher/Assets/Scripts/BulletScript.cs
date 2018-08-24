using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
//

	float velY = 0f;
	Rigidbody2D rb;


	GameObject thePlayer;
	Player playerScript;
	
	float dirX;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();

		thePlayer = GameObject.Find("C$");
		playerScript = thePlayer.GetComponent<Player>();

		dirX = playerScript.dir;

	}
	
	// Update is called once per frame
	void Update () {
		
		Debug.Log (dirX);

		rb.velocity = new Vector2 (dirX,velY);


		Destroy (gameObject, 3f);
	}
}
