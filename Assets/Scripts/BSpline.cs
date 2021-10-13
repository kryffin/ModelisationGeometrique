using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSpline : MonoBehaviour
{

    private LineRenderer baseLine;
    private LineRenderer smoothLine;

    private List<Vector3> line;
    private List<Vector3> smooth;

    public int nbIterations;

    // Smoothing a line using Chaikin algorithm
    private void SmoothLine()
    {
        for (int i = 0; i < nbIterations; i++)
        {
            List<Vector3> tmpLine = new List<Vector3>();
            tmpLine.Add(smooth[0]);

            for (int j = 0; j < smooth.Count-1; j++)
            {
                tmpLine.Add((0.75f * smooth[j]) + (0.25f * smooth[j+1]));
                tmpLine.Add((0.25f * smooth[j]) + (0.75f * smooth[j+1]));
            }

            tmpLine.Add(smooth[smooth.Count-1]);

            smooth = tmpLine;
        }
    }

    void Start()
    {
        baseLine = transform.Find("Base").GetComponent<LineRenderer>();
        smoothLine = transform.Find("Smoothed").GetComponent<LineRenderer>();

        line = new List<Vector3>(4);
        smooth = new List<Vector3>();

        //Constructs the base line
        line.Add(new Vector3(0f, 0f));
        line.Add(new Vector3(5f, 5f));
        line.Add(new Vector3(10f, 5f));
        line.Add(new Vector3(15f, 0f));
        line.Add(new Vector3(22f, 3f));
        line.Add(new Vector3(-5f, 10f));
        line.Add(new Vector3(-26f, -3f));
        line.Add(new Vector3(8f, 10f));

        smooth = line;

        baseLine.positionCount = line.Count;
        baseLine.SetPositions(line.ToArray());

        SmoothLine();

        smoothLine.positionCount = smooth.Count;
        smoothLine.SetPositions(smooth.ToArray());
    }

    void Update()
    {
        
    }
}
