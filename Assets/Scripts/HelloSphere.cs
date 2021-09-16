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

    [SerializeField]
    private int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();

        Vector3 p = transform.position;

        //(nbPara-2) because the first and last parallels are being dealt with separately (which is the +2 at the end)
        // -> the first and last parallels are only one point each
        vertices = new Vector3[((nbPara-2) * nbMeridians) + 2];

        //(nbMeridians * 6) being the first and last row, ((nbMeridians * (nbPara-3)) * 6) being the in-between quads
        triangles = new int[(nbMeridians * 6) + ((nbMeridians * (nbPara-3)) * 6)];

        float phi = Mathf.Deg2Rad * (180f / (nbPara-1)); //x axis angle
        float theta = Mathf.Deg2Rad * (360f / nbMeridians); //y axis angle

        int cptV = 0;                                                                                                                                                                                                                                                                   
        int cptT = 0;
        int offs = 2; //offset to reference vertices and complete each triangles

        //vertices for the top and bottom parts
        vertices[cptV++] = new Vector3(p.x, p.y + radius, p.z);
        vertices[cptV++] = new Vector3(p.x, p.y - radius, p.z);

        for (int j = 0; j < nbPara-1; j++) 
        {
            for (int i = 0; i < nbMeridians; i++)
            {
                if (j != 0)
                    vertices[cptV++] = new Vector3(p.x + (radius * Mathf.Sin(phi*j) * Mathf.Cos(theta*i)),
                        p.y + (radius * Mathf.Cos(phi*j)),
                        p.z + (radius * Mathf.Sin(phi*j) * Mathf.Sin(theta*i)));

                if (j == 0)
                {
                    //first row (top triangles)
                    triangles[cptT++] = 0 + offs;
                    triangles[cptT++] = 0;
                    if (i == nbMeridians - 1)
                        triangles[cptT++] = 2;
                    else
                        triangles[cptT++] = 1 + offs;
                }
                else if (j == nbPara-2)
                {
                    //last  row (bottom triangles)
                    //forget about using the offset at this point :/, due to it being the bottom left point of each quad, it's now completely offsetted ;)
                    triangles[cptT++] = 1;
                    triangles[cptT++] = vertices.Length - i - 1;
                    if (i == 0)
                        triangles[cptT++] = vertices.Length - nbMeridians;
                    else
                        triangles[cptT++] = vertices.Length - i;
                }
                else
                {
                    //iterating through each quads outside of the first and last rows
                    //ie. offset is being the bottom left point of each quad/face

                    // bottom left triangle |\
                    triangles[cptT++] = offs;
                    triangles[cptT++] = offs - nbMeridians;
                    if (i == nbMeridians-1)
                        triangles[cptT++] = offs - nbMeridians + 1;
                    else
                        triangles[cptT++] = 1 + offs;

                    // top right triangle \|
                    triangles[cptT++] = offs - nbMeridians;
                    if (i == nbMeridians - 1)
                        triangles[cptT++] = offs - (2*nbMeridians) + 1;
                    else
                        triangles[cptT++] = offs - nbMeridians + 1;
                    if (i == nbMeridians - 1)
                        triangles[cptT++] = offs - nbMeridians + 1;
                    else
                        triangles[cptT++] = offs + 1;
                }

                offs++;
            }
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
