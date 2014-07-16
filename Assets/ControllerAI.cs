using UnityEngine;
using System.Collections;
using System.Windows.Forms;

public class ControllerAI : MonoBehaviour {

	//my character
	float current_my_pos;
	bool withinKick;
	bool withinPunch;
	float 

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
		Defensive (withinKick, withinPunch);
		if (EpsilonCheck (current_my_pos, fireRightEpsilon, current_fireRight_pos)) {
			Debug.Log("near fire");
			MoveList(0);
		}
		else {
			Debug.Log("away from fire");
			if (!withinKick && ! withinPunch) MoveList(0);
		}
	}
		
	void MoveList (int num) {
		switch (num)
		{
		case 0:
			SendKeys.Send("A");  //move left
			break;
		case 1:
			SendKeys.Send("D");  //move right
			break;
		case 2:
			SendKeys.Send("W");  //kick
			break;
		case 3:
			SendKeys.Send("H");  //punch
			break;
		case 4:
			SendKeys.Send("+"); //range attack
			break;
		case 5:
			break;              //stand idle
		}
	}

	void Defensive (bool kick, bool punch) {
		MoveList (5);
		if (kick) MoveList(2); //kick
		else if (punch) MoveList(3); //punch
		else MoveList(5); //idle
		if (kick || punch) {
			//MoveList(1);  //block
			//MoveList(0);  //go back
		}
	}

	bool EpsilonCheck (float you, float epsilon, float target){
		return Mathf.Abs (you - target) <= epsilon;
	}
}
