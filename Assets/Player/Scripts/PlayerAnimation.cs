using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: i dont like this way of triggering animations, look for alternative
public class PlayerAnimation : MonoBehaviour
{
  public static PlayerAnimation current;

  private Animator animator;
  private bool isWalking;
  private bool isJumping;

  private void Awake()
  {
    current = this;
    animator = GetComponent<Animator>();
  }

  public void PlayerWalk(bool walking)
  {
    if(isWalking == walking)
    {
      return;
    }
    isWalking = walking;
    animator.SetBool("IsWalking", walking);
  }

  public bool Attack()
  {
    if (animator.GetCurrentAnimatorStateInfo(0).IsTag("0"))
    {
      return false;
    }
    animator.SetTrigger("Attack");
    return true;
  }

  public bool SpecialAttack()
  {
    if (animator.GetCurrentAnimatorStateInfo(0).IsTag("1"))
    {
      return false;
    }
    animator.SetTrigger("SpecialAttack");
    return true;
  }

  public void PlayerJump(bool jumping)
  {
    if (isJumping == jumping)
    {
      return;
    }
    isJumping = jumping;
    animator.SetBool("IsJumping", jumping);
  }

}
