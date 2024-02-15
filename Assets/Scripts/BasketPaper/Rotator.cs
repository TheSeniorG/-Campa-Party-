using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float minRotationAngle = -45f;
    [SerializeField] private float maxRotationAngle = 45f;
    [SerializeField] private bool rotate = false;

    private float currentRotation;
    private int rotationDirection = 1;
    void Update() { if(rotate)Rotate(); } // ROTAMOS EL OBJETO
    private void Start()
    {
        //DAR UN ANGULO INICIAL RANDOM
        currentRotation = Random.Range(minRotationAngle, maxRotationAngle);
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, currentRotation);

    }
    private void Rotate()
    {
        currentRotation += rotationSpeed * Time.deltaTime * rotationDirection;

        if (currentRotation > maxRotationAngle || currentRotation < minRotationAngle)
        {
            rotationDirection *= -1;
        } // CAMBIAMOS LA DIRECCIÓN DE ROTACIÓN CUANDO ALCANZA UN LÍMITE

        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, currentRotation);
    }
    public void ToogleRotation() { rotate = !rotate; }
}
