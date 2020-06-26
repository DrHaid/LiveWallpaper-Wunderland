using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPart
{
  public List<Vector3> vertices { get; set; }
  public List<int> triangles { get; set; }

  public TerrainPart(TerrainPart previous, float height, float spacing, float verticalOffset)
  {
    triangles = new List<int>
    {
      0, 1, 2,
      1, 3, 2
    };

    vertices = new List<Vector3>();
    vertices.Add((previous == null) ? new Vector3(0, 0, 0) : previous.vertices[2]);
    vertices.Add((previous == null) ? new Vector3(0, 1 + verticalOffset, 0) : previous.vertices[3]);

    vertices.Add(vertices[0] + new Vector3(spacing, 0, 0));
    vertices.Add(vertices[2] + new Vector3(0, height + verticalOffset, 0));
  }
}
