using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ErrorPooper : MonoBehaviour, IPointerClickHandler
{
  GameObject errorSpawnLocation;
  SpriteRenderer cowSprite;
  AudioSource audioSource;

  void Start()
  {
    cowSprite = gameObject.GetComponent<SpriteRenderer>();
    audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    errorSpawnLocation = gameObject.transform.GetChild(0).gameObject;
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    poopError();
  }

  [ContextMenu("Poop Error")]
  public void poopError()
  {
    //Instantiate Error Sprite, start animation and sound
    GameObject newErrorObject = Instantiate(Resources.Load("Prefabs/Error", typeof(GameObject)), 
      errorSpawnLocation.transform.position, Quaternion.identity) as GameObject;
    StartCoroutine(AnimateError(newErrorObject, (gameObject.transform.rotation.eulerAngles.y != 0)));
    audioSource.Play();
  }

  public IEnumerator AnimateError(GameObject errorObject, bool flipped)
  {
    float elapsedTime = 0f;
    float animationTime = 0.2f;
    Vector3 startPosition = errorObject.transform.position;
    Vector3 destination = startPosition + new Vector3(1.3f * (flipped ? 1 : -1), -0.8f, 0f);
    Vector3 spline = startPosition + new Vector3(0.9f * (flipped ? 1 : -1), 0f, 0f);
    Vector3 startScale = errorObject.transform.localScale;
    Vector3 endScale = new Vector3(1f, 1f, 1f);
    
    //Animation of the Error Sprite
    while (elapsedTime < animationTime)
    {
      elapsedTime += Time.deltaTime;
      Vector3 p1 = Vector3.Lerp(startPosition, spline, (elapsedTime / animationTime));
      Vector3 p2 = Vector3.Lerp(spline, destination, (elapsedTime / animationTime));
      errorObject.transform.position = Vector3.Lerp(p1, p2, (elapsedTime / animationTime));
      errorObject.transform.localScale = Vector3.Lerp(startScale, endScale, (elapsedTime / animationTime));
      yield return null;
    }
  }
}
