using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PullerHandler : MonoBehaviour, IPointerClickHandler
{
  public float FinalYPosition;

  public float distanceAddOnCowDestoy;
  public float retractionSpeed;

  public GameObject carriagePrefab;

  private float startYPosition;
  private bool fullyDescended;

  public static PullerHandler instance;

  private void Awake()
  {
    instance = this;
  }

  void Start()
  {
    startYPosition = gameObject.transform.position.y;
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if (fullyDescended && eventData.button == PointerEventData.InputButton.Left)
    {
      gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
      gameObject.GetComponent<BoxCollider2D>().enabled = false;
      Instantiate(carriagePrefab, gameObject.transform.position + new Vector3(0, startYPosition, 0), Quaternion.identity);
      Destroy(gameObject, 10);
    }
  }

  [ContextMenu("boop")]
  public void OnCowDestroy()
  {
    if (fullyDescended)
    {
      return;
    }
    gameObject.transform.position -= new Vector3(0, distanceAddOnCowDestoy, 0);
  }

  void FixedUpdate()
  {
    fullyDescended = gameObject.transform.position.y <= FinalYPosition;
    if (fullyDescended || gameObject.transform.position.y > startYPosition)
    {
      return;
    }
    gameObject.transform.position += new Vector3(0, Time.fixedDeltaTime * retractionSpeed, 0);
  }
}
