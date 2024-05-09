using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CodecWarsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] playerArea;
    [SerializeField] private GameObject[] playersCards;
    [SerializeField] private Codec[] playersCodec;
    [SerializeField] private PlayerInput[] playersInput;
    [SerializeField] private int gameTime = 120;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject fade;

    //VARIABLES PRIVADAS
    private bool startTimer = false;
    private float currentTime;
    private int playerAmount = 1;
    private Dictionary<int, int> playersScores = new();
    private PlayerManager playerManager;
    private DatabaseAccess databaseAccess;

    void Start()
    {
        currentTime = gameTime;

        //COMPROBAR QUE EXISTA PLAYER MANAGER
        if (GameObject.Find("PlayerManager"))
        {
            playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

            //OBTENEMOS LA LISTA DE JUGADORES
            playerAmount = playerManager.GetPlayerAmount();
            //BASE DE DATOS
            databaseAccess = GetComponent<DatabaseAccess>(); 
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
                playersCodec[i].AssignDeviceID(playerManager.GetPlayer(i).DeviceID);
            }
        }

        Invoke(nameof(SetGame),16f);
    }

    private void Update()
    {
        if (startTimer)
        {
            //TIEMPO DE JUEGO
            if (currentTime >= 0)
            {
                currentTime -= Time.deltaTime;
                //ACTUALIZAR TEXTO
                timeText.text = $"{currentTime.ToString("F0")}<size=32>s</size>";
            }
            else
            {
                SetGame();
                Invoke(nameof(Exit), 3f);
            }
        }
    }

    public void SetGame()
    {
        //CAMBIAR ESTADO TIMER
        startTimer = !startTimer;
        //CAMBAIAR ESTADOS DE LOS CONTROLES
        for (int i = 0; i < playerAmount; i++){playersInput[i].enabled = !playersInput[i].enabled;}
    }

    private void Exit()
    {
        //SOLO GUARDAMOS LA PUNTUACION SI HAY PLAYER MANAGER
        if (playerManager != null)
        {
            //GUARDAR I ORDENAR PUNTUACIÓN DE LOS JUGADORES
            for (int i = 0; i < playerAmount; i++) { playersScores.Add(i, playersCodec[i].GetScore()); }

            //ORDENAR PUNTUACIONES DE MAYOR A MENOR POR VALOR
            playersScores = playersScores.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            //REASSIGNAR A CADA JUGADOR SU PUNTUACION BALANCEADA
            for (int i = 0; i < playerAmount; i++)
            {
                //ESTABLECER NUEVAS PUNTUACIONES DEL 1 AL 4
                playersScores[i] = playerAmount - i;
                //ACTUALIZARLAS EN EL PLAYER MANAGER
                int dictionaryKey = playersScores.ElementAt(i).Key;
                playerManager.IncreasePlayerScore(dictionaryKey, playerAmount - i);
            }

            //REORDENAR DICCIONARIO POR KEY DE MENOR A MAYOR
            playersScores = playersScores.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            //SUBIR PUNTUACION A LA BASE DE DATOS
            databaseAccess.SetMiniGameEnd(playersScores);
        }

        fade.SetActive(true); Destroy(this);
    }
}
