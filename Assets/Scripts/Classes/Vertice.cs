using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertice
{
    public Vector3 pos;
    public Vector3 norm;

    public Vertice(float x, float y, float z)
    {
        pos = new Vector3(x, y, z);
        norm = Vector3.zero;
    }
}
