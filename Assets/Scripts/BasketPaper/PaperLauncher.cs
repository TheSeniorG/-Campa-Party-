using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaperLauncher : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private float throwForce;
    [SerializeField] private float cooldownTime;

    private int deviceID;
    private GameObject paperBall;
    private Rigidbody paperBallRB;
    private bool readyToShot;
    private float cooldown;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void AssignDeviceID(int assignedID) { deviceID = assignedID; }

    private void Update()
    {
        if (cooldown <= 0 && !readyToShot)
        {
            InstantiatePaperBall(); // INSTANCIAMOS EL OBJETO A LANZAR
        }

        if (!readyToShot)
        {
            cooldown -= Time.deltaTime;
        }
    }

    private void InstantiatePaperBall()
    {
        readyToShot = true;
        paperBall = Instantiate(ball, transform.position, Quaternion.identity);
        paperBallRB = paperBall.GetComponent<Rigidbody>();
    }

    public void LaunchPaperBall(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.control.device.deviceId.Equals(deviceID))
        {
            if (readyToShot)
            {
                paperBallRB.isKinematic = false; // DESACTIVAMOS LA PROPIEDAD KINEMÁTICA
                paperBallRB.AddForce(transform.forward * throwForce); // LANZAMOS EL OBJETO
                cooldown = cooldownTime; // RESETEAMOS EL COOLDOWN
                readyToShot = false;

                //SFX
                audioSource.Play();

                //DETRUIMOS PARA QUE NO SE ACUMULEN
                Destroy(paperBall, 3f);
            }
        }
    }
}