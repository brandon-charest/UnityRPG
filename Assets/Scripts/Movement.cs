using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	//movement variables
	private float jumpSpeed = 18.0f;
	private float gravity = 32.0f;
	private float runSpeed = 15.0f;
	private float walkSpeed = 45.0f;
	private float rotateSpeed = 150.0f;

	private bool grounded = false;
	private Vector3 moveDirection = Vector3.zero;
	private bool isWalking = false;
	private string moveStatus = "idle";

	private static bool dead = false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(dead == false)
		{

			//allows for movement and jumps while on ground
			if(grounded)
			{
				moveDirection = new Vector3((Input.GetMouseButton(1) ? Input.GetAxis("Horizontal") : 0), 0, Input.GetAxis("Vertical"));


			//straffing 			
			if (Input.GetMouseButton(1) &&  Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0)
            {
                moveDirection *= 0.7f;
            }


				moveDirection = transform.TransformDirection(moveDirection);
				moveDirection *= isWalking ? walkSpeed : runSpeed;

				moveStatus = "idle";
				if(moveDirection != Vector3.zero)
				{
					moveStatus = isWalking ? "walking" : "running";
				}

				//Jumping
				if(Input.GetKeyDown(KeyCode.Space))
				{
					moveDirection.y = jumpSpeed;
				}

			}

			// Allow turning at anytime. Keep the character facing in the same direction as the Camera if the right mouse button is down.
			if(Input.GetMouseButton(1))
			{
				transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
			} else {
				transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime, 0);
			}

			//Gravity
			moveDirection.y -= gravity * Time.deltaTime;

			//Move Controller
			CharacterController controller = GetComponent<CharacterController>();
			CollisionFlags flags = controller.Move(moveDirection * Time.deltaTime);
			grounded = (flags & CollisionFlags.Below) !=0;


		}

		if(Input.GetMouseButton(1) || Input.GetMouseButton(0))
		{
			Cursor.visible = false;
		} else {
			Cursor.visible = true;
		}

	}
}
