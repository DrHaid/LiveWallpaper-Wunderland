using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class EnemySpawner
{
  public float spawnRate { get; set; }
  public GameObject enemyPrefab { get; set; }
  public int partsCheckedForSpawn { get; private set; }
  public int skipSpawn { get; private set; }
  public List<Enemy> enemies { get; set; }

  public EnemySpawner instance { get; private set; }

  public EnemySpawner(float spawnRate, GameObject prefab, int skipSpawn)
  {
    this.spawnRate = spawnRate;
    this.enemyPrefab = prefab;
    this.partsCheckedForSpawn = 0;
    this.skipSpawn = skipSpawn;
    this.enemies = new List<Enemy>();
    instance = this;
  }

  public void UpdateEnemies(List<TerrainPart> terrainParts)
  {
    foreach(var terrainPart in terrainParts.GetRange(partsCheckedForSpawn, terrainParts.Count - partsCheckedForSpawn))
    {
      if(Random.value <= spawnRate && partsCheckedForSpawn > skipSpawn)
      {
        SpawnEnemy(terrainPart);
      }
      partsCheckedForSpawn++;
    }
  }

  public void DrawEnemiesInView(float positionX, float fieldOfView, float spawnMargin)
  {
    var startX = positionX - (fieldOfView / 2);
    foreach(var enemy in enemies)
    {
      var enemyPosX = enemy.EnemyObject.transform.position.x;
      bool inView = (enemyPosX > (startX + spawnMargin) && enemyPosX < (startX  + fieldOfView - spawnMargin));
      enemy.EnemyObject.SetActive(inView);
    }
  }

  private void SpawnEnemy(TerrainPart terrainPart)
  {
    var enemy = new Enemy(terrainPart.vertices[1] + new Vector3(0f, 0.5f, 0f), enemyPrefab);
    enemies.Add(enemy);
  }

  public void DeleteEnemy(GameObject enemyToDelete)
  {
    enemies.Remove(enemies.FirstOrDefault(x => x.EnemyObject == enemyToDelete));
  }
}
