using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class OFFParser
{
    public Vector3 com;

    private Vector3 CenterOfModel(Model m)
    {
        Vector3 sum = new Vector3();
        int count = 0;
        foreach(Vertice v in m.vertices)
        {
            sum += v.pos;
            count++;
        }

        com = sum / count;
        return sum / count;
    }

    private void RecenterModel(Model m, Vector3 center)
    {
        for (int i = 0; i < m.vertices.Count; i++)
        {
            m.vertices[i].pos -= com;
        }
    }

    private float HighestCoordinates(Model m)
    {
        float highest = float.MinValue;
        foreach (Vertice v in m.vertices)
        {
            if (Mathf.Abs(v.pos.magnitude) > highest) highest = Mathf.Abs(v.pos.magnitude);
        }
        return highest;
    }

    private void NormalizeModelSize(Model m, float highest)
    {
        for (int i = 0; i < m.vertices.Count; i++)
        {
            m.vertices[i].pos /= highest;
        }
    }

    private List<Vector3> ComputeNormals(Model m)
    {
        List<Vector3> normals = new List<Vector3>();
        foreach (Face f in m.faces)
        {
            normals.Add(Vector3.Cross(m.vertices[f.vertices[1]].pos - m.vertices[f.vertices[0]].pos, m.vertices[f.vertices[2]].pos - m.vertices[f.vertices[0]].pos).normalized);
        }
        return normals;
    }

    public Model Parse (string filename)
    {
        bool header = false;
        int nbV = 0, nbF = 0, nbE = 0, cptV = 0, cptF = 0;
        List<Vertice> vertices = new List<Vertice>();
        List<Face> faces = new List<Face>();

        string[] lines = System.IO.File.ReadAllLines(filename);
        foreach (string line in lines)
        {
            if (line.StartsWith("#") || line.StartsWith("OFF") || line.Equals("")) continue;
            string[] data = line.Split(' ');
            if (!header)
            {
                nbV = int.Parse(data[0]);
                nbF = int.Parse(data[1]);
                nbE = int.Parse(data[2]);
                header = true;
                continue;
            }
     
            if (cptV < nbV)
            {
                vertices.Add(new Vertice(float.Parse(data[0].ToString(), CultureInfo.InvariantCulture), float.Parse(data[1].ToString(), CultureInfo.InvariantCulture), float.Parse(data[2].ToString(), CultureInfo.InvariantCulture)));
                cptV++;
            }
            else if (cptF < nbF)
            {
                Face f = new Face(int.Parse(data[0].ToString()));
                for (int i = 1; i <= f.nbVertices; i++)
                {
                    f.vertices.Add(int.Parse(data[i].ToString()));
                }
                faces.Add(f);
                cptF++;
            }
        }

        Model m = new Model();
        m.nbV = nbV;
        m.nbF = nbF;
        m.nbE = nbE;
        m.vertices = vertices;
        m.faces = faces;
        m.normals = ComputeNormals(m);
        RecenterModel(m, CenterOfModel(m));
        NormalizeModelSize(m, HighestCoordinates(m));

        return m;
    }

    public void Write (Model m, string name)
    {
        string[] lines = new string[m.nbV + m.nbF + 2];
        int i = 0;

        lines[i++] = "OFF";
        lines[i++] = m.nbV + " " + m.nbF + " " + m.nbE;
        foreach (Vertice v in m.vertices)
        {
            lines[i++] = (v.pos.x.ToString() + " " + v.pos.y.ToString() + " " + v.pos.z.ToString()).Replace(",", ".");
        }
        foreach (Face f in m.faces)
        {
            string line = " ";
            for (int j = 0; j < f.nbVertices; j++)
            {
                line += f.vertices[j] + " ";
            }
            lines[i++] = f.nbVertices + line;
        }

        System.IO.File.WriteAllLines("Assets/" + name + ".off", lines, System.Text.Encoding.UTF8);
    }
}
