using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
  public float LongClickDuration;
  public float MousePlayerDistanceThreshold;

  CharacterController2D characterController;
  PlayerAttack playerAttack;

  float totalDownTime;
  bool clicking;

  void Start()
  {
    characterController = FindObjectOfType<CharacterController2D>();
    playerAttack = FindObjectOfType<PlayerAttack>();
  }

  void Update()
  {
    TryPerformAttack();

    float playerX = Camera.main.WorldToScreenPoint(gameObject.transform.position).x;
    float mouseX = Input.mousePosition.x;

    float moveMuliplier = (Mathf.Abs(playerX - mouseX) < MousePlayerDistanceThreshold) ? 0 : 1;
    float moveDirection = (playerX < mouseX) ? 1 : -1;

    characterController.Move(moveMuliplier * moveDirection, Input.GetMouseButtonDown(1));
  }

  void TryPerformAttack()
  {
    if (clicking)
    {
      totalDownTime += Time.deltaTime;
    }

    if (Input.GetMouseButtonDown(0))
    {
      clicking = true;
    }

    //Long Click
    if(totalDownTime >= LongClickDuration && clicking)
    {
      clicking = false;
      totalDownTime = 0;
      playerAttack.OnSpecialAttack();
    }

    //Short Click
    if (Input.GetMouseButtonUp(0) && clicking)
    {
      clicking = false;
      totalDownTime = 0;
      playerAttack.OnMainAttack();
    }
  }
}
