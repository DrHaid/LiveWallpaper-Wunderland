using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
  public GameObject EnemyObject { get; set; }

  public Enemy(Vector2 spawnPos, GameObject prefab)
  {
    EnemyObject = GameObject.Instantiate(prefab, spawnPos, Quaternion.identity);
  }
}
