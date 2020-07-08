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
  public float ProgressLossPerEnemyHit;
  public float DamageTimeOutDuration;


  public GameObject UnlockableUIPrefab;
  public Slider Slider;
  
  private List<ProgressBarUnlockable> Unlockables;
  private bool damageTimeOut;
  private AudioSource oofSource;

  public static ProgressGame instance;

  private void Awake()
  {
    instance = this;
    oofSource = GetComponent<AudioSource>();
  }

  void Start()
  {
    InitUnlockablePositions();
    Progress = ProgressSaveManager.Progress;
  }

  void Update()
  {
    Slider.value = Progress;
    CheckUnlock();
  }

  private void InitUnlockablePositions()
  {
    Unlockables = ProgressSaveManager.GetProgressBarUnlockables();
    var spacing = Slider.fillRect.rect.width / Unlockables.Count;

    for(int i = 0; i < Unlockables.Count; i++)
    {
      var leftAnchor = Slider.gameObject.transform.position;
      leftAnchor.x -= Slider.fillRect.rect.width / 2;
      Unlockables[i].Initialize(leftAnchor, spacing, i, Unlockables.Count, UnlockableUIPrefab, Slider.transform);
    }
  }

  private void CheckUnlock()
  {
    foreach(var unlockable in Unlockables)
    {
      if(!unlockable.Unlocked && Progress >= unlockable.UnlockProgress)
      {
        unlockable.Unlock(true);
        ProgressSaveManager.OverwriteSave(Progress, Unlockables);
      }
      else if(unlockable.Unlocked && Progress < unlockable.UnlockProgress)
      {
        unlockable.Unlock(false);
        ProgressSaveManager.OverwriteSave(Progress, Unlockables);
      }
    }
  }

  public void AddProgress()
  {
    Progress = Mathf.Clamp01(Progress + ProgressPerEnemy);
    ProgressSaveManager.OverwriteSave(Progress, Unlockables);
  }

  public void RemoveProgress()
  {
    if (damageTimeOut)
    {
      return;
    }
    Progress = Mathf.Clamp01(Progress - ProgressLossPerEnemyHit);
    ProgressSaveManager.OverwriteSave(Progress, Unlockables);
    oofSource.Play();
    StartCoroutine(SetTimeOut());
  }

  IEnumerator SetTimeOut()
  {
    damageTimeOut = true;
    yield return new WaitForSeconds(DamageTimeOutDuration);
    damageTimeOut = false;
  }
}

public class ProgressBarUnlockable
{
  public bool Unlocked;
  public Sprite Sprite;
  public GameObject UiObject;
  public float UnlockProgress;

  public void Initialize(Vector3 leftAnchor, float spacing, int index, int unlockableCount, GameObject uiPrefab, Transform parent)
  {
    UiObject = GameObject.Instantiate(
        uiPrefab,
        leftAnchor + new Vector3(spacing * (index + 1), 0, 0),
        Quaternion.identity,
        parent);
    UiObject.GetComponent<Image>().sprite = Sprite;
    UnlockProgress = (float)(index + 1) / (float)unlockableCount;

    if (Unlocked)
    {
      Unlock(true);
    }
  }

  public void Unlock(bool unlock)
  {
    Unlocked = unlock;
    UiObject.transform.GetChild(0).gameObject.SetActive(unlock);
  }
}
