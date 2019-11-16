﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggHatcher : MonoBehaviour
{
  public float timeUntilHatch;
  public GameObject leftEggSide;
  public GameObject rightEggSide;
  SpriteRenderer leftSprite;
  SpriteRenderer rightSprite;

  // Start is called before the first frame update
  void Start()
  {
    leftSprite = leftEggSide.GetComponent<SpriteRenderer>();
    rightSprite = rightEggSide.GetComponent<SpriteRenderer>();
    StartCoroutine(StartHatchTime());
  }

  /// <summary>
  /// Turn back, while you still can
  /// </summary>
  /// <returns></returns>
  public IEnumerator StartHatchTime()
  {
    yield return new WaitForSeconds(timeUntilHatch);

    //Egg crack animation
    float elapsedCrackTime = 0f;
    float crackTime = 0.2f;
    while (elapsedCrackTime < crackTime)
    {
      elapsedCrackTime += Time.deltaTime;
      leftSprite.transform.rotation = Quaternion.Euler(Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 0, 50), (elapsedCrackTime / crackTime)));
      rightSprite.transform.rotation = Quaternion.Euler(Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 0, -50), (elapsedCrackTime / crackTime)));
      yield return null;
    }

    //Spawn new cow
    GameObject newCow = Instantiate(Resources.Load("Prefabs/Cow", typeof(GameObject)),
      gameObject.transform.position, Quaternion.identity) as GameObject;
    newCow.transform.localScale = new Vector3(0, 0, 1);
    float elapsedCowHatchTime = 0f;
    float cowHatchTime = 0.2f;
    while (elapsedCowHatchTime < cowHatchTime)
    {
      elapsedCowHatchTime += Time.deltaTime;
      newCow.transform.localScale = Vector3.Lerp(new Vector3(0, 0, 1), new Vector3(1, 1, 1), (elapsedCowHatchTime / cowHatchTime));
      yield return null;
    }
    GameObject.Find("CowGameManager").GetComponent<CowGameManager>().InitializeCowGame();
    yield return new WaitForSeconds(2);

    //Fade out animation
    float elapsedFadeTime = 0f;
    float fadeTime = 2f;
    while (elapsedFadeTime < fadeTime)
    {
      elapsedFadeTime += Time.deltaTime;
      leftSprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0f, (elapsedFadeTime / fadeTime)));
      rightSprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0f, (elapsedFadeTime / fadeTime)));
      yield return null;
    }
    Destroy(gameObject);
  }
}
