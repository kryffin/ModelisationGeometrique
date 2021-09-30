using UnityEngine;

public class HelloModelOFF : MonoBehaviour
{
    private Model model;
    public string filename;
    public Material mat;
    public bool saveOFF = false;
    public string fileToSave;

    // Start is called before the first frame update
    void Start()
    {
        OFFParser p = new OFFParser();

        model = p.Parse("Assets/" + filename + ".off");

        MeshFilter mf = GetComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        // Vertices
        Vector3[] verts = new Vector3[model.vertices.Count];
        int i = 0;
        foreach (Vertice v in model.vertices)
        {
            verts[i++] = v.pos;
        }
        mesh.vertices = verts;

        // Triangles
        int[] triangles = new int[model.faces.Count * 3];
        i = 0;
        foreach (Face f in model.faces)
        {
            for (int k = 0; k < 3; k++) triangles[i++] = f.vertices[k];
        }
        mesh.triangles = triangles;

        // Normals
        Vector3[] normals = new Vector3[model.vertices.Count];
        i = 0;
        foreach (Vertice v in model.vertices)
        {
            normals[i++] = v.norm;
        }
        mesh.normals = normals;

        //mesh.RecalculateNormals(); //computes vertices normals
        mf.mesh = mesh;
        mr.material = mat;

        GetComponent<MeshCollider>().sharedMesh = mf.mesh;

        if (saveOFF) p.Write(model, fileToSave);
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
