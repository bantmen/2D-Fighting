using UnityEngine;

[RequireComponent(typeof(PlatformerCharacter2D2AI))]
public class Platformer2DUserControl2AI : MonoBehaviour 
{
	private PlatformerCharacter2D2AI character;
    private bool jump;


	void Awake()
	{
		character = GetComponent<PlatformerCharacter2D2AI>();
	}

    void Update ()
    {
        // Read the jump input in Update so button presses aren't missed.
#if CROSS_PLATFORM_INPUT
        if (CrossPlatformInput.GetButtonDown("Jump")) jump = true;
#else
		if (Input.GetButtonDown("Jump")) jump = true;
#endif

    }

	void FixedUpdate()
	{
//		float h = 0;
		// Read the inputs.
		bool crouch = Input.GetKey(KeyCode.LeftControl);
//		#if CROSS_PLATFORM_INPUT
		float h = CrossPlatformInput.GetAxis("Horizontal2");
//		#else
//		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
			h = Input.GetAxis("Horizontal2");
//				}
//		#endif

		// Pass all parameters to the character control script.
		character.Move( h, crouch , jump );

        // Reset the jump input once it has been used.
	    jump = false;
	}
}
