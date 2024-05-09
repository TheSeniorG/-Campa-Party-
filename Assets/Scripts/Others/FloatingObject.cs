using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float floatSpeed = 1.0f; // Velocidad de flotación
    public float floatAmplitude = 0.1f; // Amplitud de flotación

    private Vector3 initialPosition;
    private Transform parentTransform;

    void Start()
    {
        // Almacenamos la posición inicial del objeto hijo
        initialPosition = transform.localPosition;

        // Obtenemos el transform del objeto padre
        parentTransform = transform.parent;
    }

    void Update()
    {
        // Calculamos las posiciones de flotación usando el tiempo
        float newX = initialPosition.x + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // Actualizamos la posición local del objeto hijo
        transform.localPosition = new Vector3(newX, newY, 0);

        // Aseguramos que el objeto hijo siga la posición del objeto padre
        transform.position = parentTransform.position + transform.localPosition;
    }
}
