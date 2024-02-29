using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    //ATENCION: TODOS LOS OBJETOS DE ESTOS ARRAYS DEBEN COMENZAR APAGADOS
    [SerializeField] private GameObject[] playerCards, playerScores, playerCheckbox;

    private TextMeshPro[] playersScoreTxt;
    private PlayerManager playerManager;
    private MinigameSelector minigameSelector;
    private int playersReady = 0;

    [Header ("TESTING ONLY")]
    [SerializeField][Range(1,4)] private int playersAmount = 1;

    private void Start()
    {
        //COMPROBAR QUE EXISTA PLAYER MANAGER
        if (GameObject.Find("PlayerManager"))
        {
            playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

            //OBTENEMOS LA LISTA DE JUGADORES
            playersAmount = playerManager.GetPlayerAmount();
        }
        else { Debug.LogWarning("NO SE HA ENCONTRADO PLAYER MANAGER"); }

        //ITERAREMOS POR TODOS LOS JUGAORES ENCONTRADOS
        for(int i = 0; i < playersAmount; i++)
        {
            //ACTIVAMOS JUGADOR
            playerCards[i].SetActive(true);

            //ACTIVAMOS PUNTUACIONES DE LOS JUGADORES
            playerScores[i].SetActive(true);

            playersScoreTxt[i] = playerScores[i].GetComponent<TextMeshPro>();

            //CHECKBOX DEL MINIJUEGO PARA CUANDO ESTEN LISTOS
            playerCheckbox[i].SetActive(true);

            PlayerCard card = playerCards[i].GetComponent<PlayerCard>();

            if(playerManager != null)
            {
                //ESTABLCEMOS SU DISEÑO
                card.SetPlayerColor(playerManager.GetPlayer(i).PlayerColor);
                card.SetPlayerIcon(playerManager.GetPlayer(i).PlayerIcon);

                //ESTABLECER PUNTUACIÓN
                playersScoreTxt[i].text = playerManager.GetPlayer(i).Score.ToString();
            }
        }
    }
    public void PlayerReady()
    {
        playersReady++;
        //SI TODOS LOS JUGADOR HAN BLOQUEADO EMPEZAMOS
        if (playersReady == playersAmount){minigameSelector.StartMinigame();}
    }
}
