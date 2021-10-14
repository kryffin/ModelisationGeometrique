using System.Collections.Generic;
using UnityEngine;

public class BernsteinBezier : MonoBehaviour
{
    public GameObject pointPrefab;

    private LineRenderer baseLine;
    private LineRenderer bezierCurve;

    private List<GameObject> points;
    private List<Vector3> bezierPoints;

    public int nbPoints;

    private int Fact(int n)
    {
        if (n <= 0) return 1;
        if (n == 1 || n == 2) return n;
        return n * Fact(n - 1);
    }

    private float Oskur(int k, int n)
    {
        return Fact(n) / (Fact(k) * Fact(n - k));
    }

    private Vector3 BernsteinAtT(float t)
    {
        Vector3 p = Vector3.zero;
        int n = points.Count - 1;

        for (int i = 0; i < points.Count; i++)
        {
            p += points[i].transform.position * Oskur(i, n) * Mathf.Pow(t, i) * Mathf.Pow(1f - t, n - i);
        }

        return p;
    }

    private void ComputeBezier()
    {
        bezierPoints = new List<Vector3>();

        float step = 1f / nbPoints;
        for (float t = 0f; t < 1f; t += step)
        {
            bezierPoints.Add(BernsteinAtT(t));
        }
        bezierPoints.Add(points[points.Count-1].transform.position);

        bezierCurve.positionCount = bezierPoints.Count;
        bezierCurve.SetPositions(bezierPoints.ToArray());
    }

    private void CreatePoints(List<Vector3> ps)
    {
        Transform line = transform.Find("Base").transform;
        int i = 0;
        foreach (Vector2 v in ps)
        {
            points.Add(Instantiate(pointPrefab, v, Quaternion.identity, line));
            points[points.Count - 1].name = "Point " + (i++);
        }
    }

    void Start()
    {
        baseLine = transform.Find("Base").GetComponent<LineRenderer>();
        bezierCurve = transform.Find("Bezier").GetComponent<LineRenderer>();

        points = new List<GameObject>();
        List<Vector3> tmpPoints = new List<Vector3>();
        bezierPoints = new List<Vector3>();

        tmpPoints.Add(new Vector3(0f, 0f));
        tmpPoints.Add(new Vector3(5f, 5f));
        tmpPoints.Add(new Vector3(10f, 5f));
        tmpPoints.Add(new Vector3(15f, 0f));

        CreatePoints(tmpPoints);

        ComputeBezier();
    }

    private void FixedUpdate()
    {
        baseLine.positionCount = points.Count;
        int i = 0;
        foreach (GameObject g in points)
            baseLine.SetPosition(i++, g.transform.position);

        ComputeBezier();
    }

}
