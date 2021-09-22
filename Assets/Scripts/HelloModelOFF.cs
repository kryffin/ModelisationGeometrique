using UnityEngine;

public class HelloModelOFF : MonoBehaviour
{
    private Model model;
    public string filename;
    public bool saveOFF = false;
    public string fileToSave;

    // Start is called before the first frame update
    void Start()
    {
        OFFParser p = new OFFParser();

        model = p.Parse("Assets/" + filename + ".off");

        if (saveOFF) p.Write(model, fileToSave);
    }

    private void FixedUpdate()
    {
        int i = 0;
        foreach (Face f in model.faces)
        {
            Debug.DrawLine(model.vertices[f.vertices[0]].pos, model.vertices[f.vertices[1]].pos, Color.black);
            Debug.DrawLine(model.vertices[f.vertices[1]].pos, model.vertices[f.vertices[2]].pos, Color.black);
            Debug.DrawLine(model.vertices[f.vertices[2]].pos, model.vertices[f.vertices[0]].pos, Color.black);
            Debug.DrawRay((model.vertices[f.vertices[0]].pos + model.vertices[f.vertices[1]].pos + model.vertices[f.vertices[2]].pos) / 3f,
                model.normals[i] / 100f, Color.red);

            i++;
        }
    }

}
