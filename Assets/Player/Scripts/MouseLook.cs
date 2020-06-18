using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
	public static bool isFacingRight = true;
	private Vector3 originalScale;

	private void Awake()
	{
		originalScale = transform.localScale;
	}

	private void Flip(bool faceRight)
	{
		isFacingRight = faceRight;
		var newScale = transform.localScale;
		newScale.x = faceRight ? originalScale.x : -originalScale.x;
		transform.localScale = newScale;
	}
	
	private void OnMouseLook(InputValue inputValue)
	{
		var mousePosX = inputValue.Get<Vector2>().x;
		var playerPosX = Camera.main.WorldToScreenPoint(gameObject.transform.position).x;
		Flip(mousePosX > playerPosX);
	}
}
