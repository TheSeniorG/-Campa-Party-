using UnityEngine;

public class WinDetection : MonoBehaviour
{
    [SerializeField] private GameObject playerController;
    [SerializeField] private int playerID;

    private GameObject playerObject;
    private MadTowerManager madTowerManager;
    private float timeInsideCollider = 0f;
    private bool isInside = false;

    private void Start()
    {
        madTowerManager = GameObject.Find("MadTowerManager").GetComponent<MadTowerManager>();
    }

    private void Update()
    {
        if (isInside)
        {
            timeInsideCollider += Time.deltaTime;

            //SI ESTA MAS DE 2 SEGUNDOS EN LA META, FINALIZA
            if(timeInsideCollider > 2) { EndGame();}
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ignore"))
        {
            playerObject = other.gameObject;
            isInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Ignore"))
        {
            isInside = false;
            timeInsideCollider = 0f;
        }
    }
    private void EndGame()
    {
        //LE DECIMOS AL GAME MANAGER QUE ESTE JUGADOR HA ACABADO
        madTowerManager.EndGame(playerID);

        //DESTRUIMOS ESTE JUGADOR
        Destroy(playerController);
        Destroy(this);
    }
}