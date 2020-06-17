using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoExplosionDeleter : MonoBehaviour
{
  void Start()
  {
    AnimationClip animClip = gameObject.GetComponent<Animator>().runtimeAnimatorController.animationClips[0];
    Destroy(gameObject, animClip.length);
  }
}
