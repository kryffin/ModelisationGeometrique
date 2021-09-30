using UnityEngine;

public class Cube : Object
{
    public Vector3 c;
    public float s;
    public Cube(Vector3 c, float s)
    {
        this.c = c;
        this.s = s;
    }

    public override bool Intersect(Vector3 v, Vector3 offset)
    {
        return v.x <= (c.x - offset.x) + s && v.x >= (c.x - offset.x) - s
            && v.y <= (c.y - offset.y) + s && v.y >= (c.y - offset.y) - s
            && v.z <= (c.z - offset.z) + s && v.z >= (c.z - offset.z) - s;
    }
}
