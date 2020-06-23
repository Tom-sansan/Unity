using UnityEngine;
using System.Collections;

/// <summary>
/// Player controller to move by Key.
/// </summary>
public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
		// Left arrow is clicked
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			transform.Translate (-3, 0, 0);	// move 3 to left
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			transform.Translate (3, 0, 0);	// move 3 to right
		}
	}

	public void LButtonDown() {
		transform.Translate (-3, 0, 0);
	}

	public void RButtonDown() {
		transform.Translate (3, 0, 0);
	}
}
