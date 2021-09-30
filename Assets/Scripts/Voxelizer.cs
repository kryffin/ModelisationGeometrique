using System.Collections.Generic;
using UnityEngine;

public class Voxelizer : MonoBehaviour
{

    public int xSize, ySize, zSize;

    private List<Vector3> voxelsPos;

    private Vector3 step;

    private Mesh m;

    public Material voxelMaterial;

    public enum Operation
    {
        UNION, INTERSECT
    };
    public Operation op;

    private void Init()
    {
        voxelsPos = new List<Vector3>();
    }

    // Displays only spheres crossing other spheres
    private bool Intersect(Vector3 v, Object[] objects, float radius, Vector3 offset)
    {
        int cpt = 0;
        foreach (Object o in objects)
        {
            if (o.Intersect(v, offset)) cpt++; ;
        }

        return (cpt >= 2);
    }

    // Diplays every spheres
    private bool Union(Vector3 v, Object[] objects, float radius, Vector3 offset)
    {
        foreach (Object o in objects)
        {
            if (o.Intersect(v, offset)) return true;
        }

        return false;
    }

    private bool IsPointInside(Vector3 v, Object[] objects, float radius, Vector3 offset)
    {
        switch (op)
        {
            case Operation.UNION:
                return Union(v, objects, radius, offset);

            case Operation.INTERSECT:
                return Intersect(v, objects, radius, offset);

            default:
                return false;
        }
    }


    public void Voxelize(Mesh m, Object[] objects)
    {
        this.m = m;

        Vector3 extents = m.bounds.extents;
        step.x = (extents.x * 2f) / xSize;
        step.y = (extents.y * 2f) / ySize;
        step.z = (extents.z * 2f) / zSize;

        Init();

        for (float x = -extents.x; x < extents.x; x+= step.x)
        {
            for (float y = -extents.y; y < extents.y; y+= step.y)
            {
                for (float z = -extents.z; z < extents.z; z+= step.z)
                {
                    // if the voxel is inside one of the spheres, keep it
                    if (IsPointInside(new Vector3(x + (step.x / 2f), y + (step.y / 2f), z + (step.z / 2f)), objects, 0.5f, m.bounds.center))
                        voxelsPos.Add(new Vector3(
                            x + (step.x / 2f) + m.bounds.center.x,
                            y + (step.y / 2f) + m.bounds.center.y,
                            z + (step.z / 2f) + m.bounds.center.z
                            ));
                }
            }
        }
    }

    private void DrawVoxels()
    {
        Transform parent = transform.Find("Voxels");

        for (int i = 0; i < voxelsPos.Count; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = voxelsPos[i];
            cube.transform.localScale = step;
            cube.GetComponent<BoxCollider>().enabled = false;
            cube.transform.parent = parent;
            cube.name = "Voxel" + i;
            cube.GetComponent<MeshRenderer>().material = voxelMaterial;
        }
    }

    public void Voxelize (List<MeshFilter> meshes, Object[] objects)
    {
        Mesh m = new Mesh();

        CombineInstance[] ci = new CombineInstance[meshes.Count];
        for (int i = 0; i < meshes.Count; i++)
        {
            ci[i].mesh = meshes[i].mesh;
            ci[i].transform = meshes[i].transform.localToWorldMatrix;
        }

        m.CombineMeshes(ci);
        //GetComponent<MeshFilter>().mesh = m; //DEBUG

        Voxelize(m, objects);

        DrawVoxels();
    }

    private void OnDrawGizmos()
    {
        if (voxelsPos == null) return;

        /*Gizmos.color = Color.green;
        for (int i = 0; i < voxelsPos.Count; i++)
        {
            Gizmos.DrawCube(voxelsPos[i], step);
        }*/

        if (m == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(m.bounds.center, m.bounds.size); //renders the bbox
    }
    
}