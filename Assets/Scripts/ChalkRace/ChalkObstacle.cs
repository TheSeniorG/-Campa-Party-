using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChalkObstacle : MonoBehaviour
{
    [SerializeField] private float movSpeed = 1;
    private float chalkDamage = 0.05f;

    private void Start(){Destroy(gameObject,7f);}
    void Update()
    {
        //SE MUEVE A LA IZQUIERDA
        transform.Translate(Vector3.left * Time.deltaTime * movSpeed, Space.World);
    }
    public float GetDamage(){return chalkDamage;}
}
