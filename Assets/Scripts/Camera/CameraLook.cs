using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 50f;
    [SerializeField] private Slider sensSlider;
    [SerializeField] private float minSens = 10f;
    [SerializeField] private float maxSens = 100f;

    [SerializeField] private Transform playerBody;

    private float xRotation = 0f;

    private void Start()
    {
        sensSlider.minValue = minSens;
        sensSlider.maxValue = maxSens;

        sensSlider.value = mouseSensitivity;
    }

    private void Update()
    {
        Rotation();

        mouseSensitivity = sensSlider.value;
    }

    private void Rotation()
    {
        if (!FindObjectOfType<PauseMenu>().gameStarted) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        playerBody.Rotate(Vector3.up * mouseX * 1.2f);
    }
}
