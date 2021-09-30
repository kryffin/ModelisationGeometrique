using UnityEngine;

public class Sphere : Object
{
    public Vector3 c;
    public float r;

    public Sphere(Vector3 c, float r)
    {
        this.c = c;
        this.r = r;
    }

    public override bool Intersect(Vector3 v, Vector3 offset)
    {
        return Mathf.Pow(v.x - (c.x - offset.x), 2f) + Mathf.Pow(v.y - (c.y - offset.y), 2f) + Mathf.Pow(v.z - (c.z - offset.z), 2f) - (r * r) < 0f;
    }
}
