using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MadTowerManager : MonoBehaviour
{
    [SerializeField] private GameObject fade, winFlash;
    [SerializeField] private GameObject[] playersCards, playerArea;
    [SerializeField] private WinDetection[] winDetectors;
    [SerializeField] private PieceSpawner[] playerController;
    [SerializeField] private MoveBetweenPoints[] movBtwnPointsScript;

    private Dictionary<int, int> playersScores = new Dictionary<int, int>();
    private PlayerManager playerManager;
    private DatabaseAccess databaseAccess;
    private AudioSource audioSource;
    private int playerAmount = 1;

    private void Awake(){audioSource = GetComponent<AudioSource>();}
    private void Start()
    {
        //COMPROBAR QUE EXISTA PLAYER MANAGER
        if (GameObject.Find("PlayerManager"))
        {
            playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

            //OBTENEMOS LA LISTA DE JUGADORES
            playerAmount = playerManager.GetPlayerAmount();

            //OBTENER BASE DE DATOS
            databaseAccess = playerManager.GetComponent<DatabaseAccess>();
        }
        else { Debug.LogWarning("NO SE HA ENCONTRADO PLAYER MANAGER"); }

        for (int i = 0; i < playerAmount; i++)
        {
            //ACTIVAR ZONAS DE JUGADORES Y SCRIPTS
            playersCards[i].SetActive(true);
            playerArea[i].SetActive(true);


            if (playerManager != null)
            {
                PlayerCard card = playersCards[i].GetComponent<PlayerCard>();

                //ESTABLCEMOS SU DISEÑO
                card.SetPlayerColor(playerManager.GetPlayer(i).PlayerColor);
                card.SetPlayerIcon(playerManager.GetPlayer(i).PlayerIcon);
                card.SetPlayerName(playerManager.GetPlayer(i).PlayerName);

                //ASGINAR CONTROLADORES
                playerController[i].AssignDeviceID(playerManager.GetPlayer(i).DeviceID);
            }
        }
    }
    public void EndGame(int playerID)
    {
        //EFECTO DE FINALIZACION
        winFlash.SetActive(false);
        winFlash.SetActive(true);

        //SUMAR PUTUACION AL PLAYER ROOT
        if(playerManager != null){playerManager.IncreasePlayerScore(playerID, playerAmount); } //GUARDAMOS PUNTUACION DIRECTAMENTE CON SU ID
        playersScores.Add(playerID,playerAmount);

        playerAmount--;

        //CUNADO GANE EL PENULTIMO JUGADR ACABA LA PARTIDA
        if (playerAmount < 1)
        {
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

            //ORDENAR PUNTUACIONES
            var sortedDict = playersScores.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            //BASE DE DATOS
            databaseAccess.SetMiniGameEnd(sortedDict);

            //CARGAMOS FADE A OTRA ESCENA EN 3 SEGUNDOS
            Invoke(nameof(LoadLeaderboard), 3f);

            //SFX
            audioSource.Play();
        }
        else {Debug.Log("UN JUGADOR HA TERMINADO"); }
    }
    //ACTIVA EL FADE QUE CARGA EL LEADERBOARD
    private void LoadLeaderboard(){fade.SetActive(true);}
}
