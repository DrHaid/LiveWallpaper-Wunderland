using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Implements a Simon Says type of game to make a Cow lay an "Egg"
/// </summary>
public class CowGameManager : MonoBehaviour
{
  List<GameObject> cowList = new List<GameObject>();
  int nextCowIndex;

  void Start()
  {
    InitializeCowGame();
  }

  public void InitializeCowGame()
  {
    cowList = GameObject.FindGameObjectsWithTag("Cow").ToList();
    cowList.Shuffle();
    nextCowIndex = 0;
  }

  /// <summary>
  /// Checks if the cowObject is next in the sequence and returns specific errorType.
  /// (0 - Error, 1 - Warning, 2 - Egg)
  /// </summary>
  /// <param name="cowObject"></param>
  /// <returns></returns>
  public int GetErrorTypeForCow(GameObject cowObject)
  {
    if (cowObject == cowList[nextCowIndex])
    {
      nextCowIndex++;
      if(nextCowIndex >= cowList.Count)
      {
        //Sequence success, resetting.
        ResetSequence();
        return 2;
      }
      return 1;
    }
    //Sequence failed, resetting.
    nextCowIndex = 0;
    return 0;
  }

  void ResetSequence()
  {
    nextCowIndex = 0;
    cowList.Shuffle();
  }
}

public static class IListExtensions
{
  /// <summary>
  /// Shuffles elements of a List.
  /// </summary>
  public static void Shuffle<T>(this IList<T> ts)
  {
    var count = ts.Count;
    var last = count - 1;
    for (var i = 0; i < last; ++i)
    {
      var r = Random.Range(i, count);
      var tmp = ts[i];
      ts[i] = ts[r];
      ts[r] = tmp;
    }
  }
}