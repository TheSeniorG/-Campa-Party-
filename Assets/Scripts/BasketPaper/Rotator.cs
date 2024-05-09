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
        // DAMOS UN �NGULO INICIAL ALEATORIO DENTRO DEL RANGO DEFINIDO
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
        // CALCULAMOS LA NUEVA ROTACI�N
        currentRotation += rotationSpeed * Time.deltaTime * rotationDirection;

        // VERIFICAMOS SI ALCANZAMOS LOS L�MITES Y CAMBIAMOS LA DIRECCI�N DE ROTACI�N SI ES NECESARIO
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

        // APLICAMOS LA ROTACI�N AL OBJETO
        transform.localRotation = Quaternion.Euler(0f, 0f, currentRotation);
    }

    public void ToggleRotation()
    {
        // CAMBIAMOS EL ESTADO DE LA ROTACI�N
        rotate = !rotate;
    }
}