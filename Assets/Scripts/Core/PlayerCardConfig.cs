using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static PlayerManager;

public class PlayerCardConfig : MonoBehaviour
{
    [SerializeField] private Sprite[] playerIcons;
    [SerializeField] private Image playerIconImage;
    [SerializeField] private GameObject readyIcon;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private Transform selector;
    [SerializeField] private TextMeshProUGUI playerName;

    private Image readyImage, backgroundImage;
    private PlayerManager playerManager;
    private LobbyManager lobbyManager;

    //AUDIO
    private AudioSource audioSource;
    [SerializeField] private AudioClip navSFX;

    private int playerID;
    private InputDevice assignedDevice;
    private int deviceID;
    private int actualColor = 0, actualIcon = 0, currentOption = 0;

    private readonly float[] selectorPosY = new float[3] { -45f, -105f, -165f };

    private bool playerLocked = false;
    private Player newPlayer;
    private PlayerInput playerInput;
    private Color[] colors;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
        readyImage = readyIcon.GetComponent<Image>();
        playerInput = GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        //SETUP DE COMPONENETES
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        lobbyManager = FindObjectOfType<LobbyManager>();

        //ARRAY DE COLORES
        colors = new Color[]{
            Color.white, Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan,
            new Color(1.0f, 0.5f, 0.0f), new Color(0.5f, 0.2f, 0.8f), new Color(0.8f, 0.8f, 0.2f),
            new Color(0.3f, 0.6f, 0.9f), new Color(0.7f, 0.4f, 0.1f), new Color(0.4f, 0.9f, 0.5f),
            new Color(0.9f, 0.2f, 0.7f), new Color(0.1f, 0.8f, 0.9f), new Color(0.6f, 0.3f, 0.7f),
            new Color(0.2f, 0.2f, 0.2f), new Color(0.8f, 0.8f, 0.8f), new Color(0.5f, 0.5f, 1.0f),
            new Color(0.9f, 0.7f, 0.5f), new Color(0.2f, 0.7f, 0.2f), new Color(0.7f, 0.1f, 0.1f),
            new Color(0.8f, 0.5f, 0.2f), new Color(0.2f, 0.8f, 0.5f), new Color(0.8f, 0.2f, 0.5f)
        };

        //REGISTRAMOS UNA INSTANCIA DEL PLAYER
        newPlayer = new Player(playerID, colors[actualColor], playerIcons[actualIcon], deviceID);
        playerManager.AddPlayer(newPlayer);

        //NOMBRE DEL PLAYER
        playerName.text = newPlayer.PlayerName;

        //ASSIGNAR CONTROLADOR
        playerInput.SwitchCurrentControlScheme(assignedDevice);
    }

    public void SetupPlayer(int assignedID, InputDevice designedDevice)
    {
        //ASSIGNAR ID DE CONEXION
        playerID = assignedID;
        assignedDevice = designedDevice;
    }

    private void ChangeColor(int colorIndex)
    {
        if (!playerLocked)
        {
            actualColor += colorIndex;
            actualColor = Mathf.Clamp(actualColor, 0, colors.Length - 1);
            backgroundImage.color = colors[actualColor];
            readyImage.color = colors[actualColor];
        }
    }

    private void LockPlayer()
    {
        playerLocked = !playerLocked;

        readyIcon.SetActive(playerLocked);
        lockIcon.SetActive(playerLocked);

        if (playerLocked)
        {
            //RECONFIGURAMOS EL PLAYER CON SUS NUEVAS PROPIEDADES
            newPlayer = new Player(playerID, colors[actualColor], playerIcons[actualIcon], deviceID);
            playerManager.AddPlayer(newPlayer);
            lobbyManager.PlayerReady();
        }
        else
        {
            lobbyManager.PlayerUnready();
        }
    }

    private void ChangeIcon(int iconIndex)
    {
        if (!playerLocked)
        {
            actualIcon += iconIndex;
            actualIcon = Mathf.Clamp(actualIcon, 0, playerIcons.Length - 1);
            playerIconImage.sprite = playerIcons[actualIcon];
        }
    }

    public void MoveSelector(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            //Debug.Log("MUEVE");
            if (!playerLocked)
            {
                //OBTENER FLOAT
                int menuNavValue = (int)playerInput.actions["UI/MenuNavVer"].ReadValue<float>();

                //LIMITAR LAS OPCIONES
                currentOption = Mathf.Clamp(currentOption += menuNavValue, 0, 2);

                //PUT SELECTOR TO NEW POS
                Vector3 newPos = new(0f, selectorPosY[currentOption], 0f);
                selector.localPosition = newPos;

                //SFX
                audioSource.PlayOneShot(navSFX);
            }
        }
    }

    public void ChooseOption(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            //OBTENER FLOAT
            int selectValue = (int)playerInput.actions["UI/MenuNavHor"].ReadValue<float>();

            switch (currentOption)
            {
                case 0:
                    ChangeIcon(selectValue);
                    break;
                case 1:
                    ChangeColor(selectValue);
                    break;
                case 2:
                    LockPlayer();
                    break;
                default:
                    break;
            }

            //SFX
            audioSource.PlayOneShot(navSFX);
        }
    }
}