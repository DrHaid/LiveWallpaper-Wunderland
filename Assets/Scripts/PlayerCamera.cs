using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
  public float dampTime = 0.15f;
  public Transform target;

  private Vector3 velocity = Vector3.zero;
  private Transform cam;

  private void Start()
  {
    cam = Camera.main.transform;
  }

  void LateUpdate()
  {
    if (target)
    {
      Vector3 destination = cam.transform.position;
      destination.x = target.position.x;
      transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
    }

  }
}
