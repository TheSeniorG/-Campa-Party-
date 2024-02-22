using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerManager;

public class PlayerCardConfig : MonoBehaviour
{
    
    // REFERENCIAS A COMPONENTES PROPIOS
    [SerializeField] private Sprite[] playerIcons;
    [SerializeField] private Image playerIconImage;
    [SerializeField] private GameObject readyIcon;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private GameObject lockedEffect;
    private Image readyImage;
    private Image backgroundImage;

    // VARIABLES DE CONTROL
    private PlayerManager playerManager;
    private int playerID;
    private bool playerLocked = false;
    private int actualColor = 0, actualIcon = 0;
    private Player newPlayer;
    private LobbyManager lobbyManager;

    //SELECTOR DE OPCIONES
    private float[] selectorPosY = new float[3] { -45f, -105f, -165f };
    private int currentOption = 0;
    [SerializeField] private Transform selector;

    //COLORES
    private Color[] colors;

    private void Start()
    {
        backgroundImage = GetComponent<Image>(); 
        readyImage = readyIcon.GetComponent<Image>();

        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();

        //ARRAY DE COLORES
        colors = new Color[]{
        Color.white,
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.magenta,
        Color.cyan,
        new Color(1.0f, 0.5f, 0.0f), // Naranja
        new Color(0.5f, 0.2f, 0.8f), // Púrpura
        new Color(0.8f, 0.8f, 0.2f), // Amarillo claro
        new Color(0.3f, 0.6f, 0.9f), // Azul claro
        new Color(0.7f, 0.4f, 0.1f), // Marrón
        new Color(0.4f, 0.9f, 0.5f), // Verde claro
        new Color(0.9f, 0.2f, 0.7f), // Rosa
        new Color(0.1f, 0.8f, 0.9f), // Turquesa
        new Color(0.6f, 0.3f, 0.7f), // Púrpura oscuro
        new Color(0.2f, 0.2f, 0.2f), // Gris oscuro
        new Color(0.8f, 0.8f, 0.8f), // Gris claro
        new Color(0.5f, 0.5f, 1.0f), // Azul claro
        new Color(0.9f, 0.7f, 0.5f), // Melocotón
        new Color(0.2f, 0.7f, 0.2f), // Verde intenso
        new Color(0.7f, 0.1f, 0.1f), // Rojo intenso
        new Color(0.8f, 0.5f, 0.2f), // Marrón claro
        new Color(0.2f, 0.8f, 0.5f), // Verde azulado
        new Color(0.8f, 0.2f, 0.5f)  // Rosa oscuro
        };

        // OBTENER NÚMERO (ID) DE JUGADOR (J.1 J.2 J.3 J.4)
        playerID = playerManager.GetPlayerAmount();

        // AL CREAR LA TARJETA, SE INICIALIZA Y AÑADE UN NUEVO JUGADOR
        newPlayer = new Player(playerID, Color.white, playerIcons[actualIcon]);
        playerManager.AddPlayer(newPlayer);
    }
    // CAMBIAR COLOR DEL JUGADOR
    public void ChangeColor(int colorIndex)
    {
        if (!playerLocked)
        {
            // INCREMENTAR ÍNDICE
            actualColor += colorIndex;
            // EVITAR QUE SE PASE
            actualColor = Mathf.Clamp(actualColor, 0, colors.Length - 1);
            // CAMBIAR COLOR
            backgroundImage.color = colors[actualColor];
            readyImage.color = colors[actualColor];
        }
    }

    // BLOQUEAR/DESBLOQUEAR AL JUGADOR
    public void LockPlayer()
    {
        // TOGGLE DEL ESTADO DE BLOQUEO
        playerLocked = !playerLocked;
        
        //ICONO READY
        readyIcon.SetActive(playerLocked);
        lockIcon.SetActive(playerLocked);

        //POR SI EL JUGADOR VUEVE ATRÁS
        if (playerLocked) 
        {
            //EEFCTO DE BLOQUEO
            lockedEffect.SetActive(true);

            //ACTUAIZAMOS EL PLAYER
            newPlayer = new Player(playerID, colors[actualColor], playerIcons[actualIcon]);
            playerManager.AddPlayer(newPlayer);

            lobbyManager.PlayerReady();
        }
        else { lobbyManager.PlayerUnReady(); }
    }

    // CAMBIAR ÍCONO DEL JUGADOR
    public void ChangeIcon(int iconIndex)
    {
        if (!playerLocked)
        {
            // INCREMENTAR ÍNDICE
            actualIcon += iconIndex;
            // EVITAR QUE SE PASE
            actualIcon = Mathf.Clamp(actualIcon, 0, playerIcons.Length - 1);
            // ACTUALIZAR IMAGEN DEL JUGADOR
            playerIconImage.sprite = playerIcons[actualIcon];
        }
    }

    private void Update()
    {
        //CONTROLES DEL JUGADOR PARA EDITAR EL PERSONAJE
        if (!playerLocked)
        {
            if(Input.GetKeyDown(KeyCode.DownArrow)){MoveSelector(1);}
            else if (Input.GetKeyDown(KeyCode.UpArrow)) { MoveSelector(-1); }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) { ChooseOption(1); }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) { ChooseOption(-1); }
        }
        else{if (Input.GetKeyDown(KeyCode.Escape)) { ChooseOption(); }}
    }
    private void MoveSelector(int nextPos = 1)
    {
        // PASAMOS A LA SIGUIENTE OPCION
        currentOption = Mathf.Clamp(currentOption += nextPos,0,2);
        Vector3 newPos = new Vector3(0f, selectorPosY[currentOption], 0f);
        selector.localPosition = newPos;
    }
    private void ChooseOption(int next = 1)
    {
        switch (currentOption)
        {
            case 0:
                ChangeIcon(next);
                break;
            case 1:
                ChangeColor(next);
                break;
            case 2:
                LockPlayer();
                break;
            default:
                break;
        }
    }
}