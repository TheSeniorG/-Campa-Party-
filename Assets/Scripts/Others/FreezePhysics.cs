using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class FreezePhysics : MonoBehaviour
{
    [HideIf("freezeOnTime")][SerializeField] private bool freezeOnContact;
    [HideIf("freezeOnContact")][SerializeField] private bool freezeOnTime;
    [ShowIf("freezeOnTime")][SerializeField] private float freezeTime;

    private Rigidbody rb;
    private void Start(){rb = GetComponent<Rigidbody>();}
    public void StartFreezeTime(){Invoke("FreezeRB", freezeTime);}
    private void OnCollisionEnter(Collision collision){if (freezeOnContact){FreezeRB();}}
    void FreezeRB(){rb.isKinematic = true;}
}
