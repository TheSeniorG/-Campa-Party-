using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ReadyPlayer : MonoBehaviour
{
    [SerializeField] private Image playerImage, playerCheckboxImage;
    [SerializeField] private GameObject readyPlayer;
    [SerializeField] private LeaderboardManager manager;

    private TextMeshProUGUI readyText;
    private AudioSource audioSource;
    private PlayerInput playerInput;

    private int deviceID;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>();
        readyText = readyPlayer.GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        readyPlayer.SetActive(true);
        playerInput.enabled = true;
    }
    public void SetDeviceId(int assignedDeviceID) { deviceID = assignedDeviceID; }
    public void Ready(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && callbackContext.control.device.deviceId.Equals(deviceID))
        {
            //CAMBIAMOS EL CHECKBOX I EL TEXTO DE LISTO AL COLOR DEL JUGADOR
            playerCheckboxImage.color = playerImage.color;
            readyText.color = playerImage.color;
            readyText.text = "OK";

            //INDICAMOS AL MANAGER QUE EL JUADOR ESTA LISTO
            manager.PlayerReady();

            //SFX
            audioSource.Play(); 

            //DESTRUIMOS
            Destroy(this);
        }
    }
}
