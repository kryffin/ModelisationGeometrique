using UnityEngine;

public class HelloTriangle : MonoBehaviour
{

    public int nbLignes;
    public int nbColonnes;

    public Material mat;

    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite �tre visualis�
        gameObject.AddComponent<MeshRenderer>();

        Vector3[] vertices = new Vector3[4* (nbLignes * nbColonnes)];            // Cr�ation des structures de donn�es qui accueilleront sommets et  triangles
        int[] triangles = new int[6*(nbLignes*nbColonnes)];

        int cptV = 0;
        int cptT = 0;
        int offs = 0; //offset to the vertices used by each triangle
        for (int i = 0; i < nbColonnes; i++)
        {
            for (int j = 0; j < nbLignes; j++)
            {
                vertices[cptV++] = new Vector3(0 + i, 0 + j, 0);            // Remplissage de la structure sommet 
                vertices[cptV++] = new Vector3(1 + i, 0 + j, 0);
                vertices[cptV++] = new Vector3(0 + i, 1 + j, 0);
                vertices[cptV++] = new Vector3(1 + i, 1 + j, 0);

                triangles[cptT++] = 0 + offs;                               // Remplissage de la structure triangle. Les sommets sont repr�sent�s par leurs indices
                triangles[cptT++] = 1 + offs;                               // les triangles sont repr�sent�s par trois indices (et sont mis bout � bout)
                triangles[cptT++] = 2 + offs;
                triangles[cptT++] = 1 + offs;
                triangles[cptT++] = 3 + offs;
                triangles[cptT++] = 2 + offs;

                offs += 4;
            }
        }

        Mesh msh = new Mesh();                          // Cr�ation et remplissage du Mesh

        msh.vertices = vertices;
        msh.triangles = triangles;

        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du mat�riel
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }
}
