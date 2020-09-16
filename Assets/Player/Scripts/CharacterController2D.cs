using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.InputSystem; //New Input System not used

public class CharacterController2D : MonoBehaviour
{
	public float WalkSpeed = 1f;
	public float JumpForce = 400f;
	[Range(0, .3f)] public float MovementSmoothing = .05f;
	public bool AirControl = false;
	public float GroundTestHight;
	public LayerMask WhatIsGround;

	private BoxCollider2D col2D;
	private Rigidbody2D rb2D;
	private Vector3 currentVelocity = Vector3.zero;

	private Vector2 inputVector;
	private bool isJumping;

	private void Awake()
	{
		rb2D = GetComponent<Rigidbody2D>();
		col2D = GetComponent<BoxCollider2D>();
	}

	//private void Update()
	//{
	//	Move(inputVector.x, isJumping);
	//}

	public void Move(float move, bool jump)
	{
		bool isGrounded = IsGrounded();

		if (isGrounded || AirControl)
		{
			Vector3 targetVelocity = new Vector2(move * WalkSpeed, rb2D.velocity.y);
			rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref currentVelocity, MovementSmoothing);
		}

		if (isGrounded && jump)
		{
			rb2D.AddForce(new Vector2(0f, JumpForce));
			isJumping = false;
		}

		//Animation
		PlayerAnimation.current.PlayerJump(!isGrounded);
		PlayerAnimation.current.PlayerWalk(move != 0 && isGrounded);
	}

	private bool IsGrounded()
	{
		RaycastHit2D hit = Physics2D.BoxCast(col2D.bounds.center, col2D.bounds.size, 0f, Vector2.down, GroundTestHight, WhatIsGround);
		return hit.collider != null;
	}

	////Only needed with new Input System 
	//private void OnMovement(InputValue inputValue)
  //{
  //  inputVector = inputValue.Get<Vector2>();
  //}
	//
  //private void OnJump()
  //{
  //  isJumping = true;
  //}
}
