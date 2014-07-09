using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;
//using XInputDotNetPure;
using System.Runtime.InteropServices;

public class PlayerMoves : MonoBehaviour {
	
	public AudioClip kick_swing_1;    //played on kick initiate
	public AudioClip punch_swing_1;   //played on punch initiate
	AudioClip swingAudio;             //temp holder for swing audio
	public AudioClip kick_1;
	public AudioClip kick_2;
	public AudioClip punch_1;
	public AudioClip punch_2;
	public AudioClip thrown_1;         //when the throw is successful
	public AudioClip concentrate_1;    //played on throw initiate
	public AudioClip throw_1_landed;
	public AudioClip blocked;

	public byte swinged = 0; // 0, 1, 2, 3, 4 -> None, Punch-1, Punch-2, Kick-1, Kick-2
	float epsilonKick1 = 1.451275f;    //ADD PUSH TO EACH
	float epsilonKick2 = 1.361275f;
	float epsilonPunch1 = 1.210906f;
	float epsilonPunch2 = 1.010906f;
	float epsilon;   //swing range
	float countKick1 = 0.6f;
	float countKick2 = 0.55f;
	float countPunch1 = 0.40f;
	float countPunch2 = 0.32f;
	float count;    //swing cooldown
	float lastSwing;  //used for cooldown implementation
	float kickDamage1 = 6f;
	float kickDamage2 = 5f;
	float punchDamage1 = 4.5f;
	float punchDamage2 = 3.8f;
	public float damage;
	public byte swingLanded = 0; //0-> enemy not in range or swing null, 1-> enemy blocked, 2-> successful hit 

	float pushKick1 = 1.2f;
	float pushKick2 = 0.90f;
	float pushPunch1 = 0.75f;
	float pushPunch2 = 0.60f;
	float pushThrow1 = 1.5f;
	public float pushedBy;

	public byte thrown; //currently only 1
	float throwCount = 2f;   //cooldown for throws
	float lastThrown;   //to implement isFocusing
	float throwDamage1 = 5f;
	public bool isFocusing = false;    //true when throw initiate, when true cant move or do any other moves
	float currentHp;            //making sure that the throw was not canceled
	public bool throwDone = false;
	//IMPLEMENT PUSH/RECOIL etc.

	GameObject go;
	PlayerMoves2 script;
	public byte temp_swinged = 0;   //sending the information to the other player

	float distanceX;               //distance between two players

	PlatformerCharacter2D2 script2;   
	PlatformerCharacter2D script3;   //PLAYER 1

	public float hitPoint = 100;

	bool speakOnce = true;
		
	void Start () {
		go = GameObject.Find ("2D Character-2");
//		AudioClip[] audioArray = new AudioClip[6] {kick_1, kick_2, punch_1, punch_2, thrown_1, concentrate_1};

		script = go.GetComponent <PlayerMoves2> ();
		script2 = go.GetComponent <PlatformerCharacter2D2> ();
		script3 = GetComponent <PlatformerCharacter2D> ();
	}

	void Update () {
		AudioClip[] audioArray = {kick_1, kick_2, punch_1, punch_2, thrown_1, concentrate_1};   //move it to start
		if (script.temp_swinged != 0 && swingLanded == 2) {   //regarding getting hit
			audio.PlayOneShot(audioArray[script.temp_swinged-1]);  //since 0 -> None
			swingLanded = 0;	
			script.temp_swinged = 0;
			hitPoint -= script.damage;
			
			Vector3 temp = transform.position;
			temp.x -= script.pushedBy;
			transform.position = temp;
		}
		else if (script.throwDone) {
			audio.PlayOneShot(throw_1_landed);
			hitPoint -= script.damage;
			script.throwDone = false;
			UnityEngine.Debug.Log (hitPoint);
			
			Vector3 temp = transform.position;
			temp.x -= script.pushedBy;
			transform.position = temp;
		}
		if (swingLanded == 1) {
			audio.PlayOneShot(blocked);
			swingLanded = 0;
		}

		if (!isFocusing) {
			if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) {
				if (Input.GetKeyDown (KeyCode.K)) { //Punch-1
					swinged = 1;
				}
				else if (Input.GetKeyDown (KeyCode.L)) { //Punch-2
					swinged = 2;
				}
				else if (Input.GetKeyDown (KeyCode.M)) { //Kick-1
					swinged = 3;
				}
				else if (Input.GetKeyDown (KeyCode.Comma)) { //Kick-2
					swinged = 4;
				}
				else {
					swinged = 0;                        //None
				}
				if (Input.GetKeyDown (KeyCode.Space)) {
					thrown = 1;
				}
			}
			else {
				swinged = 0;
			}
			
			
			if (swinged != 0) {  	
				if (swinged == 1) {  
					swingAudio = kick_swing_1;
					epsilon = epsilonKick1;
					count = countKick1;
					damage = kickDamage1;
					pushedBy = pushKick1;
				} 
				else if (swinged == 2) {
					swingAudio = kick_swing_1;
					epsilon = epsilonKick2;
					count = countKick2;
					damage = kickDamage2;
					pushedBy = pushKick2;
				}
				else if (swinged == 3) {
					swingAudio = punch_swing_1;
					epsilon = epsilonPunch1;
					count = countPunch1;
					damage = punchDamage1;
					pushedBy = pushPunch1;
				}
				else if (swinged == 4) {
					swingAudio = punch_swing_1;
					epsilon = epsilonPunch2;
					count = countPunch2;
					damage = punchDamage2;
					pushedBy = pushPunch2;
				}
				if (lastSwing == 0 || (Time.time - lastSwing > count)) {//can only swing if swing count is reached
					audio.PlayOneShot (swingAudio);
					lastSwing = Time.time;
					distanceX = go.transform.position.x - transform.position.x;  //distance between two players on x-axis
					if ((Mathf.Abs(distanceX) < epsilon)) { 
						//can only hit if within the move range
						if (Facing ()) {
							//can only hit if both are looking at each other
							script.swingLanded = 2;
							temp_swinged = swinged;
						}
						else {
							script.swingLanded = 1;
						}
					}
				}
			}
			else if (thrown == 1) {          //PROJECTILE
				audio.PlayOneShot(concentrate_1); 
				lastThrown = Time.time;
				isFocusing = true;
				currentHp = hitPoint;   //to make sure that throw was not canceled by the enemy
			}   
			
			//PUSH/RECOIL?
			
			if (Input.GetKeyDown (KeyCode.RightShift)) {
				SpeakForMe (Mathf.Round(hitPoint).ToString());                 //tells you your hitpoints
			}
		}
		else {    //if not damaged for throwCount then throw successful, else failed
			if (hitPoint != currentHp) {    //damaged by the opponent -> move failed or looking at wrong dir
				isFocusing = false;
				thrown = 0;
			}
			if (Time.time - lastThrown > throwCount && hitPoint == currentHp) {
				if (Facing ()) {
					audio.PlayOneShot(thrown_1);
					isFocusing = false;
					throwDone = true;
					damage = throwDamage1;
					pushedBy = pushThrow1;
					thrown = 0;
				}
				else {
					isFocusing = false;
					throwDone = false;
					damage = 0;
					pushedBy = 0;
					thrown = 0;
					audio.PlayOneShot(blocked);
				}
			}
		}
	
	}

	void FixedUpdate ()  {
		if (transform.position.y <= -5) {             //Falling down
			hitPoint = 0;
		}
		if (hitPoint <= 0 && script.hitPoint > 0) {                          //GAME OVER
			if (speakOnce) {
				SpeakForMe("Player 2 is victorious. Game Over. To play again press space bar.");
				speakOnce = false;
			}
			if (Input.GetKeyDown(KeyCode.Space)) {
				Application.LoadLevel(Application.loadedLevel);
			}
		} 
		else if (script.hitPoint <=0 && hitPoint > 0) {
			if (speakOnce) {
				SpeakForMe("Player 1 is victorious. Game Over. To play again press space bar.");
				speakOnce = false;
			}
			if (Input.GetKeyDown(KeyCode.Space)) {
				Application.LoadLevel(Application.loadedLevel);
			}
		}
		else if (hitPoint <= 0 && script.hitPoint <= 0) {
			if (speakOnce) {
				SpeakForMe("The game is a tie. To play again press space bar.");
				speakOnce = false;
			}
			if (Input.GetKeyDown(KeyCode.Space)) {
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}


	void SpeakForMe (string message) {
		Process say = new Process ();
		say.StartInfo.FileName   = "C:\\Users\\Berkay Antmen\\Documents\\2D Fighting\\Assets\\Tools\\SpeechDemo.exe";  
		say.StartInfo.Arguments = message;
		say.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
		say.Start();
	}

	bool Facing () {
		if (script3.facingRight && !script2.facingRight) return true;
		else return false;
	}

}
