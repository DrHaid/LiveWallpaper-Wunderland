using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayAfterTime : MonoBehaviour
{
  public float lifeTime;
  SpriteRenderer sprite;

  void Start()
  {
    StartCoroutine(StartLifeTime());
    sprite = gameObject.GetComponent<SpriteRenderer>();
  }

  public IEnumerator StartLifeTime()
  {
    yield return new WaitForSeconds(lifeTime);

    float elapsedTime = 0f;
    float fadeTime = 2f;
    while(elapsedTime < fadeTime)
    {
      elapsedTime += Time.deltaTime;
      sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0f, (elapsedTime / fadeTime)));
      yield return null;
    }
    Destroy(gameObject);
  }
}
