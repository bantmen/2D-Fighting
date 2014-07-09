using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class Welcome : MonoBehaviour {

	bool flag = true;
	public bool listenIntro = true;

	void Start () {
		if (listenIntro) {
			Time.timeScale = 0;
			SpeakForMe ("Welcome to this experimental multiplayer 2D fighting game. I will go over the general gameplay and the rules. Press J or K to kick. " +
			            "Press N or M to punch. Press the arrow keys to walk along the x-axis. Press Enter to learn about your remaining hitpoints." +
			            "Each hit damages and pushes the enemy, Kicks hit harder and push further but are slower than the punches." +
			            "To hit the enemy, both players should face each other." +
			            "Press Right Shift for your ranged attack. There is fire on the either sides of the stage and becareful to not fall below! " +
			            "To win the game, either reduce the enemy's hitpoints to zero or push them off the stage." +
			            "Press N to start.");
		}
	}

	void Update () {
		if (flag && Input.GetKeyDown(KeyCode.N)) {
			UnityEngine.Debug.Log("unpaused");
			Time.timeScale = 1;
			flag = false;
			audio.Play();
		}
	}

	void SpeakForMe (string message) {
		Process say = new Process ();
		say.StartInfo.FileName   = "C:\\Users\\Berkay Antmen\\Documents\\2D Fighting\\Assets\\Tools\\SpeechDemo.exe";  
		say.StartInfo.Arguments = message;
		say.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
		say.Start();
	}
}
