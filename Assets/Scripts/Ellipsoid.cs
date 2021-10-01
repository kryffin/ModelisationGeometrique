using UnityEngine;

public class Ellipsoid : Object
{
    public Vector3 center;
    public float a, b, c;

    public Ellipsoid(Vector3 center, Vector3 abc)
    {
        this.center = center;
        this.a = abc.x;
        this.b = abc.y;
        this.c = abc.z;
    }

    public override bool Intersect(Vector3 v, Vector3 offset)
    {
        return Mathf.Pow(v.x - (center.x - offset.x), 2f) / Mathf.Pow(a, 2f)
            + Mathf.Pow(v.y - (center.y - offset.y), 2f) / Mathf.Pow(b, 2f)
            + Mathf.Pow(v.z - (center.z - offset.z), 2f) / Mathf.Pow(c, 2f) < 1f;
    }
}
