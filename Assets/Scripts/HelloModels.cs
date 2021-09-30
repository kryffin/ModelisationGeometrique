using System.Collections.Generic;
using UnityEngine;

public class HelloModels : MonoBehaviour
{
    public bool disableMeshes = true;

    // Start is called before the first frame update
    void Start()
    {
        List<MeshFilter> meshes = new List<MeshFilter>();
        List<Object> objects = new List<Object>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeInHierarchy) continue; //removes spheres disabled in hierarchy

            if (transform.GetChild(i).name.Contains("Sphere"))
            {
                transform.GetChild(i).GetComponent<MeshFilter>().gameObject.SetActive(!disableMeshes);
                meshes.Add(transform.GetChild(i).GetComponent<MeshFilter>());
                objects.Add(new Sphere(transform.GetChild(i).transform.localPosition, transform.GetChild(i).transform.localScale.x / 2f));
            }
            else if (transform.GetChild(i).name.Contains("Cube"))
            {
                transform.GetChild(i).GetComponent<MeshFilter>().gameObject.SetActive(!disableMeshes);
                meshes.Add(transform.GetChild(i).GetComponent<MeshFilter>());
                objects.Add(new Cube(transform.GetChild(i).transform.localPosition, transform.GetChild(i).transform.localScale.x / 2f));
            }
        }

        GetComponent<Voxelizer>().Voxelize(meshes, objects.ToArray());
    }

    /*private void FixedUpdate()
    {
        foreach (Vertice v in model.vertices)
        {
            Debug.DrawRay(v.pos + transform.position,
                v.norm / 100f, Color.red);
        }
    }*/

}
