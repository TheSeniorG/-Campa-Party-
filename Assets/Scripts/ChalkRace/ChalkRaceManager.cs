using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChalkRaceManager : MonoBehaviour
{
    [Header("PLAYER")]
    //ESTAS LISTAS TIENEN QUE TENER TODOS LOS ELEMNTOS DESACTIVADOS ANTES DE INICIAR EL JUEGO
    [SerializeField] private GameObject[] players;
    [SerializeField] private GameObject[] playersCards;
    [SerializeField] private GameObject[] playersPosIndicator;
    //-----------------------------------------------------------
    [Header("OTHER")]
    [SerializeField] private ObstacleGenerator obstacleGenerator;
    [SerializeField] private ScrollMatOffset chalkboardMatSlider;
    [SerializeField] private GameObject fade;

    private readonly List<PlayerChalk> playerChalks = new();
    private int playersRemaining;
    private PlayerManager playerManager;
    private Dictionary<int, int> playersScore = new();

    //SIEMPRE HAY 1 JUGADOR
    private int playerAmount = 1;

    //AUDIO
    private AudioSource audioSource;
    [SerializeField] private AudioClip endGame_SFX;

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

        playersRemaining = playerAmount;

        for (int i = 0; i < playerAmount; i++)
        {
            //ACTIVAR JUGADORES
            players[i].SetActive(true); 
            playersCards[i].SetActive(true);
            playersPosIndicator[i].SetActive(true);

            //GUARDAR COMPONENTE DEL PLAYER EN LA LISTA
            playerChalks.Add(players[i].GetComponent<PlayerChalk>());
            playerChalks[i].SetChalkColor(Color.white);

            if (playerManager != null)
            {
                //ESTABLCEMOS DISEÑO DE LA TARJETA
                PlayerCard card = playersCards[i].GetComponent<PlayerCard>();
                card.SetPlayerColor(playerManager.GetPlayer(i).PlayerColor);
                card.SetPlayerIcon(playerManager.GetPlayer(i).PlayerIcon);
                card.SetPlayerName(playerManager.GetPlayer(i).PlayerName);
                playerChalks[i].AssignDeviceID(playerManager.GetPlayer(i).DeviceID);

                //CAMBIAR COLOR TIZA
                playerChalks[i].SetChalkColor(playerManager.GetPlayer(i).PlayerColor);
            }

        }
        // INICIA LA CORRUTINA PARA HABILITAR EL JUEGO
        StartCoroutine(SetGame(13f));

        // CADA 5 SEGUNDOS AUMENTA EL SPAWN RATIO DE OBSTÁCULOS
        InvokeRepeating(nameof(IncreaseDifficulty), 12f, 5f);
    }

    //AUMENTA EL SPAWN RATE DE OBSTACULOS
    private void IncreaseDifficulty(){if(obstacleGenerator != null)obstacleGenerator.IncreaseSpawnRate(0.25f);}

    public IEnumerator SetGame(float waitCall = 0f)
    {
        // ESPERA ANTES DE REALIZAR CAMBIOS
        yield return new WaitForSeconds(waitCall);

        bool newState = playersRemaining >0;

        // HABILITA EL SPAWNER DE OBSTACULOS
        obstacleGenerator.enabled = newState;

        // HABILITAR BOLEANOS DEL JUGADOR
        foreach (PlayerChalk playerChalk in playerChalks)
            playerChalk.ToggleGame(newState);

        //HACER QUE LA PIZARRA SE DESPLACE
        chalkboardMatSlider.enabled = true;
    }

    public void EliminatePlayer(int playerId, int score)
    {
        // DECREMENTA EL NÚMERO DE JUGADORES RESTANTES
        playersRemaining--;

        if(playerManager != null)
        {
            //GUARDAR LA PUNTUACION HECHA POR EL JUGADOR
            playersScore.Add(playerId, score);
        }

        // SI NO HAY JUGADORES RESTANTES, DESACTIVA EL JUEGO
        if (playersRemaining == 0) 
        {
            if(playerManager != null)
            {
                //ORDENAR PUNTUACIONES DEL JUGADOR
                playersScore = playersScore.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                for (int i = 0; i < playerAmount; i++)
                {
                    //ESTABLECER NUEVAS PUNTUACIONES
                    playersScore[i] = playerAmount - i;
                    //ACTUALIZARLAS EN EL PLAYER MANAGER
                    int dictionaryKey = playersScore.ElementAt(i).Key;
                    playerManager.IncreasePlayerScore(dictionaryKey,playerAmount-i);
                }
            }

            //DESACTIVAR CONTROLES
            StartCoroutine(SetGame());
            StartCoroutine(Fade(3f));

            //SFX
            audioSource.PlayOneShot(endGame_SFX);
        }
    }

    private IEnumerator Fade(float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);
        fade.SetActive(true);
    }
}