using System.Collections.Generic;
using UnityEngine;

public class BernsteinBezier : MonoBehaviour
{
    public GameObject pointPrefab;

    private LineRenderer baseLine;
    private LineRenderer baseLine2;
    private LineRenderer bezierCurve;
    private LineRenderer bezierCurve2;

    private List<GameObject> points;
    private List<GameObject> points2;
    private List<Vector3> bezierPoints;
    private List<Vector3> bezierPoints2;

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

    private Vector3 BernsteinAtT2(float t)
    {
        Vector3 p = Vector3.zero;
        int n = points2.Count - 1;

        for (int i = 0; i < points2.Count; i++)
        {
            p += points2[i].transform.position * Oskur(i, n) * Mathf.Pow(t, i) * Mathf.Pow(1f - t, n - i);
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

    private void ComputeBezier2()
    {
        bezierPoints2 = new List<Vector3>();

        float step = 1f / nbPoints;
        for (float t = 0f; t < 1f; t += step)
        {
            bezierPoints2.Add(BernsteinAtT2(t));
        }
        bezierPoints2.Add(points2[points2.Count - 1].transform.position);

        bezierCurve2.positionCount = bezierPoints2.Count;
        bezierCurve2.SetPositions(bezierPoints2.ToArray());
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

    private void CreatePoints2(List<Vector3> ps)
    {
        Transform line = transform.Find("Base2").transform;
        int i = 0;
        foreach (Vector2 v in ps)
        {
            points2.Add(Instantiate(pointPrefab, v, Quaternion.identity, line));
            points2[points2.Count - 1].name = "Point " + (i++);
        }
    }

    void Start()
    {
        baseLine = transform.Find("Base").GetComponent<LineRenderer>();
        baseLine2 = transform.Find("Base2").GetComponent<LineRenderer>();
        bezierCurve = transform.Find("Bezier").GetComponent<LineRenderer>();
        bezierCurve2 = transform.Find("Bezier2").GetComponent<LineRenderer>();

        points = new List<GameObject>();
        points2 = new List<GameObject>();
        List<Vector3> tmpPoints = new List<Vector3>();
        List<Vector3> tmpPoints2 = new List<Vector3>();
        bezierPoints = new List<Vector3>();
        bezierPoints2 = new List<Vector3>();

        tmpPoints.Add(new Vector3(0f, 0f));
        tmpPoints.Add(new Vector3(5f, 5f));
        tmpPoints.Add(new Vector3(10f, 5f));
        tmpPoints.Add(new Vector3(15f, 0f));

        tmpPoints2.Add(new Vector3(15f, 0f));
        tmpPoints2.Add(new Vector3(20f, -5f));
        tmpPoints2.Add(new Vector3(25f, -5f));
        tmpPoints2.Add(new Vector3(30f, 0f));

        CreatePoints(tmpPoints);
        CreatePoints2(tmpPoints2);

        ComputeBezier();
        ComputeBezier2();
    }

    private void FixedUpdate()
    {
        points2[1].transform.position = (points[3].transform.position - points[2].transform.position) + points[points.Count-1].transform.position;

        baseLine.positionCount = points.Count;
        int i = 0;
        foreach (GameObject g in points)
            baseLine.SetPosition(i++, g.transform.position);

        baseLine2.positionCount = points2.Count;
        i = 0;
        foreach (GameObject g in points2)
            baseLine2.SetPosition(i++, g.transform.position);

        ComputeBezier();
        ComputeBezier2();
    }

}
