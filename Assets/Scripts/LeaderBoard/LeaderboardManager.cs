using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject[] playerCards;
    [SerializeField] private TextMeshProUGUI[] playersScoreTxt;

    private PlayerManager playerManager;

    [Header ("TESTING ONLY")]
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

        //ITERAREMOS POR TODOS LOS JUGAORES ENCONTRADOS
        for(int i = 0; i < playerAmount; i++)
        {
            //ACTIVAMOS JUGADOR
            playerCards[i].SetActive(true);

            PlayerCard card = playerCards[i].GetComponent<PlayerCard>();

            if(playerManager != null)
            {
                //ESTABLCEMOS SU DISEÑO
                card.SetPlayerColor(playerManager.GetPlayer(i).playerColor);
                card.SetPlayerIcon(playerManager.GetPlayer(i).playerIcon);

                //ESTABLECER PUNTUACIÓN
                playersScoreTxt[i].text = playerManager.GetPlayer(i).score.ToString();
            }
        }
    }
}
