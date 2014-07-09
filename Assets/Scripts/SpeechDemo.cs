using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class SpeechDemo : MonoBehaviour {
	
	void Start () {
		Process say = new Process ();

		say.StartInfo.FileName   = "C:\\Users\\Berkay Antmen\\Documents\\2D Fighting\\Assets\\Tools\\SpeechDemo.exe";  //ENTER YOUR OWN PATH
		say.StartInfo.Arguments = "Hello, World!";
		say.StartInfo.UseShellExecute = true;
		say.StartInfo.CreateNoWindow = true;
		say.Start();
	
	}

	void Update () {
	
	}
}
