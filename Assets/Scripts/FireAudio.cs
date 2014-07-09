using UnityEngine;
using System.Collections;

public class FireAudio : MonoBehaviour {

	GameObject go1;
	GameObject go2;
	PlayerMoves script1;
	PlayerMoves2 script2;
	float cooldown = 1.5f;
	float lastTime = 0;

	public float epsilon;  //currently 2f
	float pos1;
	float pos2;

	void Start () {
		go1 = GameObject.Find ("2D Character-1");
		script1 = go1.GetComponent<PlayerMoves> ();
		go2 = GameObject.Find ("2D Character-2");
		script2 = go2.GetComponent<PlayerMoves2> ();
	}

	void FixedUpdate () {   //fire sound is heard if either of the players is near it
		pos1 = go1.transform.position.x;
		pos2 = go2.transform.position.x;
		if (!audio.isPlaying && (Mathf.Abs(transform.position.x - pos1) < epsilon || Mathf.Abs(transform.position.x - pos2) < epsilon)) {
			//audio.Play ();
			if (Mathf.Abs(transform.position.x - pos1) < epsilon/2) {
				if (lastTime == 0 || Time.time - lastTime > cooldown ) {
					script1.hitPoint -= 10;
					lastTime = Time.time;
				}
			}
			if (Mathf.Abs(transform.position.x - pos2) < epsilon/2) {
				if (lastTime == 0 || Time.time - lastTime > cooldown ) {
					script2.hitPoint -= 10;
					lastTime = Time.time;
				}
			}
		} 
	}
}
