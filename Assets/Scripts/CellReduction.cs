using System.Collections.Generic;
using UnityEngine;

public class CellReduction : MonoBehaviour
{

    public int xSize;
    public int ySize;
    public int zSize;

    public void Reduction()
    {
        Mesh m = GetComponent<MeshFilter>().sharedMesh;
        Dictionary<int, Vector3> vertexPerCell = new Dictionary<int, Vector3>();

        m.RecalculateBounds();

        Vector3 extents = m.bounds.extents;
        extents.x += 0.01f;
        extents.y += 0.01f;
        extents.z += 0.01f;

        Vector3 step = Vector3.zero;
        step.x = (extents.x * 2f) / xSize;
        step.y = (extents.y * 2f) / ySize;
        step.z = (extents.z * 2f) / zSize;

        Vector3[] verts = new Vector3[m.vertices.Length];

        for (float x = -extents.x; x < extents.x; x += step.x)
        {
            for (float y = -extents.y; y < extents.y; y += step.y)
            {
                for (float z = -extents.z; z < extents.z; z += step.z)
                {

                    for (int i = 0; i < m.vertices.Length; i++)
                    {

                        // Is the vertex inside the cell ?
                        if (m.vertices[i].x <= x + step.x && m.vertices[i].x >= x &&
                            m.vertices[i].y <= y + step.y && m.vertices[i].y >= y &&
                            m.vertices[i].z <= z + step.z && m.vertices[i].z >= z)
                        {
                            Vector3 cell = new Vector3(x + (step.x / 2f), y + (step.y / 2f), z + (step.z / 2f));
                            vertexPerCell.Add(i, cell);
                        }
                    }
                }
            }
        }


        foreach (int i in vertexPerCell.Keys)
        {
            verts[i] = vertexPerCell[i];
        }

        m.vertices = verts;

        Debug.Log("Temps de réduction : " + Time.realtimeSinceStartup + "s");

        // Clean triangles ?
    }
}
