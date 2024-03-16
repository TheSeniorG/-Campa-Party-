using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    //ATENCION: TODOS LOS OBJETOS DE ESTOS ARRAYS DEBEN COMENZAR APAGADOS
    [SerializeField] private GameObject[] playerCards, playersScore, playersObtainedScore ,playerCheckbox;
    //----------------------------------------------------------------------------------

    private TextMeshProUGUI[] playerScoreTxt, playerObtainedScoreTxt;
    private PlayerManager playerManager;
    [SerializeField] private MinigameSelector minigameSelector;
    private int playersReady = 0;

    [Header ("TESTING ONLY")]
    [SerializeField][Range(1,4)] private int playersAmount = 1;

    private void Start()
    {
        playerScoreTxt = new TextMeshProUGUI[playersAmount];
        playerObtainedScoreTxt = new TextMeshProUGUI[playersAmount];

        //COMPROBAR QUE EXISTA PLAYER MANAGER
        if (GameObject.Find("PlayerManager"))
        {
            playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

            //OBTENEMOS LA LISTA DE JUGADORES
            playersAmount = playerManager.GetPlayerAmount();
        }
        else { Debug.LogWarning("NO SE HA ENCONTRADO PLAYER MANAGER"); }

        //ITERAREMOS POR TODOS LOS JUGAORES ENCONTRADOS Y ACTIVAMOS LOS COMPONENETES QUE LE PERTOCAN
        for(int i = 0; i < playersAmount; i++)
        {
            //ACTIVAMOS JUGADOR
            playerCards[i].SetActive(true);

            //ACTIVAMOS PUNTUACIONES DE LOS JUGADORES
            playersScore[i].SetActive(true);
            playerScoreTxt[i] = playersScore[i].GetComponent<TextMeshProUGUI>();
            playersObtainedScore[i].SetActive(true);
            playerObtainedScoreTxt[i] = playersObtainedScore[i].GetComponentInChildren<TextMeshProUGUI>();

            //CHECKBOX DEL MINIJUEGO PARA CUANDO ESTEN LISTOS
            playerCheckbox[i].SetActive(true);

            PlayerCard card = playerCards[i].GetComponent<PlayerCard>();

            if(playerManager != null)
            {
                //ESTABLCEMOS SU DISEÑO
                card.SetPlayerColor(playerManager.GetPlayer(i).PlayerColor);
                card.SetPlayerIcon(playerManager.GetPlayer(i).PlayerIcon);
            }
        }
    }
    //CONTROLADO POR EVENTOS DE ANIMACION DEL PANEL PLAYER
    public void UpdateTempScores()
    {
        if(playerManager != null)
        {
            for (int i = 0; i < playersAmount; i++)
            {
                //ESTABLECER PUNTUACIÓN
                playerScoreTxt[i].text = playerManager.GetPlayer(i).OldScore.ToString();
                playerObtainedScoreTxt[i].text = playerManager.GetPlayer(i).ScoreObatined.ToString();
            }
        }
    }
    public void UpdateMaxScores()
    {
        if (playerManager != null)
        {
            for (int i = 0; i < playersAmount; i++)
            {
                //ESTABLECER PUNTUACIÓN MÁXIMA FINAL
                playerScoreTxt[i].text = playerManager.GetPlayer(i).Score.ToString();
            }
        }
    }
    public void PlayerReady()
    {
        playersReady++;
        //SI TODOS LOS JUGADOR ESTAN LISTOS ACTIVAMOS TRANSICION AL MINIJUEGO
        if (playersReady == playersAmount){minigameSelector.StartMinigame();}
    }
}
