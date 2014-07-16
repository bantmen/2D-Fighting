using UnityEngine;
using System.Collections;

public class MoveAudioListener : MonoBehaviour {

	GameObject go;
	GameObject go2;

	void Start () {
		go = GameObject.Find ("2D Character-1");  //the blind player
		go2 = GameObject.Find ("2D Character-2-AI");  //FIX IT BACK FOR THE MULTIPLAYER!!!
	}

	void Update () {                                           //NEED TO FIX THIS AUDIO OUTPUT ISSUE
		Vector3 temp = transform.position;
				//CHOOSE THIS FOR MULTIPLAYER
		//temp.x = (go.transform.position.x + go2.transform.position.x)/2;
		//temp.y = (go.transform.position.y + go2.transform.position.y)/2;
				//CHOOSE THIS FOR SINGLEPLAYER
		temp.x = go.transform.position.x;
		temp.y = go.transform.position.y;
		transform.position = temp;

	}
}
