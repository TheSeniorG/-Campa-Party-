using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionParticles : MonoBehaviour
{
    [SerializeField] private GameObject impactParticle;
    private void OnCollisionEnter(Collision collision)
    {
        //OBTENER PUNTO DE CONTATCTO
        ContactPoint contact = collision.contacts[0];
        Vector3 contactPosition = contact.point;
        //PARTICULA DE IMPACTO
        Instantiate(impactParticle, contactPosition, Quaternion.identity);
    }
}
