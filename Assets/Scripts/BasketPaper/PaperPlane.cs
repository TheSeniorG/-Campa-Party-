using UnityEngine;

public class PaperPlane : MonoBehaviour
{
    [SerializeField] private float movSpeed = 5f;
    [SerializeField] private float rotationLimit = 10f;
    [SerializeField] private float rotationTime = 1f;
    [SerializeField] private float rotationSpeed = 2f;

    private float lastRotation;
    private Rigidbody rb;
    private Quaternion randomRotation;
    private bool isFlying = true;

    private void Start()
    {
        lastRotation = Time.time;
        rb = GetComponent<Rigidbody>();

        // INVOCAR EL MÉTODO GenerateRandomRotation REPETIDAMENTE
        InvokeRepeating(nameof(GenerateRandomRotation), 0, rotationTime);
        Destroy(gameObject,10f);
    }

    private void Update()
    {
        if (isFlying)
        {

            // MOVIMIENTO HACIA DELANTE CONSTANTE
            transform.Translate(Vector3.forward * (movSpeed * Time.deltaTime));
            // ROTACIÓN HACIA LA ROTACIÓN ALEATORIA
            transform.rotation = Quaternion.RotateTowards(transform.rotation, randomRotation, rotationSpeed * Time.deltaTime);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // AL CHOCAR, ACTIVAR LA GRAVEDAD Y DESTRUIR EL OBJETO DESPUÉS DE 3 SEGUNDOS
        rb.useGravity = true;
        isFlying = false;
        Destroy(gameObject, 3f);
    }

    private void GenerateRandomRotation()
    {
        // GENERAR PEQUEÑAS DESVIACIONES DE LA ROTACIÓN ACTUAL DENTRO DE LOS LÍMITES ESTABLECIDOS
        float randomX = Random.Range(-rotationLimit, rotationLimit);
        //float randomY = Random.Range(-rotationLimit, rotationLimit);
        float randomZ = Random.Range(-rotationLimit, rotationLimit);

        // CALCULAR LOS NUEVOS ÁNGULOS SUMANDO LAS DESVIACIONES A LOS ÁNGULOS ACTUALES DE ROTACIÓN
        float newRotationX = transform.rotation.eulerAngles.x + randomX;
        float newRotationY = transform.rotation.eulerAngles.y;
        float newRotationZ = transform.rotation.eulerAngles.z + randomZ;

        // CREAR UNA ROTACIÓN QUATERNION BASADA EN LOS NUEVOS ÁNGULOS
        randomRotation = Quaternion.Euler(newRotationX, newRotationY, newRotationZ);

        //Debug.Log(randomRotation);
    }

    public void SetSpeed(float newSpeed)
    {
        movSpeed = newSpeed;
    }
}