using UnityEngine;
using System.Collections;
using System.Windows.Forms;

public class ControllerAI : MonoBehaviour {
	
	void Start () {
	}

	void FixedUpdate () {
		SendKeys.Send("G");
	}
}
