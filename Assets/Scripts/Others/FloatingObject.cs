using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float floatSpeed = 1.0f; // Velocidad de flotaci�n
    public float floatAmplitude = 0.1f; // Amplitud de flotaci�n

    private Vector3 initialPosition;
    private Transform parentTransform;

    void Start()
    {
        // Almacenamos la posici�n inicial del objeto hijo
        initialPosition = transform.localPosition;

        // Obtenemos el transform del objeto padre
        parentTransform = transform.parent;
    }

    void Update()
    {
        // Calculamos las posiciones de flotaci�n usando el tiempo
        float newX = initialPosition.x + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // Actualizamos la posici�n local del objeto hijo
        transform.localPosition = new Vector3(newX, newY, 0);

        // Aseguramos que el objeto hijo siga la posici�n del objeto padre
        transform.position = parentTransform.position + transform.localPosition;
    }
}
