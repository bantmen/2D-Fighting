using UnityEngine;
using System.Collections;
using System.Windows.Forms;

public class ControllerAI : MonoBehaviour {

	//my character
	float current_my_pos;
	bool withinKick;
	bool withinPunch;

	//player1
	PlayerMoves player1_script;
	float kickEpsilon;
	float punchEpsilon;
	GameObject player1_go;
	float current_enemy_pos;

	//fire on the right
	float fireRightEpsilon = 2.86f;
	GameObject fireRight_go;
	float current_fireRight_pos;

	void Start () {
		player1_go = GameObject.Find ("2D Character-1");
		player1_script = player1_go.GetComponent <PlayerMoves> ();
		kickEpsilon = player1_script.epsilonKick1;
		punchEpsilon = player1_script.epsilonPunch2;

		fireRight_go = GameObject.Find ("FireRight");
	}

	void FixedUpdate () {
		current_my_pos = transform.position.x;
		current_enemy_pos = player1_go.transform.position.x;
		current_fireRight_pos = fireRight_go.transform.position.x;
		if (EpsilonCheck(current_my_pos, kickEpsilon, current_enemy_pos)){
			withinKick = true;
		}
		else if (EpsilonCheck(current_my_pos, punchEpsilon, current_enemy_pos)){
			withinKick = false;
			withinPunch = true;
		}
		else{
			withinKick = false;
			withinPunch = false;
		}
		if (EpsilonCheck (current_my_pos, fireRightEpsilon, current_fireRight_pos)) {
			Debug.Log("near fire");
			//SendKeys.Send("A");
		}
		else {
			Debug.Log("away from fire");
			//SendKeys.Send("D");
		}
	}

	void Defensive(){	
	}

	void Aggressive(){
	}

	bool EpsilonCheck (float you, float epsilon, float target){
		return Mathf.Abs (you - target) <= epsilon;
	}
}
