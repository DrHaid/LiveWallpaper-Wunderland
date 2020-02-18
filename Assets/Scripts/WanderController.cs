using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderController : MonoBehaviour
{
  public float maxWanderDistance;
  public float maxIdleTime;
  public float wanderingSpeed;
  private Collider2D wanderAreaCollider;
  private bool usesWanderArea;
  private float previousX;
  private bool isReadyForWander;

  void Start()
  {
    //Try finding WanderArea
    try
    {
      wanderAreaCollider = GameObject.FindGameObjectWithTag("WanderArea").GetComponent<PolygonCollider2D>();
      usesWanderArea = true;
    }
    catch (System.NullReferenceException)
    {
      Debug.LogWarning($"No Object with Tag WanderArea found or Object has no PolygonCollider2D. Wandering of {gameObject.name} will be without bounds.");
      usesWanderArea = false;
    }

    previousX = gameObject.transform.position.x;
    isReadyForWander = true;
  }

  void Update()
  {
    //Make sprite face the moving direction
    float facingValue = (gameObject.transform.position.x - previousX);
    if (facingValue != 0f)
      gameObject.transform.rotation = Quaternion.Euler(0, ((facingValue < 0) ? 180 : 0), 0);
    previousX = gameObject.transform.position.x;

    //Start wander when ready
    if (isReadyForWander)
    {
      Vector3 newDestination = GetRandomDestination();
      StartCoroutine(WanderToPosition(newDestination));
    }
  }

  Vector3 GetRandomDestination()
  {
    int tryCounter = 0;
    Vector3 newDestination = Vector3.zero;
    //Find new random point. If point is not in WanderArea, retry
    do
    {
      newDestination =
      new Vector3(gameObject.transform.position.x + Random.Range(-maxWanderDistance, maxWanderDistance),
      gameObject.transform.position.y + Random.Range(-maxWanderDistance, maxWanderDistance),
      gameObject.transform.position.z);
      tryCounter++;
    } while (usesWanderArea && !IsPointWithinBounds(newDestination) && (tryCounter < 100));

    //Too many tries indicates that WanderArea is too far away.
    if (tryCounter >= 100)
    {
      Debug.LogWarning("No WanderArea near Object found. Readjusting...");
      newDestination = wanderAreaCollider.gameObject.transform.position;
    }
    return newDestination;
  }

  bool IsPointWithinBounds(Vector3 newDestination)
  {
    //Check if point is in viewport and in WanderArea
    bool isWithinWanderArea = wanderAreaCollider.OverlapPoint(newDestination);
    Vector3 viewPortPos = Camera.main.WorldToViewportPoint(newDestination);
    bool isWithiView = (viewPortPos.x >= 0 && viewPortPos.x <= 1) 
      && (viewPortPos.y >= 0 
      && viewPortPos.y <= 1) 
      && (viewPortPos.z > 0);
    return isWithinWanderArea && isWithiView;
  }

  public IEnumerator WanderToPosition(Vector3 destination)
  {
    isReadyForWander = false;
    float elapsedTime = 0f;
    float animationTime = Vector3.Distance(gameObject.transform.position, destination) / wanderingSpeed;
    Vector3 startPosition = gameObject.transform.position;

    //Moving
    while (elapsedTime < animationTime)
    {
      elapsedTime += Time.deltaTime;
      gameObject.transform.position = Vector3.Lerp(startPosition, destination, (elapsedTime / animationTime));
      yield return null;
    }

    //Idling
    yield return new WaitForSeconds(Random.Range(0f, maxIdleTime));
    isReadyForWander = true;
  }
}
