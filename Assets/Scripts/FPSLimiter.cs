using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
  public int maxFramerate;

  void Awake()
  {
    QualitySettings.vSyncCount = 0;
    Application.targetFrameRate = maxFramerate;
  }
}
