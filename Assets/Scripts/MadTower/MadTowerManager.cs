using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadTowerManager : MonoBehaviour
{
    [SerializeField] private GameObject fade, winFlash;
    [SerializeField] private GameObject[] playersCards, playerArea;
    [SerializeField] private WinDetection[] winDetectors;
    [SerializeField] private PieceSpawner[] playerController;
    [SerializeField] private MoveBetweenPoints[] movBtwnPointsScript;

    private PlayerManager playerManager;

    [Header("TESTING ONLY")]
    [SerializeField][Range(1,4)] private int playerAmount = 1;

    private void Start()
    {

        //COMPROBAR QUE EXISTA PLAYER MANAGER
        if (GameObject.Find("PlayerManager"))
        {
            playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

            //OBTENEMOS LA LISTA DE JUGADORES
            playerAmount = playerManager.GetPlayerAmount();
        }
        else { Debug.LogWarning("NO SE HA ENCONTRADO PLAYER MANAGER"); }

        for (int i = 0; i < playerAmount; i++)
        {
            playersCards[i].SetActive(true);
            playerArea[i].SetActive(true);

            PlayerCard card = playersCards[i].GetComponent<PlayerCard>();

            if (playerManager != null)
            {
                //ESTABLCEMOS SU DISEÑO
                card.SetPlayerColor(playerManager.GetPlayer(i).PlayerColor);
                card.SetPlayerIcon(playerManager.GetPlayer(i).PlayerIcon);
            }
        }
    }
    public void EndGame(int playerID)
    {
        //EFECTO DE FINALIZACION
        winFlash.SetActive(false);
        winFlash.SetActive(true);

        if(playerManager != null){playerManager.IncreasePlayerScore(playerID, playerAmount); } //GUARDAMOS PUNTUACION DIRECTAMENTE CON SU ID

        playerAmount--;

        //CUNADO GANE EL PENULTIMO JUGADR ACABA LA PARTIDA
        if (playerAmount <= 1)
        {
            //AÑADIR PUNTUACION DEL ULTIMO JUGADOR (SERA DE 1)
            if (playerManager != null){playerManager.IncreasePlayerScore(playerID, playerAmount);}

            //PARALIZAR TODOS LOS RB ACTIVOS
            Rigidbody[] allRBs = GameObject.FindObjectsOfType<Rigidbody>();
            foreach (Rigidbody rb in allRBs){if (rb != null) rb.isKinematic = true;}

            //DESCTIVAR TODO LOS CONTROLES DE LOS JUGADORES
            for (int i = 0; i < playerAmount; i++)
            {
                winDetectors[i].enabled = false;
                playerController[i].enabled = false;
                movBtwnPointsScript[i].enabled = false;
            }

            //CARGAMOS FADE A OTRA ESCENA
            Invoke("LoadLeaderboard", 5f);
        }
        else {Debug.Log("UN JUGADOR HA TERMINADO"); }
    }
    //ACTIVA EL FADE QUE CARGA EL LEADERBOARD
    private void LoadLeaderboard(){fade.SetActive(true);}
}
