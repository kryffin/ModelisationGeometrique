using UnityEngine;

public class HelloCylinder : MonoBehaviour
{
    public Material mat;

    public float radius;
    public float height;
    public int nbMeridians;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite �tre visualis�
        gameObject.AddComponent<MeshRenderer>();

        Vector3 p = transform.position;

        Vector3[] vertices = new Vector3[(2 * nbMeridians)+4];            // Cr�ation des structures de donn�es qui accueilleront sommets et  triangles
        int[] triangles = new int[12 * nbMeridians];

        float dA = Mathf.Deg2Rad * (360f / nbMeridians);

        int cptV = 0;
        int cptT = 0;
        int offs = 2; //offset to reference vertices and complete each triangles

        //verttices for the top and bottom parts
        vertices[cptV++] = new Vector3(p.x, p.y - (height / 2f), p.z);
        vertices[cptV++] = new Vector3(p.x, p.y + (height / 2f), p.z);

        //first meridian
        vertices[cptV++] = new Vector3(p.x + (radius * Mathf.Cos(0)), p.y - (height / 2f), p.z + (radius * Mathf.Sin(0)));
        vertices[cptV++] = new Vector3(p.x + (radius * Mathf.Cos(0)), p.y + (height / 2f), p.z + (radius * Mathf.Sin(0)));

        for (int i = 0; i < nbMeridians; i++)
        {
            //(i+1) meridian
            vertices[cptV++] = new Vector3(p.x + (radius * Mathf.Cos(dA * (i+1))), p.y - (height / 2f), p.z + (radius * Mathf.Sin(dA * (i + 1))));
            vertices[cptV++] = new Vector3(p.x + (radius * Mathf.Cos(dA * (i+1))), p.y + (height/2f), p.z + (radius * Mathf.Sin(dA * (i + 1))));

            //merging i and i+1 meridians into 2 triangles (quad)
            triangles[cptT++] = 0 + offs;
            triangles[cptT++] = 1 + offs;
            triangles[cptT++] = 2 + offs;
            triangles[cptT++] = 1 + offs;
            triangles[cptT++] = 3 + offs;
            triangles[cptT++] = 2 + offs;

            //top triangle
            triangles[cptT++] = 0 + offs;
            triangles[cptT++] = 2 + offs;
            triangles[cptT++] = 0;

            //bottom triangle
            triangles[cptT++] = 1 + offs;
            triangles[cptT++] = 1;
            triangles[cptT++] = 3 + offs;

            offs += 2;
        }

        Mesh msh = new Mesh();                          // Cr�ation et remplissage du Mesh

        msh.vertices = vertices;
        msh.triangles = triangles;
        
        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du mat�riel
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }
}
