using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float walkSpeed = 1f;
	[SerializeField] private float jumpForce = 400f;
	[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
	[SerializeField] private bool airControl = false;
	[SerializeField] private float groundTestHight;
	[SerializeField] private LayerMask whatIsGround;

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

	private void Update()
	{
		Move(inputVector.x, isJumping);
	}

	public void Move(float move, bool jump)
	{
		bool isGrounded = IsGrounded();

		if (isGrounded || airControl)
		{
			Vector3 targetVelocity = new Vector2(move * walkSpeed, rb2D.velocity.y);
			rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref currentVelocity, movementSmoothing);
		}

		if (isGrounded && jump)
		{
			rb2D.AddForce(new Vector2(0f, jumpForce));
			isJumping = false;
		}

		//Animation
		PlayerAnimation.current.PlayerJump(!isGrounded);
		PlayerAnimation.current.PlayerWalk(move != 0 && isGrounded);
	}

	private bool IsGrounded()
	{
		RaycastHit2D hit = Physics2D.BoxCast(col2D.bounds.center, col2D.bounds.size, 0f, Vector2.down, groundTestHight, whatIsGround);
		return hit.collider != null;
	}

	private void OnMovement(InputValue inputValue)
  {
    inputVector = inputValue.Get<Vector2>();
  }

  private void OnJump()
  {
    isJumping = true;
  }
}
