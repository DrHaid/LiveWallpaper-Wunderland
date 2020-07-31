using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
  private SpriteRenderer spriteRenderer;
  private GameObject uiCarriage;
  private CanvasGroup canvasGroup;

  public static SceneTransitioner instance;

  void Start()
  {
    instance = this;
    canvasGroup = FindObjectOfType<CanvasGroup>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    uiCarriage = gameObject.transform.GetChild(0).gameObject;
    if (TransitionInformation.DoTransition)
    {
      var color = spriteRenderer.color;
      color.a = 1f;
      spriteRenderer.color = color;
      TransitionInformation.DoTransition = false;
      gameObject.transform.GetComponent<Animator>().Play("Transition", -1, TransitionInformation.PlaybackTime);
      if (canvasGroup != null)
      {
        canvasGroup.alpha = 0;
        LeanTween.value(canvasGroup.gameObject, (float f) => { canvasGroup.alpha = f; }, 0f, 1f, 0.6f).setEase(LeanTweenType.linear);
      }
      LeanTween.alpha(gameObject, 0f, 0.6f).setEase(LeanTweenType.linear);
    }
  }

  public void StartTransition(string sceneName)
  {
    if(canvasGroup != null)
    {
      LeanTween.value(canvasGroup.gameObject, (float f) => { canvasGroup.alpha = f; }, 1f, 0f, 0.6f).setEase(LeanTweenType.linear);
    }
    LeanTween.alpha(gameObject, 1f, 0.6f).setEase(LeanTweenType.linear).setOnComplete(() => {
      MoveUICarriage(sceneName);
    });
  }

  private void MoveUICarriage(string sceneName)
  {
    var targetPosX = uiCarriage.transform.position.x - gameObject.transform.position.x;
    LeanTween.move(uiCarriage, gameObject.transform.position - new Vector3(targetPosX, 0), 2).setEase(LeanTweenType.linear).setOnComplete(() => {
      ChangeScene(sceneName);
    });
  }

  private void ChangeScene(string sceneName)
  {
    TransitionInformation.DoTransition = true;
    TransitionInformation.PlaybackTime = gameObject.transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
    SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
  }
}

public static class TransitionInformation
{
  public static bool DoTransition { get; set; }
  public static float PlaybackTime { get; set; }
}
