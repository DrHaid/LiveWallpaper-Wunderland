using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CowDeleter : MonoBehaviour
{
   private void OnCollisionEnter2D(Collision2D collision)
  {
    if(collision.gameObject.tag == "RecycleBin" && gameObject.tag != "DeletedCow")
    {
      DeleteCow();
    }
  }

  private void DeleteCow()
  {
    gameObject.tag = "DeletedCow";
    GameObject.Find("CowGameManager").GetComponent<CowGameManager>().InitializeCowGame();
    GameObject explosion = Instantiate(Resources.Load("Prefabs/Explosion", typeof(GameObject)),
       gameObject.transform.position + new Vector3(0f, 1f, 0f), 
       Quaternion.identity) as GameObject;
    Destroy(gameObject, explosion.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length * 0.25f);
  }
}
