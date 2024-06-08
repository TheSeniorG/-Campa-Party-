using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BasketPaperManager : MonoBehaviour
{
    // VARIABLES SERIALIZADAS
    [SerializeField] private GameObject fade;
    [SerializeField] private GameObject[] players, playersCards, obstaclesSpawner;
    [SerializeField] private Baskets[] baskets;
    [SerializeField] private PaperLauncher[] launcherScripts;
    [SerializeField] private Rotator[] rotatorScripts;
    [SerializeField] private int gameTime = 120;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private AudioClip whistle_SFX;

    //VARIABLES PRIVADAS
    private bool startTimer = false;
    private float currentTime;
    private PlayerManager playerManager;
    private DatabaseAccess databaseAccess;
    private Dictionary<int, int> playersScores = new();
    private AudioSource audioSource;
    private int playerAmount = 1;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        currentTime = gameTime;

        //COMPROBAR QUE EXISTA PLAYER MANAGER
        if (GameObject.Find("PlayerManager"))
        {
            playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

            //OBTENEMOS LA LISTA DE JUGADORES
            playerAmount = playerManager.GetPlayerAmount();
            //BASE DE DATOS
            databaseAccess = playerManager.GetComponent<DatabaseAccess>();
        }
        else { Debug.LogWarning("NO SE HA ENCONTRADO PLAYER MANAGER"); }

        for (int i = 0; i < playerAmount; i++)
        {
            //ACTIVAR JUGADORES
            players[i].SetActive(true);
            playersCards[i].SetActive(true);

            if (playerManager != null)
            {
                PlayerCard card = playersCards[i].GetComponent<PlayerCard>();
                //ESTABLCEMOS SU DISEÑO
                card.SetPlayerColor(playerManager.GetPlayer(i).PlayerColor);
                card.SetPlayerIcon(playerManager.GetPlayer(i).PlayerIcon);
                card.SetPlayerName(playerManager.GetPlayer(i).PlayerName);

                launcherScripts[i].AssignDeviceID(playerManager.GetPlayer(i).DeviceID);
            }
        }
        Invoke(nameof(SetGame), 12.5f);
    }
    private void SetGame()
    {
        //CAMBIAR ESTADO TIMER
        startTimer = !startTimer;

        //SFX
        audioSource.Play();

        //CAMBAIAR ESTADOS DE LOS JUGADORES
        for (int i = 0; i < playerAmount; i++)
        {
            launcherScripts[i].enabled = !launcherScripts[i].enabled;
            rotatorScripts[i].ToggleRotation();
        }
        foreach (GameObject go in obstaclesSpawner){go.SetActive(!go.activeSelf);}
    }
    private void Update()
    {
        if (startTimer)
        {
            //TIEMPO DE JUEGO
            if(currentTime >= 0)
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
    private void Exit()
    {
        //SOLO GUARDAMOS LA PUNTUACION SI HAY PLAYER MANAGER
        if(playerManager != null)
        {
            //GUARDAR I ORDENAR PUNTUACIÓN DE LOS JUGADORES
            for (int i = 0; i < playerAmount; i++) { playersScores.Add(i, baskets[i].GetScore()); }

            //ORDENAR PUNTUACIONES
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
            //GUARDAR MINIJUEGO
            databaseAccess.SetMiniGameEnd(playersScores);
        }

        fade.SetActive(true); Destroy(this); 
    }
}
