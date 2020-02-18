using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ErrorPooper : MonoBehaviour, IPointerClickHandler
{
  GameObject errorSpawnLocation;
  SpriteRenderer cowSprite;
  CowGameManager cowGameManager;

  List<string> errorNames = new List<string> { "Error", "Warning", "Egg"};

  void Start()
  {
    cowSprite = gameObject.GetComponent<SpriteRenderer>();
    errorSpawnLocation = gameObject.transform.GetChild(0).gameObject;
    try
    {
      cowGameManager = GameObject.Find("CowGameManager").GetComponent<CowGameManager>();
    }
    catch (System.NullReferenceException)
    {
      Debug.LogWarning("Could not find CowGameManager for CowObject: " + gameObject.name);
    }
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if(eventData.button == PointerEventData.InputButton.Left)
    {
      PoopError();
    }
  }

  [ContextMenu("Poop Error")]
  public void PoopError()
  {
    //Check if there is a cowGameManager, if not do the standard Error
    string resourcePath = "Prefabs/Error";
    if (cowGameManager != null)
      resourcePath = "Prefabs/" + errorNames[cowGameManager.GetErrorTypeForCow(gameObject)];
    
    //Instantiate Error Sprite, start animation and sound
    GameObject newErrorObject = Instantiate(Resources.Load(resourcePath, typeof(GameObject)), 
      errorSpawnLocation.transform.position, Quaternion.identity) as GameObject;
    StartCoroutine(AnimateError(newErrorObject, (gameObject.transform.rotation.eulerAngles.y != 0)));
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
