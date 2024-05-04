using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] protected float headRotationSensitivity = 0.000002f;
    [SerializeField] protected float headRotationLimitX = 45.0f;
    [SerializeField] protected float headRotationLimitY = 50.0f;

    [SerializeField] protected GameObject headRef;

    protected float rotationX = 0;
    protected float rotationY = 0;

    // Start is called before the first frame update
    void Awake()
    {
        headRef = GameObject.Find("Main Camera");
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();

       if (movement.y != 0)
        {
            Debug.Log("is rotating " + rotationX);
            rotationX -= movement.y * headRotationSensitivity;
            rotationX = Mathf.Clamp(rotationX, -headRotationLimitX,
            headRotationLimitX);
            rotationY += movement.x * headRotationSensitivity;
            rotationY = Mathf.Clamp(rotationY, -headRotationLimitX,
            headRotationLimitX);

            headRef.transform.localEulerAngles = new Vector3(rotationX, rotationY);
        }
    }

}
