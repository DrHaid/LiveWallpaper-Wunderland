using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ProjectileShooter : MonoBehaviour
{
  public GameObject projectile;
  public float maxStretch;
  public float strengthMultiplier;
  public LineRenderer line;
  private float power;
  private Vector3 startPos;
  private Vector3 destPos;
  private bool isDrawing;

  void Update()
  {
    if (Mouse.current.leftButton.wasPressedThisFrame)
    {
      startPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
      startPos.Scale(new Vector3(1, 1, 0));
      isDrawing = true;
      line.enabled = true;
    }
    if (Mouse.current.leftButton.wasReleasedThisFrame)
    {
      isDrawing = false;
      line.enabled = false;
      if(Vector3.Distance(startPos, destPos) >= 0.2) //Don't shoot on click
      {
        ShootProjectile();
      }
    }
    if (isDrawing)
    {
      destPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
      destPos.Scale(new Vector3(1, 1, 0));
      SetLinePositions();
    }
  }

  void SetLinePositions()
  {
    power = Vector3.Distance(startPos, destPos).Remap01(0, maxStretch);
    Color lineColor;
    if (power < 0.5f)
    {
      lineColor = Color.Lerp(Color.green, Color.yellow, power*2);
    }
    else
    {
      lineColor = Color.Lerp(Color.yellow, Color.red, (power-0.5f)*2);
    }
    line.startColor = line.endColor = lineColor;
    line.SetPosition(0, startPos);
    line.SetPosition(1, destPos);
  }

  void ShootProjectile()
  {
    Vector3 dir = (startPos - destPos).normalized;
    GameObject recycleBin = Instantiate(projectile,
       startPos,
       Quaternion.identity) as GameObject;
    recycleBin.GetComponent<Rigidbody2D>()
      .AddForce(new Vector2(dir.x, dir.y) * (power*strengthMultiplier), ForceMode2D.Impulse);
  }
}

public static class ExtensionMethods
{
  public static float Remap01(this float value, float from1, float to1)
  {
    return Mathf.Clamp01((value - from1) / (to1 - from1));
  }
}
