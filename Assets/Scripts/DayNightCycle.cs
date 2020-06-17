using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
  public float minutesPerCycle;
  public Sprite dayBackground;
  public Sprite nightBackground;
  private bool isDay = true;

  void Start()
  {
    if(minutesPerCycle > 0)
    {
      InvokeRepeating("ToggleBackground", minutesPerCycle * 60, minutesPerCycle * 60);
    }
  }

  [ContextMenu("Toggle Time")]
  void ToggleBackground()
  {
    isDay = !isDay;
    gameObject.GetComponent<SpriteRenderer>().sprite = isDay ? dayBackground : nightBackground;
  }
}
