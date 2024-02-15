using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.WSA;

public class BasketPaperManager : MonoBehaviour
{
    // VARIABLES SERIALIZADAS
    [SerializeField] private GameObject fade;
    [SerializeField] private GameObject[] players, playersCards;
    [SerializeField] private PaperLauncher[] launcherScripts;
    [SerializeField] private Rotator[] rotatorScripts;
    [SerializeField] private int gameTime = 120;
    [SerializeField] private TextMeshProUGUI timeText;

    //VARIABLES PRIVADAS
    private bool startTimer = false;
    private float currentTime;
    private PlayerManager playerManager;

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
            playerAmount = playerManager.GetListLength();
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
                card.SetPlayerColor(playerManager.GetPlayer(i).playerColor);
                card.SetPlayerIcon(playerManager.GetPlayer(i).playerIcon);
            }
        }
        Invoke("SetGame", 12.5f);
    }
    private void SetGame()
    {
        //CAMBIAR ESTADO TIMER
        startTimer = !startTimer;
        //ACTIVAR SCRITS JUGADORES
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
                //PARAMOS LA CUENTA

                startTimer = false;
                SetGame();
                Invoke("EndScene", 3f);
            }
        }
    }
    private void EndScene() { fade.SetActive(true); Destroy(this); }
}
