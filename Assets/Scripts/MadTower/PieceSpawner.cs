using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] buildPieces;
    [SerializeField] private float spawnTime = 3f;

    private int deviceID;
    private GameObject newPiece;
    private bool canDrop;

    public void AssignDeviceID(int assignedId) { deviceID = assignedId; }

    private void Start()
    {
        SpawnNextPiece();
    }
    private void SpawnNextPiece()
    {
        int nextPiece = Random.Range(0, buildPieces.Length);

        newPiece = Instantiate(buildPieces[nextPiece], transform.position, Quaternion.identity);
        newPiece.transform.SetParent(transform);

        canDrop = true;
    }
    public void DropPiece(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.control.device.deviceId.Equals(deviceID))
        {
            if (canDrop)
            {
                //DESENPARENTAR
                newPiece.transform.SetParent(transform.parent);
                newPiece.GetComponent<Rigidbody>().isKinematic = false;

                //CAMBIAR TAG
                ChangeTag(newPiece.transform, "Untagged");

                //ACTIVAR CONGELAR RIGIDBODY
                newPiece.GetComponent<FreezePhysics>().StartFreezeTime();
                canDrop = false;


                Invoke(nameof(SpawnNextPiece), spawnTime);
            }
        }
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