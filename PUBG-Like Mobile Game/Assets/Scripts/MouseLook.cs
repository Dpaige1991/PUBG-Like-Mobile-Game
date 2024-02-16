using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Min & Max Camera View")]
    private const float YMin = -50f;
    private const float YMax = 50f;

    [Header("Camera View")]
    public Transform lookAt;
    public Transform player;

    [Header("Camera Position")]
    public float CameraDistance = 10f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    public float cameraSensitivity = 4f;

    public FloatingJoystick floatingJoystick;

    // Update is called once per frame
    private void LateUpdate()
    {
        currentX += floatingJoystick.Horizontal * cameraSensitivity * Time.deltaTime;
        currentY -= floatingJoystick.Vertical + cameraSensitivity * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Vector3 direction = new Vector3(0, 0, -CameraDistance);

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        transform.position = lookAt.position + rotation * direction;

        transform.LookAt(lookAt.position);
    }
}
