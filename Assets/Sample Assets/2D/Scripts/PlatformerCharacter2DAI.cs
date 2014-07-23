using UnityEngine;

public class PlatformerCharacter2DAI : MonoBehaviour 
{
	public bool facingRight = true;							// For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.
	[SerializeField] float jumpForce = 400f;			// Amount of force added when the player jumps.	

	[Range(0, 1)]
	[SerializeField] float crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	
	[SerializeField] bool airControl = false;			// Whether or not a player can steer while jumping;
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	
	Transform groundCheck;								// A position marking where to check if the player is grounded.
	float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.
	Transform ceilingCheck;								// A position marking where to check for ceilings
	float ceilingRadius = .01f;							// Radius of the overlap circle to determine if the player can stand up
	Animator anim;										// Reference to the player's animator component.

	PlayerMovesAI script;  //to look for isFocusing
	public AudioClip grunt;

	GameObject go;
	PlatformerCharacter2D2AI script2; //to look for move
	public bool moving = false;
	public AudioClip pushing;

    void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		anim = GetComponent<Animator>();

		script = GetComponent <PlayerMovesAI> ();

		go = GameObject.Find ("2D Character-2-AI");
		script2 = go.GetComponent <PlatformerCharacter2D2AI> ();
	}


	void FixedUpdate()
	{
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		anim.SetBool("Ground", grounded);

		// Set the vertical animation
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
	
	}


	public void Move(float move, bool crouch, bool jump)
	{

		if (!script.isFocusing) {
			
			// If crouching, check to see if the character can stand up
			if(!crouch && anim.GetBool("Crouch"))
			{
				// If the character has a ceiling preventing them from standing up, keep them crouching
				if( Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
					crouch = true;
			}
			
			// Set whether or not the character is crouching in the animator
			anim.SetBool("Crouch", crouch);
			
			//only control the player if grounded or airControl is turned on
			if(grounded || airControl)
			{
				// Reduce the speed if crouching by the crouchSpeed multiplier
				move = (crouch ? move * crouchSpeed : move);
				
				// The Speed animator parameter is set to the absolute value of the horizontal input.
				anim.SetFloat("Speed", Mathf.Abs(move));
				
				if (move != 0 && !audio.isPlaying && grounded) {
					audio.Play();
				}
				
				rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
				
				// If the input is moving the player right and the player is facing left...
				if(move > 0 && !facingRight) {
					// ... flip the player.
					Flip();
					audio.PlayOneShot(grunt);
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if(move < 0 && facingRight) {
					// ... flip the player.
					Flip();
					audio.PlayOneShot(grunt);
				}

				if (move == 0) {
					moving = false;
				}
				else {
					moving = true;
				}
			}
			
			// If the player should jump...
			if (grounded && jump) {
				// Add a vertical force to the player.
				anim.SetBool("Ground", false);
				//            rigidbody2D.AddForce(new Vector2(0f, jumpForce));             JUMP DISABLED
			}
		}
		}


	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	bool Facing () {
		if (facingRight && !script2.facingRight) return true;
		else return false;
	}
	
	void OnTriggerEnter2D (Collider2D other) {

		if ((moving || script2.moving) && Facing ()) {  //then someone is pushing
				if (!audio.isPlaying && Time.time % 1.5 == 0) {
					audio.PlayOneShot(pushing);
					//Debug.Log(123);
				}
		}
	}

}
