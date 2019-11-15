using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderController : MonoBehaviour
{
  public GameObject wanderArea;
  public float maxWanderDistance;
  public float maxIdleTime;
  public float wanderingSpeed;
  private Collider2D wanderAreaCollider;
  private bool usesWanderArea;
  private float previousX;
  private bool isReadyForWander;

  void Start()
  {
    usesWanderArea = (wanderArea != null) ? true : false;
    if (usesWanderArea)
      wanderAreaCollider = wanderArea.GetComponent<PolygonCollider2D>();
    
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
    //Find new random point. If point is not in wanderArea, retry
    do
    {
      newDestination =
      new Vector3(gameObject.transform.position.x + Random.Range(-maxWanderDistance, maxWanderDistance),
      gameObject.transform.position.y + Random.Range(-maxWanderDistance, maxWanderDistance),
      gameObject.transform.position.z);
      tryCounter++;
    } while (usesWanderArea && !wanderAreaCollider.OverlapPoint(newDestination) && (tryCounter < 100));

    //Too many tries indicates that WanderArea is too far away.
    if (tryCounter >= 100)
    {
      Debug.LogWarning("No WanderArea near Object found. Readjusting...");
      newDestination = wanderArea.transform.position;
    }
    return newDestination;
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
