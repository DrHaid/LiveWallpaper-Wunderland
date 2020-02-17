using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSettings : MonoBehaviour
{
  public int maxFramerate;
  public SpriteRenderer background;

  //Set max FPS
  void Awake()
  {
    QualitySettings.vSyncCount = 0;
    Application.targetFrameRate = maxFramerate;
  }
  
  public void FitCameraToBackground()
  {
    float screenRatio = (float)Screen.width / (float)Screen.height;
    float spriteRatio = background.sprite.rect.width / background.sprite.rect.height;
    Debug.Log(screenRatio + " - " + spriteRatio);
    if(spriteRatio >= screenRatio)
    {
      Camera.main.orthographicSize = background.bounds.size.x * Screen.height / Screen.width * 0.5f;
    }
    else
    {
      Camera.main.orthographicSize = background.bounds.size.y / 2;
    }
  }
}
