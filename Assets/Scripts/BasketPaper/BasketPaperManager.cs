using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.WSA;

public class BasketPaperManager : MonoBehaviour
{
    // VARIABLES SERIALIZADAS
    [SerializeField] private GameObject fade;
    [SerializeField] private GameObject[] players, playersCards;
    [SerializeField] private Baskets[] baskets;
    [SerializeField] private PaperLauncher[] launcherScripts;
    [SerializeField] private Rotator[] rotatorScripts;
    [SerializeField] private int gameTime = 120;
    [SerializeField] private TextMeshProUGUI timeText;

    //VARIABLES PRIVADAS
    private bool startTimer = false;
    private float currentTime;
    private PlayerManager playerManager;
    private Dictionary<int, int> playersScore = new Dictionary<int, int>();

    [Header("ESTO LUEGO SERA SOLO PRIVADO")]
    //DURANTE TESTEO LA HAGO SERIALIZABLE
    [SerializeField][Range(1, 4)] private int playerAmount = 1;


    private void Start()
    {
        currentTime = gameTime;

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
            //ACTIVAR JUGADORES
            players[i].SetActive(true);
            playersCards[i].SetActive(true);

            if (playerManager != null)
            {
                PlayerCard card = playersCards[i].GetComponent<PlayerCard>();
                //ESTABLCEMOS SU DISE�O
                card.SetPlayerColor(playerManager.GetPlayer(i).PlayerColor);
                card.SetPlayerIcon(playerManager.GetPlayer(i).PlayerIcon);
            }
        }
        Invoke("SetGame", 12.5f);
    }
    private void SetGame()
    {
        //CAMBIAR ESTADO TIMER
        startTimer = !startTimer;
        //CAMBAIR ESTADOS DE LOS JUGADORES
        for (int i = 0; i < playerAmount; i++)
        {
            launcherScripts[i].enabled = !launcherScripts[i].enabled;
            rotatorScripts[i].ToogleRotation();
        }
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
            //GUARDAR I ORDENAR PUNTUACI�N DE LOS JUGADORES
            for (int i = 0; i < playerAmount; i++) { playersScore.Add(i, baskets[i].GetScore()); }

            //ORDENAR PUNTUACIONES
            playersScore = playersScore.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            //REASSIGNAR A CADA JUGADOR SU PUNTUACION BALANCEADA
            for (int i = 0; i < playerAmount; i++)
            {
                //ESTABLECER NUEVAS PUNTUACIONES DEL 1 AL 4
                playersScore[i] = playerAmount - i;
                //ACTUALIZARLAS EN EL PLAYER MANAGER
                int dictionaryKey = playersScore.ElementAt(i).Key;
                playerManager.IncreasePlayerScore(dictionaryKey, playerAmount - i);
            }
        }

        fade.SetActive(true); Destroy(this); 
    }
}
