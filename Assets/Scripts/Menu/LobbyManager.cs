using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject countdown;
    [SerializeField] private GameObject lockEffect;
    [SerializeField] private GameObject playerCard;
    [SerializeField] private Transform playersCardLayout;
    [SerializeField] private Animator cameraAnimator, lobbyAnimator;

    private List<int> devicesRegistered = new();
    private List<PlayerInput> playersInputRegistered = new();

    [SerializeField] private GameObject playerManager;
    private DatabaseAccess databaseAccess;
    private PlayerManager playerManagerScript;
    private PlayerInput playerInput;
    private AudioSource audioSource;

    private int playersReady = 0;
    private int connectedPlayers = 0;
    private const int MAX_PLAYERS = 4;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); audioSource = GetComponent<AudioSource>();
        playerManagerScript = playerManager.GetComponent<PlayerManager>();
        databaseAccess = playerManager.GetComponent<DatabaseAccess>();
    }

    public void PlayerReady()
    {
        playersReady++;
        CheckStartGame();
    }

    public void PlayerUnready()
    {
        playersReady--;
        CheckStartGame();
        CancelInvoke();
    }

    private void CheckStartGame()
    {
        //COMPROBAMOS CUANTOS JUGADORES HAY LISTOS
        int playersAmount = playerManagerScript.GetPlayerAmount();
        Debug.Log(playersReady + " DE " + playersAmount + " LISTOS");

        //ACTIVAMOS O DESACTIVAMOS ELEMENTOS DEPENDIENDO DE SI LOS JUGADORES ESTAN LISTOS
        countdown.SetActive(playersReady == playersAmount);
        lockEffect.SetActive(playersReady == playersAmount);
        playerInput.enabled = (playersReady != playersAmount);

        //DESACTIVAR ONTROLES DE JUGADORES ARA QUE NO PUEDAN BUGUEARLO
        if(playersReady >= playersAmount) { Invoke(nameof(DeactivatePlayersInputs),3f); }
    }

    public void GoBack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && playersReady == 0)
        {
            //ACTIVAMOS ANIMACION DE VUELTA ATRÁS
            cameraAnimator.SetTrigger("next");
            lobbyAnimator.SetTrigger("next");
            //SFX
            audioSource.Play();
        }
    }

    public void SetupPlayer(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            //OBTENEMOS EL ID DEL CONTROLADOR A UNIRSE
            int deviceID = callbackContext.control.device.deviceId;
            if (connectedPlayers < MAX_PLAYERS && !devicesRegistered.Contains(deviceID))
            {
                //REGISTRAMOS EL CONTROLADOR DEL JUGADOR QUE SE UNE
                devicesRegistered.Add(deviceID);

                //CREAMOS NA TARJETA DE JUGADOR I LE ASIGNAMOS UN ID
                GameObject newPlayerCard = Instantiate(playerCard, playersCardLayout);
                newPlayerCard.GetComponent<PlayerCardConfig>().SetupPlayer(connectedPlayers, callbackContext.control.device);

                //REGISTRAMOS EL CONTROADOR AÑADIDO
                playersInputRegistered.Add(newPlayerCard.GetComponent<PlayerInput>());

                connectedPlayers++;
                Debug.Log("JUGADOR " + connectedPlayers + " CONECTADO");
                Debug.Log("ID CONTROLADOR: " + deviceID);
            }
        }
    }

    private void DeactivatePlayersInputs()
    {
        foreach(PlayerInput pInpt in playersInputRegistered) { pInpt.enabled = false;}

        //BASE DE DATOS
        //REGISTRAMOS EN LA BASE DE DATOS EL INICIO DE LA PARTIDA
        databaseAccess.SetStartGame();
        for(int i= 0; i< connectedPlayers; i++)
        {
            databaseAccess.AddPlayer(playerManagerScript.GetPlayer(i).PlayerName);
        }
    }
}