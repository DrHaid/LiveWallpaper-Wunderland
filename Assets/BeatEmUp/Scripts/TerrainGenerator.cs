using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator
{
  public List<TerrainPart> terrainParts { get; set; }
  public float scale { get; set; }
  public float amplification { get; set; }
  public float spacing { get; set; }
  public float verticalOffset { get; set; }

  public TerrainGenerator(float scale, float amplification, float spacing, float verticalOffset)
  {
    this.scale = scale;
    this.amplification = amplification;
    this.spacing = spacing;
    this.verticalOffset = verticalOffset;
    terrainParts = new List<TerrainPart>();
  }

  public Mesh GetMeshSection(int index, int count)
  {
    if(index < 0 || count < 0)
    {
      Debug.LogWarning("Could not GetMeshSection. index or count negative");
      return null;
    }
    if(terrainParts.Count == 0)
    {
      GenerateMeshSection(0, index + count); //TODO: Bad! Shouldn't generate everything from 0
    }
    else if(index + count > terrainParts.Count)
    {
      GenerateMeshSection(terrainParts.Count, count - ((terrainParts.Count) - index));
    }
    return GetBuiltTerrainMesh(index, count);
  }

  private void GenerateMeshSection(int index, int count)
  {
    for (int i = index; i < index + count; i++)
    {
      var height = Mathf.PerlinNoise(0, i / scale) * amplification;
      if (i == 0)
      {
        terrainParts.Add(new TerrainPart(null, height, spacing, verticalOffset));
        continue;
      }
      terrainParts.Add(new TerrainPart(terrainParts[i - 1], height, spacing, verticalOffset));
    }
  }

  private Mesh GetBuiltTerrainMesh(int index, int count)
  {
    Mesh terrainMesh = new Mesh();
    terrainMesh.Clear();

    var vertices = new List<Vector3>();
    var triangles = new List<int>();
    for(int i = index; i < index + count; i++)
    {
      vertices.AddRange(terrainParts[i].vertices);
      var addedTriangleIndices = new List<int>();
      foreach(var triangle in terrainParts[i].triangles)
      {
        addedTriangleIndices.Add(triangle + (4 * (i - index)));
      }
      triangles.AddRange(addedTriangleIndices);
    }
    terrainMesh.vertices = vertices.ToArray();
    terrainMesh.triangles = triangles.ToArray();
    terrainMesh.uv = GetUvs(terrainMesh.vertices);
    terrainMesh.RecalculateNormals();
    return terrainMesh;
  }

  public Vector2[] GetUvs(Vector3[] vertices)
  {
    Vector2[] uvs = new Vector2[vertices.Length];
    for (int i = 0; i < uvs.Length; i++)
    {
      uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
    }
    return uvs;
  }

  public List<Vector2> GetPolyColliderPoints(int index, int count)
  {
    var polyColliderPoints = new List<Vector2>();
    polyColliderPoints.Add(terrainParts[index].vertices[0]);
    foreach (var item in terrainParts.GetRange(index, count))
    {
      polyColliderPoints.Add(item.vertices[1]);
    }
    polyColliderPoints.Add(terrainParts[(index + count) - 1].vertices[0]); //TODO: - 1 not good. last segment has no collision
    return polyColliderPoints;  
  }
}
