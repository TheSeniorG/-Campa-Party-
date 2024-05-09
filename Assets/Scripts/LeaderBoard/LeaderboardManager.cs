using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static PlayerManager;

public class LeaderboardManager : MonoBehaviour
{
    [Header("PLAYER REFERENCES")]
    [SerializeField] private GameObject[] playerCards;
    [SerializeField] private GameObject[] playersScore;
    [SerializeField] private GameObject[] playersObtainedScore;
    [SerializeField] private GameObject[] playerCheckbox;
    [SerializeField] private ReadyPlayer[] readyPlayerScripts;

    [Header("CLASSIFICATION")]
    [SerializeField] private Vector3[] leaderboardPos;
    [SerializeField] private float cardMovSpeed = 1f;
    [Header("MINIGAMES")]
    [SerializeField] private MinigameSelector minigameSelector;

    private TextMeshProUGUI[] playerScoreTxt, playerObtainedScoreTxt;
    private PlayerManager playerManager;
    private int playersReady = 0;
    private int playersAmount = 1;

    private void Awake()
    {
        // COMPROBAR QUE EXISTA PLAYER MANAGER
        if (GameObject.Find("PlayerManager"))
        {
            playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

            // OBTENER LA LISTA DE JUGADORES
            playersAmount = playerManager.GetPlayerAmount();
        }
        else{Debug.LogWarning("NO SE HA ENCONTRADO PLAYER MANAGER");}
    }

    private void Start()
    {
        playerScoreTxt = new TextMeshProUGUI[playersAmount];
        playerObtainedScoreTxt = new TextMeshProUGUI[playersAmount];

        // ITERAR POR TODOS LOS JUGADORES ENCONTRADOS Y ACTIVAR LOS COMPONENTES QUE LES PERTENECEN
        for (int i = 0; i < playersAmount; i++)
        {
            // ACTIVAR JUGADOR
            playerCards[i].SetActive(true);

            // ACTIVAR PUNTUACIONES DE LOS JUGADORES
            playersScore[i].SetActive(true);
            playerScoreTxt[i] = playersScore[i].GetComponent<TextMeshProUGUI>();
            playersObtainedScore[i].SetActive(true);
            playerObtainedScoreTxt[i] = playersObtainedScore[i].GetComponentInChildren<TextMeshProUGUI>();

            // CHECKBOX DEL MINIJUEGO PARA CUANDO ESTÉN LISTOS
            playerCheckbox[i].SetActive(true);

            PlayerCard card = playerCards[i].GetComponent<PlayerCard>();

            if (playerManager != null)
            {
                // ESTABLECER SU DISEÑO
                card.SetPlayerColor(playerManager.GetPlayer(i).PlayerColor);
                card.SetPlayerIcon(playerManager.GetPlayer(i).PlayerIcon);
                card.SetPlayerName(playerManager.GetPlayer(i).PlayerName);
                // REFERENCIAR SUS CONTROLADORES
                readyPlayerScripts[i].SetDeviceId(playerManager.GetPlayer(i).DeviceID);
            }
        }
    }

    // CONTROLADO POR EVENTOS DE ANIMACIÓN DEL PANEL PLAYER
    public void UpdateTempScores()
    {
        if (playerManager != null)
        {
            for (int i = 0; i < playersAmount; i++)
            {
                // ESTABLECER PUNTUACIÓN
                playerScoreTxt[i].text = playerManager.GetPlayer(i).OldScore.ToString();
                playerObtainedScoreTxt[i].text = "+" + playerManager.GetPlayer(i).ScoreObatined.ToString();
            }
        }
    }

    public void UpdateMaxScores()
    {
        if (playerManager != null)
        {
            for (int i = 0; i < playersAmount; i++)
            {
                // ESTABLECER PUNTUACIÓN MÁXIMA FINAL
                playerScoreTxt[i].text = playerManager.GetPlayer(i).Score.ToString();
            }
        }
    }

    public void PlayerReady()
    {
        playersReady++;
        // SI TODOS LOS JUGADORES ESTÁN LISTOS ACTIVAMOS TRANSICIÓN AL MINIJUEGO
        if (playersReady == playersAmount){minigameSelector.StartMinigame();}
    }

    public void UpdatePlayersPositions()
    {
        if (playersAmount > 1)
        {
            // PUNTUACIONES DE JUGADORES
            Dictionary<int, int> playersScores = new();

            // RELLENAR CON PUNTUACIONES
            for (int i = 0; i < playersAmount; i++)
            {
                Player player = playerManager.GetPlayer(i);
                playersScores.Add(player.PlayerID, player.Score);
            }

            // ORDENAR DE MANERA DESCENDENTE
            playersScores = playersScores.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            // MOVER LAS TARJETAS DE LOS JUGADORES A LAS NUEVAS POSICIONES
            for (int i = 0; i < playersScores.Count; i++)
            {
                var kvp = playersScores.ElementAt(i);
                GameObject cardToMove = playerCards[kvp.Key];
                Vector3 targetPosition = leaderboardPos[i];

                // MOVER LA TARJETA HACIA LA NUEVA POSICIÓN DE MANERA SUAVE
                while (Vector3.Distance(cardToMove.transform.localPosition, targetPosition) > 0.025f)
                {
                    cardToMove.transform.localPosition = Vector3.Lerp(cardToMove.transform.localPosition, targetPosition, Time.deltaTime * cardMovSpeed);
                }
            }
        }
    }
}