using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermiteCurve : MonoBehaviour
{

    private LineRenderer hermiteLine;

    private GameObject P0, P1, V0, V1;

    public int nbPoints;
    public float factor;

    private float F1(float u)
    {
        return (2f * Mathf.Pow(u, 3f)) - (3f * Mathf.Pow(u, 2f)) + 1f;
    }

    private float F2(float u)
    {
        return -(2f * Mathf.Pow(u, 3f)) + (3f * Mathf.Pow(u, 2f));
    }

    private float F3(float u)
    {
        return Mathf.Pow(u, 3f) - (2f * Mathf.Pow(u, 2f)) + u;
    }

    private float F4(float u)
    {
        return Mathf.Pow(u, 3f) - Mathf.Pow(u, 2f);
    }

    private void TraceHermite(Vector2 P0, Vector2 P1, Vector2 V0, Vector2 V1, int nbStep)
    {
        List<Vector3> line = new List<Vector3>();

        V0 *= factor;
        V1 *= factor;

        float step = 1f / nbStep;
        for (float u = 0f; u < 1f; u += step)
        {
            Vector3 p_u = (F1(u) * P0) + (F2(u) * P1) + (F3(u) * V0) + (F4(u) * V1);
            line.Add(p_u);
        }

        line.Add(P1);

        hermiteLine.positionCount = line.Count;
        hermiteLine.SetPositions(line.ToArray());
    }

    void Start()
    {
        hermiteLine = GetComponent<LineRenderer>();

        P0 = transform.Find("P0").gameObject;
        P1 = transform.Find("P1").gameObject;
        V0 = transform.Find("P0").transform.Find("V0").gameObject;
        V1 = transform.Find("P1").transform.Find("V1").gameObject;

        TraceHermite(P0.transform.position, P1.transform.position, V0.transform.position - P0.transform.position, -(V1.transform.position - P1.transform.position), nbPoints-1);
    }

    private void FixedUpdate()
    {
        TraceHermite(P0.transform.position, P1.transform.position, V0.transform.position - P0.transform.position, -(V1.transform.position - P1.transform.position), nbPoints - 1);
    }

    private void OnDrawGizmos()
    {
        if (P0 == null || P1 == null || V0 == null || V1 == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(P0.transform.position, V0.transform.position - P0.transform.position);
        Gizmos.DrawRay(P1.transform.position, V1.transform.position - P1.transform.position);
    }

}
