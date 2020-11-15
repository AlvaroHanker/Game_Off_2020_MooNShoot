using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    public CharacterController charController;
    public float speed = 10f;
    public Camera camera;
    public float mouseSensitivity = 500f;
    private float xRotation = 0f;
    private float yRotation = 0f;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        //float hm = InputManager.HorizontalMovement();
        //if (hm != 0.0f)
        //    Debug.Log("Horizontal Movement: " + hm);
        //float vm = InputManager.VerticalMovement();
        //if (vm != 0.0f)
        //    Debug.Log("Vertical Movement: " + vm);
        //float hr = InputManager.HorizontalRotation();
        //if (hr > -1.0)
        //    Debug.Log("Horizontal Rotation: " + hr);
        //float vr = InputManager.VerticalRotation();
        //if (vr > -1.0)
        //    Debug.Log("Vertical Rotation: " + vr);
        if (InputManager.SprintButton())
            Debug.Log("Sprint Button (SHIFT / L1)");
        if (InputManager.InteractionButton())
            Debug.Log("Interaction Button (F / cuadrado)");
        if (InputManager.MeleeWeaponButton())
            Debug.Log("Melee Weapon (E / X)");
        if (InputManager.TrapButton())
            Debug.Log("Trap Button (Q / circulo)");
        if (InputManager.AimGunButton())
            Debug.Log("Aim Gun Button (right click / L2)");
        if (InputManager.ShootGunButton())
            Debug.Log("Shoot Gun Button (left click / R2)");

        MovePlayer();
        RotateCamera();
    }

    public void MovePlayer()
    {
        //Movement
        float x = InputManager.HorizontalMovement();
        float z = InputManager.VerticalMovement();

        Vector3 move = transform.right * x + transform.forward * z;
        //Vector3 direction = InputManager.MovementDirection();
        //transform.forward += direction;

        charController.Move(move * speed * Time.deltaTime);

    }

    public void RotateCamera()
    {
        //Camera
        float mouseX = InputManager.HorizontalRotation() * mouseSensitivity * Time.deltaTime;
        float mouseY = InputManager.VerticalRotation() * mouseSensitivity * Time.deltaTime;

        xRotation = mouseY;
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);
        yRotation += mouseX;
        //yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        //camera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        //camera.transform.Rotate(Vector3.up * mouseX);
        camera.transform.Rotate(Vector3.right * xRotation);
    }
}
