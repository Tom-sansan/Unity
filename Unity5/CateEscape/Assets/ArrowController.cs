using UnityEngine;
using System.Collections;

/// <summary>
/// Arrow controller.
/// </summary>
public class ArrowController : MonoBehaviour {

	GameObject player;

	// Use this for initialization
	void Start () {
		this.player = GameObject.Find ("player");
	}
	
	// Update is called once per frame
	void Update () {
		// Drop at the same speed by frame
		transform.Translate(0, -0.1f, 0);

		// Dismiss the object at the time of going out of screen
		if (transform.position.y < -5.0f) {
			Destroy (gameObject);
		}
		// Collision detection
		Vector2 p1 = transform.position;				// Central coordinate of arrow
		Vector2 p2 = this.player.transform.position;	// Central coordinate of player
		Vector2 dir = p1 - p2;
		float d = dir.magnitude;
		float r1 = 0.5f;	// Radius of arrow
		float r2 = 1.0f;	// Radius of player

		if (d < r1 + r2) {
			// Delete in case of collision
			Destroy(gameObject);
			// Report collition with player to Director script
			GameObject director = GameObject.Find("GameDirector");
			director.GetComponent<GameDirector> ().DecreaseHp ();
		}
	}
}
