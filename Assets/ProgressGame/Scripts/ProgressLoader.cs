using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressLoader : MonoBehaviour
{
  public float defaultProgress;
  public List<UnlockableSave> StandardUnlockables;

  void Awake()
  {
    if(ProgressSaveManager.UnlockableJson != null)
    {
      return;
    }

    if (ProgressSaveManager.LoadSave())
    {
      return;
    }

    //Create new save file with standard values
    ProgressSaveManager.WriteToSave(defaultProgress, StandardUnlockables);
  }
}