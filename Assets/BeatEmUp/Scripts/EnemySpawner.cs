using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EnemySpawner
{
  public float spawnRate { get; set; }
  public GameObject enemyPrefab { get; set; }
  public int partsCheckedForSpawn { get; private set; }
  public List<Enemy> enemies { get; set; }

  public EnemySpawner(float spawnRate, GameObject prefab)
  {
    this.spawnRate = spawnRate;
    this.enemyPrefab = prefab;
    this.partsCheckedForSpawn = 0;
    this.enemies = new List<Enemy>();
  }

  public void UpdateEnemies(List<TerrainPart> terrainParts)
  {
    foreach(var terrainPart in terrainParts.GetRange(partsCheckedForSpawn, terrainParts.Count - partsCheckedForSpawn))
    {
      if(Random.value <= spawnRate)
      {
        SpawnEnemy(terrainPart);
      }
      partsCheckedForSpawn++;
    }
  }

  public void DrawEnemiesInView(float positionX, float fieldOfView)
  {
    var startX = positionX - (fieldOfView / 2);
    foreach(var enemy in enemies)
    {
      var enemyPosX = enemy.EnemyObject.transform.position.x;
      bool inView = (enemyPosX > startX && enemyPosX < startX + fieldOfView);
      enemy.EnemyObject.SetActive(inView);
    }
  }

  private void SpawnEnemy(TerrainPart terrainPart)
  {
    var enemy = new Enemy(terrainPart.vertices[1] + new Vector3(0f, 0.1f, 0f), enemyPrefab);
    enemies.Add(enemy);
  }
}
