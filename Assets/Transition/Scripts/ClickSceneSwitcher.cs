using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ClickSceneSwitcher : MonoBehaviour, IPointerClickHandler
{
  public void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.button == PointerEventData.InputButton.Left)
    {
      var scenes = new List<string> { "BeatEmUp", "Wunderland" };
      scenes.Remove(SceneManager.GetActiveScene().name);
      SceneTransitioner.instance.StartTransition(scenes[0]);
      Destroy(this);
    }
  }
}