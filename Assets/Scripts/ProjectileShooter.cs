using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectileShooter : MonoBehaviour
{
  public GameObject projectile;
  public float strengthMultiplier;
  public LineRenderer line;
  private float power;
  private Vector3 startPos;
  private Vector3 destPos;
  private bool isDrawing;

  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      startPos.Scale(new Vector3(1, 1, 0));
      isDrawing = true;
      line.enabled = true;
      Debug.Log("Down");
    }
    if (Input.GetMouseButtonUp(0))
    {
      isDrawing = false;
      line.enabled = false;
      if(Vector3.Distance(startPos, destPos) >= 0.1) //Don't react on click
      {
        ShootProjectile();
      }
      Debug.Log("Up");
    }
    if (isDrawing)
    {
      destPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      destPos.Scale(new Vector3(1, 1, 0));
      SetLinePositions();
    }
  }

  void SetLinePositions()
  {
    power = Vector3.Distance(startPos, destPos).Remap01(0, 10);
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
    Debug.Log("Bang!");
    Vector3 dir = (startPos - destPos).normalized;
    GameObject recycleBin = Instantiate(Resources.Load("Prefabs/RecycleBin", typeof(GameObject)),
       startPos,
       Quaternion.identity) as GameObject;
    recycleBin.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir.x, dir.y) * (power*strengthMultiplier), ForceMode2D.Impulse);
  }
}

public static class ExtensionMethods
{
  public static float Remap01(this float value, float from1, float to1)
  {
    return Mathf.Clamp01((value - from1) / (to1 - from1));
  }
}
