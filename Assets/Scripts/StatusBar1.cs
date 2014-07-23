using UnityEngine;
using System.Collections;

public class StatusBar1 : MonoBehaviour {

	GameObject go;
	PlayerMoves2 script;

	void Start () {
		go = GameObject.Find ("2D Character-2");  //FIX IT BACK FOR THE MULTIPLAYER
		script = go.GetComponent <PlayerMoves2> ();

	}

	void FixedUpdate () {
		//guiText.text = "Health: " + script.hitPoint;
	}
}
