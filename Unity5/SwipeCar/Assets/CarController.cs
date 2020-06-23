using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {

	float speed = 0;
	Vector2 startPos;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// Swipe length
		if (Input.GetMouseButtonDown (0)) {	// if mouse is clicked,
			//this.speed = 0.2f;				// sets the first speed.
			// Coordinate that Mouse is clicked
			this.startPos = Input.mousePosition;
		} else if (Input.GetMouseButtonUp (0)) {
			// Coordinate that Mouse is up
			Vector2 endPos = Input.mousePosition;
			float swipeLength = (endPos.x - this.startPos.x);
			// Convert swipte length to speed
			this.speed = swipeLength / 800.0f;
			// Sound effect
			GetComponent<AudioSource>().Play();
		}
		transform.Translate (this.speed, 0, 0);	// move
		this.speed *= 0.98f;					// set the speed slow gradualy
	}
}
