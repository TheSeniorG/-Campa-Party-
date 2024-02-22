using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject countdown;
    private PlayerManager playerManager;
    private int playerAmount, playersReady;

    private void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }
    public void PlayerReady()
    {
        playersReady++;
        playerAmount = playerManager.GetPlayerAmount();

        //SI TODOS LOS JUGADORS ESTAN LISTOS SE INICIA LA PARTIDA
        if (playersReady == playerAmount){countdown.SetActive(true);}
    }
    public void PlayerUnReady()
    {
        playersReady--;

        //CANCELA LA CUENTA ATRÁS
        if (countdown.activeSelf) { countdown.SetActive(false);}
    }
}
