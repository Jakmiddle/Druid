using UnityEngine;
using System.Collections;

public class CharacterCont : MonoBehaviour {

	public class States {

		public bool Walking;
		public bool MidJump;

		public States (bool wal, bool mid)
		{
			Walking = wal;
			MidJump = mid;
		}
	}

	public States State = new States (false, false);

	public int MoveDir;
	public bool facingRight;
	public float maxSpeed;
	public float curSpeed;

	public Vector2 playerVelocity;
	public Vector2 Origin;
	LayerMask groundLayer = 1<<9;
	public bool grounded;
	public float jumpHeight;

	public bool Walled;



	// Use this for initialization
	void Start () 
	{
		State.Walking = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Origin = new Vector2 (transform.position.x, transform.position.y - 1.0f);

		GroundCheck ();
		MAWallCheck();


		if (grounded == true) 
		{
			State.Walking = true;
			State.MidJump = false;

			playerVelocity.y = 0;
			rigidbody2D.velocity = playerVelocity;

			Jump();
		}
		else 
		{
			State.Walking = false;
			State.MidJump = true;
		}

		if (State.Walking == true) {

			Walk ();

		} else if (State.MidJump == true) {

			State.Walking = false;
			WallJump();
			AffectJump();
		}

		if (playerVelocity.x > -0.02 && playerVelocity.x < 0.02) 
		{
			playerVelocity.x = 0;
			playerVelocity.y = rigidbody2D.velocity.y;
			rigidbody2D.velocity = playerVelocity;
		}
	}

	void Walk ()
	{
		float moving = Input.GetAxis ("Horizontal");

		if (moving > 0) 
		{
			MoveDir = 1;
			SUSD();
			playerVelocity.x = curSpeed; 

		}
		else if (moving < 0) 
		{
			MoveDir = -1;
			SUSD();
			playerVelocity.x = curSpeed;
		} 
		else 
		{
			MoveDir = 0;
			SUSD();
			playerVelocity.x = curSpeed;
		}

		rigidbody2D.velocity = playerVelocity;


	}

	void SUSD ()
	{
		if (MoveDir == 1) {

			if (curSpeed < maxSpeed)
			{
				if (facingRight == false)
				{
					curSpeed = curSpeed + (Time.deltaTime * 100);
					facingRight = true;
				}
				else
				{
					curSpeed = curSpeed + (Time.deltaTime * 10);
				}
			}
			else if (curSpeed > maxSpeed) 
			{
				curSpeed = maxSpeed;
			}
		}
		else if (MoveDir == -1) 
		{
			if (curSpeed > -maxSpeed)
			{
				if (facingRight == true)
				{
					curSpeed = curSpeed - (Time.deltaTime * 100);
					facingRight = false;
				}
				else
				{
					curSpeed = curSpeed - (Time.deltaTime * 10);
				}
			}
			else if (curSpeed < -maxSpeed) 
			{
				curSpeed = -maxSpeed;
			}
		}
		else if (MoveDir == 0) 
		{
			if (curSpeed < 0)
			{
				curSpeed += (Time.deltaTime * 10);
			}
			else if (curSpeed > 0)
			{
				curSpeed -= (Time.deltaTime * 10);
			}
		}
	}

	void Jump ()
	{
		if (Input.GetButtonDown("Fire1")) {
			if (grounded == true){
				playerVelocity.y = jumpHeight/Time.deltaTime;
				
				if (playerVelocity.y > 9f)
				{
					playerVelocity.y = 9f;
				}
				
				rigidbody2D.AddForce(new Vector2(0, playerVelocity.y), ForceMode2D.Impulse);
				playerVelocity.y = 0f;

				State.Walking = false;
				State.MidJump = true;
				
			}
		}
	}

	void AffectJump ()
	{
		curSpeed = 0; //making sure there is no accidental movement on landing

		float horizontal = Input.GetAxis ("Horizontal");

		if (facingRight == true && horizontal < 0) 
		{
			playerVelocity.x += (horizontal * 4f) * Time.deltaTime;
		} 
		else if (facingRight == true && horizontal > 0) 
		{
			playerVelocity.x += (horizontal * 4) * Time.deltaTime;
		}
		else if (facingRight == false && horizontal > 0) 
		{
			playerVelocity.x += (horizontal * 4) * Time.deltaTime;
		} 
		else if (facingRight == false && horizontal < 0) 
		{
			playerVelocity.x += (horizontal * 4) * Time.deltaTime;
		}

		playerVelocity.y = rigidbody2D.velocity.y;
		rigidbody2D.velocity = playerVelocity;
	}

	void GroundCheck ()
	{
		Debug.DrawRay (new Vector2(Origin.x +0.25f, Origin.y), -Vector2.up, Color.red);
		Debug.DrawRay (new Vector2(Origin.x -0.25f, Origin.y), -Vector2.up, Color.red);

		if (Physics2D.Raycast (new Vector2(Origin.x +0.25f, Origin.y), -Vector2.up, 0.1f, groundLayer) 
		    || Physics2D.Raycast (new Vector2(Origin.x -0.25f, Origin.y), -Vector2.up, 0.1f, groundLayer))
			grounded = true;
		else
			grounded = false;
	}

	void WallJump ()
	{
		if (Input.GetButtonDown ("Fire1")) {
			if (Walled == true) {
				playerVelocity.y = jumpHeight / Time.deltaTime;
				
				if (playerVelocity.y > 9f) {
					playerVelocity.y = 9f;
				}

				int Facing;
				if (facingRight == true)
					Facing = -1;
				else
					Facing = 1;

				playerVelocity.x = Facing * maxSpeed;
				rigidbody2D.AddForce (playerVelocity, ForceMode2D.Impulse);

				facingRight = !facingRight;

				playerVelocity.y = 0f;
			}
		} 

		else if (Walled == true) 
		{
			curSpeed = 0; //preventing some strange movement

			playerVelocity.x = 0;
			playerVelocity.y = rigidbody2D.velocity.y;
			rigidbody2D.velocity = playerVelocity;
		}
	}
	void MAWallCheck ()
	{
		Vector2 wallOrigin;

		if (facingRight == true) 
		{
			wallOrigin = new Vector2 (transform.position.x + 0.5f, transform.position.y);

			if (Physics2D.Raycast (new Vector2 (wallOrigin.x, wallOrigin.y + 0.25f), Vector2.right, 0.1f, groundLayer) 
			    || Physics2D.Raycast (new Vector2 (wallOrigin.x, wallOrigin.y - 0.3f), Vector2.right, 0.1f, groundLayer))
				Walled = true;
			else
				Walled = false;
		} 
		else 
		{
			wallOrigin = new Vector2 (transform.position.x - 0.5f, transform.position.y);

			if (Physics2D.Raycast (new Vector2 (wallOrigin.x, wallOrigin.y + 0.25f), -Vector2.right, 0.1f, groundLayer) 
			    || Physics2D.Raycast (new Vector2 (wallOrigin.x, wallOrigin.y - 0.25f), -Vector2.right, 0.1f, groundLayer))
				Walled = true;
			else
				Walled = false;
		}
	}
}
