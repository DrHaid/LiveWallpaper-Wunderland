using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CowDeleter : MonoBehaviour, IPointerClickHandler
{
  public void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.button == PointerEventData.InputButton.Right)
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
