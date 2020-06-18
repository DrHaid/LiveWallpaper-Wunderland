using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBuilder : MonoBehaviour
{
  public bool liveUpdateGenerator;
  public float Scale = 10;
  public float Amplification = 5;
  public float Spacing = 1;
  public float VerticalOffset = 0.5f;
  [Space]
  public int Index = 0;
  public int Count = 10;

  private MeshFilter meshFilter;
  private PolygonCollider2D polyCollider;
  private TerrainGenerator terrainGenerator;

  void Start()
  {
    meshFilter = GetComponent<MeshFilter>();
    polyCollider = GetComponent<PolygonCollider2D>();
    terrainGenerator = new TerrainGenerator(Scale, Amplification, Spacing, VerticalOffset);
  }

  private void FixedUpdate()
  {
    Index = (int)(Camera.main.transform.position.x / Spacing) - (int)((Count) / 2f);

    if (liveUpdateGenerator)
    {
      terrainGenerator = new TerrainGenerator(Scale, Amplification, Spacing, VerticalOffset);
    }
    meshFilter.mesh = terrainGenerator.GetMeshSection(Index, Count);

    polyCollider.pathCount = 1;
    polyCollider.SetPath(0, terrainGenerator.GetPolyColliderPoints(Index, Count));
  }
}
