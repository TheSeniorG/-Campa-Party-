using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static PlayerManager;

public class PlayerCardConfig : MonoBehaviour
{
    // REFERENCIAS A COMPONENTES
    [SerializeField] private Sprite[] playerIcons;
    [SerializeField] private SpriteRenderer backgroundsColor;
    [SerializeField] private SpriteRenderer playerLock;
    [SerializeField] private TextMeshPro lockedText;
    [SerializeField] private SpriteRenderer playerIconRend;

    // VARIABLES DE CONTROL
    private PlayerManager playerManager;
    private int playerID;
    private bool playerLocked = false;
    private int actualColor = 0, actualIcon = 0;

    // ARREGLO DE COLORES
    private Color[] colors;

    private void Start()
    {
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
        // OBTENER REFERENCIA AL PLAYERMANAGER
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        // OBTENER NÚMERO (ID) DE JUGADOR (J.1 J.2 J.3 J.4)
        playerID = playerManager.GetListLength();

        // AL CREAR LA TARJETA, SE INICIALIZA Y AÑADE UN NUEVO JUGADOR
        Player newPlayer = new Player(playerID, Color.white, playerIcons[actualIcon]);
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
            backgroundsColor.color = colors[actualColor];
        }
    }

    // BLOQUEAR/DESBLOQUEAR AL JUGADOR
    public void LockPlayer()
    {
        // TOGGLE DEL ESTADO DE BLOQUEO
        playerLocked = !playerLocked;

        // CAMBIAR APARIENCIA DEL BOTÓN Y BLOQUEAR/ACTUALIZAR PARÁMETROS DEL JUGADOR
        if (playerLocked)
        {
            // CAMBIAR APARIENCIA DEL BOTÓN AL ESTAR BLOQUEADO
            lockedText.color = Color.gray;
            playerLock.color = new Color(0.2f, 0.2f, 0.2f);

            // BLOQUEAR Y ACTUALIZAR PARÁMETROS DEL JUGADOR
            playerManager.UpdatePlayerInfo(playerID, colors[actualColor], playerIcons[actualIcon]);
        }
        else
        {
            // RESTAURAR APARIENCIA DEL BOTÓN AL ESTAR DESBLOQUEADO
            lockedText.color = Color.black;
            playerLock.color = Color.white;
        }
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
            playerIconRend.sprite = playerIcons[actualIcon];
        }
    }
}