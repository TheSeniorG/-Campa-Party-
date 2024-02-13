using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperLauncher : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private float throwForce;
    [SerializeField] private float cooldownTime;

    private GameObject paperBall;
    private Rigidbody paperBallRB;
    private bool readyToShot;
    private float cooldown;

    private void Update()
    {
        if (cooldown <= 0 && !readyToShot)
        {
            InstantiatePaperBall(); // INSTANCIAMOS EL OBJETO A LANZAR
        }

        if (Input.GetButtonDown("Jump") && readyToShot)
        {
            LaunchPaperBall(); // LANZAMOS EL OBJETO
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

    private void LaunchPaperBall()
    {
        paperBallRB.isKinematic = false; // DESACTIVAMOS LA PROPIEDAD KINEMÁTICA
        paperBallRB.AddForce(transform.forward * throwForce); // LANZAMOS EL OBJETO
        cooldown = cooldownTime; // RESETEAMOS EL COOLDOWN
        readyToShot = false;
        //DETRUIMOS PARA QUE NO SE ACUMULEN
        Destroy(paperBall, 3f);
    }
}