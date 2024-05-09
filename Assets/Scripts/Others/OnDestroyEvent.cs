using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyEvent : MonoBehaviour
{
    [SerializeField] private GameObject particle;
    private bool isQuitting;

    //CUIDADO, AL CERRAR LA APLICACION, EL OBJETO SE DESTRUYE POR LO TANTO SE EJECUTA CÓDIGO MIENTRAS SE CIERRA.
    //A UNITY NO LE GUSTA ASI QUE HAY QUE VERIFICAR SI SE CIERRA LA APLICACION ANTES DE HACER EL CODIGO DE ON DESTROY
    void OnApplicationQuit(){isQuitting = true;}
    private void OnDestroy()
    {
        if (!isQuitting){Instantiate(particle, transform.position, Quaternion.identity);}
    }
}
