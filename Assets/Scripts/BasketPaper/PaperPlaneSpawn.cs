using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlaneSpawn : MonoBehaviour
{
    [SerializeField] private GameObject paperPlane;
    [SerializeField] private float spawnTime;

    void Start()
    {
        SpawnPaperPlane();
    }

    private void SpawnPaperPlane()
    {
        GameObject newPlane = Instantiate(paperPlane,transform.position,transform.rotation);
        newPlane.GetComponent<PaperPlane>().SetSpeed(Random.Range(1f,3f));

        //GENERAMOS OTRO TIEMPO ALEATORIO DE SPAWN
        Invoke(nameof(SpawnPaperPlane), Random.Range(1f, spawnTime));
    }
}
