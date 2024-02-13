using System.Collections;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ChalkRaceManager : MonoBehaviour
{
    // VARIABLES SERIALIZADAS
    [SerializeField] private ObstacleGenerator obstacleGenerator;
    [SerializeField] private ScrollMatOffset chalkboardMatSlider;
    [SerializeField] private GameObject fade;
    [SerializeField] private GameObject[] players, playersCards;

    // VARIABLES PRIVADAS
    private PlayerChalk[] playerChalks;
    private int playersRemaining;
    private PlayerManager playerManager;

    //SIEMPRE HAY 1 JUGADOR
    private int playerAmount = 1;

    private void Start()
    {
        //COMPROBAR QUE EXISTA PLAYER MANAGER
        if (GameObject.Find("PlayerManager"))
        {
            playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

            //OBTENEMOS LA LISTA DE JUGADORES
            playerAmount = playerManager.GetListLength();
        }
        else { Debug.LogWarning("NO SE HA ENCONTRADO PLAYER MANAGER"); }

        for (int i = 0; i < playerAmount; i++)
        {
            //ACTIVAR JUGADORES
            players[i].SetActive(true); 
            playersCards[i].SetActive(true);

            if(playerManager != null)
            {
                PlayerCard card = playersCards[i].GetComponent<PlayerCard>();
                //ESTABLCEMOS SU DISEÑO
                card.SetPlayerColor(playerManager.GetPlayer(i).playerColor);
                card.SetPlayerIcon(playerManager.GetPlayer(i).playerIcon);
            }
        }

        playersRemaining = playerAmount;

        // OBTÉN INSTANCIAS DE PLAYERCHALK
        playerChalks = new PlayerChalk[playerAmount];
        for (int i = 0; i < playerAmount; i++)
        {
            //ACCEDER COMPONENTE DEL PLAYER
            playerChalks[i] = players[i].GetComponent<PlayerChalk>();

            if(playerManager != null)
            {
                //CAMBIAR COLOR TIZA
                playerChalks[i].SetChalkColor(playerManager.GetPlayer(i).playerColor);
            }
        }

        // INICIA LA CORRUTINA PARA HABILITAR EL JUEGO
        StartCoroutine(SetGame(12f));

        // CADA 5 SEGUNDOS AUMENTA EL SPAWN RATIO DE OBSTÁCULOS
        InvokeRepeating("IncreaseDifficulty", 12f, 5f);
    }

    // MÉTODO DE AUMENTO DE DIFICULTAD
    private void IncreaseDifficulty(){if(obstacleGenerator != null)obstacleGenerator.IncreaseSpawnRate(0.25f);}

    // CORRUTINA PARA GESTIONAR EL ESTADO DEL JUEGO
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

    // MÉTODO DE ELIMINACIÓN DE JUGADORES
    public void EliminatePlayer()
    {
        // DECREMENTA EL NÚMERO DE JUGADORES RESTANTES
        playersRemaining--;

        // SI NO HAY JUGADORES RESTANTES, DESACTIVA EL JUEGO
        if (playersRemaining == 0) 
        {
            StartCoroutine(SetGame());
            StartCoroutine(Fade(3f));
        }
    }
    
    private IEnumerator Fade(float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);
        fade.SetActive(true);
    }
}
