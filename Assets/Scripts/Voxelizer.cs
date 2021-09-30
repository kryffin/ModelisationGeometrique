using System.Collections.Generic;
using UnityEngine;

public class Voxelizer : MonoBehaviour
{

    public int xSize, ySize, zSize;

    private List<Vector3> voxelsPos;

    private Vector3 step;

    private Mesh m;

    public Material voxelMaterial;

    private Transform voxels;

    public enum Operation
    {
        UNION, INTERSECT, SUBSTRACT
    };
    public Operation op;

    private void Start()
    {
        voxels = transform.Find("Voxels");
    }

    private void Init()
    {
        voxelsPos = new List<Vector3>();
    }

    // Only substracts Object1 - Object2
    private bool Substract(Vector3 v, Object[] objects, float radius, Vector3 offset)
    {
        if (objects.Length < 2)
        {
            Debug.LogWarning("Substract needs at least 2 objects !\nSwitching to UNION mode.");
            op = Operation.UNION;
            //might forget to cycle back on the first cell ?
            return false;
        }

        return objects[0].Intersect(v, offset) && !objects[1].Intersect(v, offset);
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

            case Operation.SUBSTRACT:
                return Substract(v, objects, radius, offset);

            default:
                return false;
        }
    }

    private void CleanVoxels()
    {
        foreach (Transform child in transform.Find("Voxels"))
        {
            Destroy(child.gameObject);
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(r, out hit, 100f))
            {
                if (hit.transform != null)
                {
                    int voxel = int.Parse(hit.transform.name.Replace("Voxel", ""));
                    voxelsPos.RemoveAt(voxel);
                    CleanVoxels();
                    DrawVoxels();
                }
            }
        }
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
