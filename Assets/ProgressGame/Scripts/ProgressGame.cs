using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressGame : MonoBehaviour
{
  [Range(0, 1)]
  public float Progress;

  public GameObject UnlockableUIPrefab;
  public List<Unlockable> Unlockables;

  private Slider slider;

  void Start()
  {
    slider = GetComponent<Slider>();
    PositionUnlockables();
  }

  void Update()
  {
    slider.value = Progress;
  }

  private void PositionUnlockables()
  {
    var spacing = slider.fillRect.rect.width / Unlockables.Count;
    for(int i = 0; i < Unlockables.Count; i++)
    {
      var leftAnchor = gameObject.transform.position;
      leftAnchor.x -= slider.fillRect.rect.width / 2;
      Unlockables[i].UiObject = Instantiate(
        UnlockableUIPrefab,
        leftAnchor + new Vector3(spacing * (i + 1), 0, 0), 
        Quaternion.identity, 
        gameObject.transform);
      Unlockables[i].UiObject.GetComponent<Image>().sprite = Unlockables[i].Sprite;
    }
  }
}

[Serializable]
public class Unlockable
{
  public bool Unlocked;
  public Sprite Sprite;
  public GameObject UiObject;
}
