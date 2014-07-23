using UnityEngine;
using System.Collections;

public class StatusBar1AI : MonoBehaviour {

	GameObject go;
	PlayerMoves2AI script;

	void Start () {
		go = GameObject.Find ("2D Character-2-AI");  //FIX IT BACK FOR THE MULTIPLAYER
		script = go.GetComponent <PlayerMoves2AI> ();

	}

	void FixedUpdate () {
		//guiText.text = "Health: " + script.hitPoint;
	}
}
