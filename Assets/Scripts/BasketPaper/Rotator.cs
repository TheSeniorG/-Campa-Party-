using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float minRotationAngle = -45f;
    [SerializeField] private float maxRotationAngle = 45f;
    [SerializeField] private bool rotate = false;

    private float currentRotation;
    private int rotationDirection = 1;

    private void Start()
    {
        // DAMOS UN ÁNGULO INICIAL ALEATORIO DENTRO DEL RANGO DEFINIDO
        currentRotation = Random.Range(minRotationAngle, maxRotationAngle);
        transform.localRotation = Quaternion.Euler(0f, 0f, currentRotation);
    }

    private void Update()
    {
        // ROTAMOS EL OBJETO SOLO SI LA VARIABLE ROTATE ES TRUE
        if (rotate)
        {
            Rotate();
        }
    }

    private void Rotate()
    {
        // CALCULAMOS LA NUEVA ROTACIÓN
        currentRotation += rotationSpeed * Time.deltaTime * rotationDirection;

        // VERIFICAMOS SI ALCANZAMOS LOS LÍMITES Y CAMBIAMOS LA DIRECCIÓN DE ROTACIÓN SI ES NECESARIO
        if (currentRotation > maxRotationAngle)
        {
            currentRotation = maxRotationAngle;
            rotationDirection = -1;
        }
        else if (currentRotation < minRotationAngle)
        {
            currentRotation = minRotationAngle;
            rotationDirection = 1;
        }

        // APLICAMOS LA ROTACIÓN AL OBJETO
        transform.localRotation = Quaternion.Euler(0f, 0f, currentRotation);
    }

    public void ToggleRotation()
    {
        // CAMBIAMOS EL ESTADO DE LA ROTACIÓN
        rotate = !rotate;
    }
}