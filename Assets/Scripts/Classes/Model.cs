using System.Collections.Generic;
using UnityEngine;

public class Model
{
    public int nbV;
    public int nbF;
    public int nbE;
    public List<Vertice> vertices;
    public List<Face> faces;
    public List<Vector3> normals;

    public Model()
    {
        nbV = 0;
        nbF = 0;
        nbE = 0;
        vertices = new List<Vertice>();
        faces = new List<Face>();
        normals = new List<Vector3>();
    }
}
