using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
  [Space]
  [Header("Terrain")]
  [HideInInspector]public bool liveUpdateGenerator;
  public float Scale = 10;
  public float Amplification = 5;
  public float Spacing = 1;
  public float VerticalOffset = 0.5f;
  [Space]
  [Header("Enemies")]
  public GameObject EnemyPrefab;
  public int SkipSpawn;
  public float SpawnRate;
  [Space]
  [Header("Rendering")]
  public float PositionX = 0;
  public int FieldOfView = 10;

  private MeshFilter meshFilter;
  private PolygonCollider2D polyCollider;
  private TerrainGenerator terrainGenerator;
  private EnemySpawner enemySpawner;

  void Start()
  {
    meshFilter = GetComponent<MeshFilter>();
    polyCollider = GetComponent<PolygonCollider2D>();
    terrainGenerator = new TerrainGenerator(Scale, Amplification, Spacing, VerticalOffset);
    enemySpawner = new EnemySpawner(SpawnRate, EnemyPrefab, SkipSpawn);
  }

  private void FixedUpdate()
  {
    PositionX = Camera.main.transform.position.x;
    int terrainPartCount = (int)(FieldOfView / Spacing);
    int terrainIndex = (int)(PositionX / Spacing) - (int)(terrainPartCount / 2f);

    if (liveUpdateGenerator)
    {
      terrainGenerator = new TerrainGenerator(Scale, Amplification, Spacing, VerticalOffset);
    }
    meshFilter.mesh = terrainGenerator.GetMeshSection(terrainIndex, terrainPartCount);

    polyCollider.pathCount = 1;
    polyCollider.SetPath(0, terrainGenerator.GetPolyColliderPoints(terrainIndex, terrainPartCount));

    enemySpawner.UpdateEnemies(terrainGenerator.terrainParts);
    enemySpawner.DrawEnemiesInView(PositionX, FieldOfView, Spacing);
  }
}
