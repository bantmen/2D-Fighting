using UnityEngine;
using System.Collections;
using System.Windows.Forms;

public class ControllerAI : MonoBehaviour {

	//my character - config - hard
	byte playMode = 1; // 0 -> Aggressive, 1 -> Defensive
	PlatformerCharacter2D2AI my_characterScript;
	PlayerMoves2AI my_characterMoves;

	//my character - config - soft
	float current_my_pos;
	bool withinKick;
	bool withinPunch;
	bool shouldRange;

	//my character - config - defensive
	float blockCooldown = 5f;
	float lastBlock = -1f;

	//player1
	PlayerMovesAI player1_script;
	float kickEpsilon;
	float punchEpsilon;
	float rangeEpsilon;
	GameObject player1_go;
	float current_enemy_pos;

	//fire on the right
	float fireRightEpsilon = 2.86f;
	GameObject fireRight_go;
	float current_fireRight_pos;

	void Start () {
		my_characterScript = GetComponent<PlatformerCharacter2D2AI> ();
		my_characterMoves = GetComponent<PlayerMoves2AI> ();

		player1_go = GameObject.Find ("2D Character-1");
		player1_script = player1_go.GetComponent <PlayerMovesAI> ();
		kickEpsilon = player1_script.epsilonKick1;
		punchEpsilon = player1_script.epsilonPunch2;
		rangeEpsilon = 10f;  //CAN CHANGE LATER

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
		if (current_my_pos - current_enemy_pos > rangeEpsilon) {
			shouldRange = true;
		}
		else{
			shouldRange = false;
		}
		switch (playMode){
			case 0:
				Aggressive (withinKick, withinPunch, shouldRange);
				break;
			case 1:
				Defensive (withinKick, withinPunch, shouldRange);
				break;
		}

		if (EpsilonCheck (current_my_pos, fireRightEpsilon, current_fireRight_pos)) {
			MoveList(0);
		}
		else {
			if (!withinKick && !withinPunch && !shouldRange) MoveList(0);
		}
	}
		
	void MoveList (int num) {
		switch (num){
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
				SendKeys.Send("Z"); //range attack
				break;
			case 5:
				break;              //stand idle
			case 6:
				my_characterScript.Flip();
				break;
		}
	}

	void Aggressive (bool kick, bool punch, bool range) {   //playMode -> 0
		MoveList (5);       //idle
		if (!range) {
			if (kick) MoveList(2);        //kick
			else if (punch) MoveList(3);  //punch
			else MoveList(5);             //idle
		}
		else {
			Debug.Log("here");
			MoveList(4);                  //range
		}
	}

	void Defensive (bool kick, bool punch, bool range){    //playMode -> 1
		bool isFacing = my_characterMoves.Facing ();       // checking if facing IN THE beginning
		if (range) {         //if there is a distance between two characters 
			if (isFacing && CooldownCheck(blockCooldown, lastBlock)) {
				MoveList(6); //flip
				lastBlock = Time.time;
			}
			else {
				Debug.Log("in else");
				MoveList(0);
			}
		}
		else {               //if the characters are close to each other
			if ((kick || punch) && !isFacing) MoveList(6); 
			if (kick) MoveList(2);        //kick
			else if (punch) MoveList(3);  //punch
			//MoveList(6);
		}
	}
	
	bool EpsilonCheck (float you, float epsilon, float target){  //DOES NOT CHECK FOR THE UPPERBOUND!
		return Mathf.Abs (you - target) <= epsilon;
	}

	bool CooldownCheck (float cooldown, float lastTime) {
		return lastTime == -1 || Time.time - cooldown > lastTime;  //-1 to check if initialized
	}
}
