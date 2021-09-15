using UnityEngine;
using UnityEngine.InputSystem;

public class CylindreController : MonoBehaviour
{

    private CharacterController cc;
    private Vector3 mvt;

    public InputAction mvtInput;
    public InputAction rotG;
    public InputAction rotD;
    public float speed = 2f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        mvt = mvtInput.ReadValue<Vector2>();
        mvt.z = mvt.y;
        mvt.y = 0f;

        if (rotG.ReadValue<float>() == 1f)
            transform.Rotate(new Vector3(0f, -1.5f, 0f));

        if (rotD.ReadValue<float>() == 1f)
            transform.Rotate(new Vector3(0f, 1.5f, 0f));
    }

    private void FixedUpdate()
    {
        cc.Move(mvt.normalized * speed);
    }

    private void OnEnable()
    {
        mvtInput.Enable();
        rotG.Enable();
        rotD.Enable();
    }

    private void OnDisable()
    {
        mvtInput.Disable();
        rotG.Disable();
        rotD.Disable();
    }

}
