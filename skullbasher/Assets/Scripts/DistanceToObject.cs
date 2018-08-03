using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DistanceToObject : MonoBehaviour {



	[SerializeField]
	private Transform checkpoint;

	[SerializeField]
	private Text distanceText;

	private float distance;


	// Update is called once per frame
	private void Update () {
		distance = (checkpoint.transform.position.x - transform.position.x);


		distanceText.text = "Distance: " + distance.ToString("F1") + "meters";

		if(distance <=0)
			distanceText.text = "Nice!";
	}
}
