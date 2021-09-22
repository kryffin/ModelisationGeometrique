using System.Collections.Generic;

public class Face
{
    public int nbVertices;
    public List<int> vertices;

    public Face(int nbV)
    {
        nbVertices = nbV;
        vertices = new List<int>();
    }

}
