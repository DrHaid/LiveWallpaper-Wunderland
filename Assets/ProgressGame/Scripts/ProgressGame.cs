using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressGame : MonoBehaviour
{
  [Range(0, 1)]
  public float Progress;
  public float ProgressPerEnemy;

  public GameObject UnlockableUIPrefab;
  public Slider Slider;
  public List<Unlockable> Unlockables;

  public static ProgressGame instance;

  private void Awake()
  {
    instance = this;
  }

  void Start()
  {
    InitUnlockablePositions();
  }

  void Update()
  {
    Slider.value = Progress;
    CheckUnlock();
  }

  private void InitUnlockablePositions()
  {
    var spacing = Slider.fillRect.rect.width / Unlockables.Count;
    for(int i = 0; i < Unlockables.Count; i++)
    {
      var leftAnchor = Slider.gameObject.transform.position;
      leftAnchor.x -= Slider.fillRect.rect.width / 2;
      Unlockables[i].UiObject = Instantiate(
        UnlockableUIPrefab,
        leftAnchor + new Vector3(spacing * (i + 1), 0, 0), 
        Quaternion.identity, 
        Slider.gameObject.transform);
      Unlockables[i].UiObject.GetComponent<Image>().sprite = Unlockables[i].Sprite;
      Unlockables[i].UnlockProgress = (float)(i + 1) / (float)Unlockables.Count;
    }
  }

  private void CheckUnlock()
  {
    foreach(var unlockable in Unlockables)
    {
      if(!unlockable.Unlocked && Progress >= unlockable.UnlockProgress)
      {
        unlockable.Unlock();
      }
    }
  }

  public void AddProgress()
  {
    Progress = Mathf.Clamp01(Progress + ProgressPerEnemy);
  }
}

[Serializable]
public class Unlockable
{
  [HideInInspector] public bool Unlocked;
  public Sprite Sprite;
  [HideInInspector] public GameObject UiObject;
  [HideInInspector] public float UnlockProgress;

  public void Unlock()
  {
    Unlocked = true;
    UiObject.transform.GetChild(0).gameObject.SetActive(true);
  }
}
