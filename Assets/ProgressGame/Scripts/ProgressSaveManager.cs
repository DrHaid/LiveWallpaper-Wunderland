using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ProgressSaveManager
{
  private const string pathSuffix = "/wunderland/unlockables.json";

  public static float Progress;
  public static List<UnlockableSave> UnlockableJson;

  public static bool LoadSave()
  {
    var path = GetPath();
    if (!File.Exists(path))
    {
      return false;
    }
    var fileContent = File.ReadAllText(path);
    var jsonContent = JsonUtility.FromJson<UnlockableJson>(fileContent);
    Progress = jsonContent.progressSave;
    UnlockableJson = jsonContent.unlockableSaves;
    return true;
  }

  public static void WriteToSave(float progress, List<UnlockableSave> unlockables)
  {
    Progress = progress;
    UnlockableJson = unlockables;
    var path = GetPath();

    if (!Directory.Exists(Path.GetDirectoryName(path)))
    {
      Directory.CreateDirectory(Path.GetDirectoryName(path));
    }
    var json = new UnlockableJson() { progressSave = progress, unlockableSaves = UnlockableJson };
    File.WriteAllText(path, JsonUtility.ToJson(json));
  }

  public static void OverwriteSave(float progress, List<ProgressBarUnlockable> progressBarUnlockables)
  {
    var updatedUnlockables = new List<UnlockableSave>();
    foreach (var progBarUnlockable in progressBarUnlockables)
    {
      var updatedUnlockable = new UnlockableSave();
      updatedUnlockable.name = progBarUnlockable.Sprite.name;
      updatedUnlockable.unlocked = progBarUnlockable.Unlocked;
      updatedUnlockables.Add(updatedUnlockable);
    }
    WriteToSave(progress, updatedUnlockables);
  }

  public static List<ProgressBarUnlockable> GetProgressBarUnlockables()
  {
    var progressBarUnlockables = new List<ProgressBarUnlockable>();
    foreach(var unlockableSave in UnlockableJson)
    {
      var progBarUnlockable = new ProgressBarUnlockable();
      progBarUnlockable.Unlocked = unlockableSave.unlocked;
      progBarUnlockable.Sprite = Resources.Load<Sprite>("Unlockables/" + unlockableSave.name);
      progressBarUnlockables.Add(progBarUnlockable);
    }
    return progressBarUnlockables;
  }

  private static string GetPath()
  {
    return Path.Combine(Application.dataPath, pathSuffix);
  }
}

#region Serializables for JSON parsing

[Serializable]
public class UnlockableSave
{
  public string name;
  public bool unlocked;
}

[Serializable]
public class UnlockableJson
{
  public float progressSave;
  public List<UnlockableSave> unlockableSaves;
}

#endregion