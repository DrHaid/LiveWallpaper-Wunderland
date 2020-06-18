using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
  public float ParallaxEffect;
  
  private float length;
  private float startPosition;
  private Transform mainCamera;

  void Start()
  {
    mainCamera = Camera.main.transform;
    startPosition = transform.position.x;
    length = GetComponent<SpriteRenderer>().bounds.size.x;
  }

  void FixedUpdate()
  {
    float temp = mainCamera.position.x * (1 - ParallaxEffect);
    float dist = mainCamera.position.x * ParallaxEffect;

    transform.position = new Vector3(startPosition + dist, transform.position.y, transform.position.z);

    if(temp > startPosition + length)
    {
      startPosition += length;
    }
    else if(temp < startPosition - length)
    {
      startPosition -= length;
    }
  }
}
