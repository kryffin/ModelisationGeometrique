using UnityEngine;
using UnityEditor;

public class HelloSphere : MonoBehaviour
{
    public Material mat;

    public float radius;
    public int nbPara;
    public int nbMeridians;

    [SerializeField]
    private Vector3[] vertices;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();

        Vector3 p = transform.position;

        vertices = new Vector3[((nbPara-2) * nbMeridians) + 2];
        //Vector3[] vertices = new Vector3[(nbPara * nbMeridians)+2];
        int[] triangles = new int[(nbPara + 1) * 6];

        float phi = Mathf.Deg2Rad * (180f / (nbPara-1)); //x
        float theta = Mathf.Deg2Rad * (360f / nbMeridians); //y

        int cptV = 0;                                                                                                                                                                                                                                                                   
        int cptT = 0;
        int offs = 2; //offset to reference vertices and complete each triangles

        //vertices for the top and bottom parts
        vertices[cptV++] = new Vector3(p.x, p.y - radius, p.z);
        vertices[cptV++] = new Vector3(p.x, p.y + radius, p.z);

        for (int j = 1; j <= nbPara; j++) 
        {
            for (int i = 0; i < nbMeridians; i++)
            {
                vertices[cptV++] = new Vector3(p.x + (radius * Mathf.Sin(phi*j) * Mathf.Cos(theta*i)),
                    p.y + (radius * Mathf.Cos(phi*j)),
                    p.z + (radius * Mathf.Sin(phi*j) * Mathf.Sin(theta*i)));

                //merging i and i+1 meridians into 2 triangles (quad)
                /*triangles[cptT++] = 0 + offs;
                triangles[cptT++] = 1 + offs;
                triangles[cptT++] = 2 + offs;
                triangles[cptT++] = 1 + offs;
                triangles[cptT++] = 3 + offs;
                triangles[cptT++] = 2 + offs;*/

                offs++;
            }

            //top triangle
            triangles[cptT++] = 0 + offs;
            triangles[cptT++] = 1;
            triangles[cptT++] = (0 + offs + (nbPara-1)) % ((nbPara * nbMeridians) + 2);

            //bottom triangle
            /*triangles[cptT++] = 1 + offs;
            triangles[cptT++] = 1;
            triangles[cptT++] = 3 + offs;*/
        }

        Mesh msh = new Mesh();

        msh.vertices = vertices;
        msh.triangles = triangles;

        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (vertices != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(transform.position + vertices[i], 0.01f);
                Handles.Label(transform.position + vertices[i] - Vector3.up * .01f, i.ToString());
            }
        }
    }
#endif
}
