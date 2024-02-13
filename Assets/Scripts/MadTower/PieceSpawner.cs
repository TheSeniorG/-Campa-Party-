using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] buildPieces;
    [SerializeField] private float spawnTime = 3f;

    private GameObject newPiece;
    private bool canDrop;

    private void Start()
    {
        SpawnNextPiece();
    }
    private void Update()
    {
        if (canDrop && Input.GetKeyDown(KeyCode.Space)){StartCoroutine(DropPiece());}
    }
    private void SpawnNextPiece()
    {
        int nextPiece = Random.Range(0, buildPieces.Length);

        newPiece = Instantiate(buildPieces[nextPiece], transform.position, Quaternion.identity);
        newPiece.transform.SetParent(transform);
        newPiece.GetComponent<Rigidbody>().isKinematic = true;

        canDrop = true;
    }
    IEnumerator DropPiece()
    {
        //DESENPARENTAR
        newPiece.transform.SetParent(transform.parent);
        newPiece.GetComponent<Rigidbody>().isKinematic = false;

        //CAMBIAR TAG
        ChangeTag(newPiece.transform, "Untagged");

        //ACTIVAR CONGELAR RIGIDBODY
        newPiece.GetComponent<FreezePhysics>().StartFreezeTime();
        canDrop = false;

        yield return new WaitForSeconds(spawnTime);

        SpawnNextPiece();
    }
    void ChangeTag(Transform parent, string nuevaTag)
    {
        parent.gameObject.tag = nuevaTag;

        foreach (Transform child in parent)
        {
            ChangeTag(child, nuevaTag);
        }
    }
}