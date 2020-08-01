using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CowHatSetter : MonoBehaviour
{
  void Start()
  {
    var cowHatSpriteRenderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
    var unlocked = ProgressSaveManager.UnlockableJson.Where(unlockable => unlockable.unlocked).ToList();
    if (unlocked == null || unlocked.Count == 0)
    {
      return;
    }
    var hatName = ProgressSaveManager.UnlockableJson[Random.Range(0, unlocked.Count)].name;
    cowHatSpriteRenderer.sprite = Resources.Load<Sprite>("Unlockables/" + hatName);
  }
}
