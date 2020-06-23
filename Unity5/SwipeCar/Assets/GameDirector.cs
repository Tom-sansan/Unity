using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Show the distance between car and flag
/// </summary>
public class GameDirector : MonoBehaviour {

	GameObject car;
	GameObject flag;
	GameObject distance;

	// Use this for initialization
	void Start () {
		this.car = GameObject.Find("car");
		this.flag = GameObject.Find("flag");
		this.distance = GameObject.Find("Distance");
	}
	
	// Update is called once per frame
	void Update () {
		float length = this.flag.transform.position.x - this.car.transform.position.x;
		if (length >= 0) {
			this.distance.GetComponent<Text> ().text = "By Goal: " + length.ToString ("F2") + "m";
		} else {
			this.distance.GetComponent<Text> ().text = "Game Over";
		}
	}
}
