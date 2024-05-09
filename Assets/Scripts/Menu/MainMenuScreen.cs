using UnityEngine;
using UnityEngine.InputSystem;


public class MainMenuScreen : MonoBehaviour
{

    private int currentOption = 0;
    private int currentMenu = 0;
    private int optionsAmount = 0;
     
    [SerializeField] private Animator cameraAnimator;
    private PlayerInput playerInput;
    [SerializeField] private VolumeController musicVolume, sfxVolume;
    private AudioSource audioSource;
    [SerializeField] private Menu[] menus;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        ChangeMenu(0);
        playerInput.enabled = true;
    }

    public void SelectOption(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            switch (currentMenu)
            {
                case 0: //MENU PRINCIPAL
                    switch (currentOption)
                    {
                        case 0: //PLAY
                            ChangeMenu(3);
                            cameraAnimator.SetTrigger("next");
                            playerInput.enabled = false;
                            break;
                        case 1: //SETTINGS
                            ChangeMenu(1);
                            break;
                        case 2: //EXIT
                            Debug.Log("HAS SALIDO DEL JUEGO");
                            Application.Quit();
                            break;
                        default:
                            break;
                    }
                    break;
                case 1: //MENU DE SETTINGS
                    switch (currentOption)
                    {
                        case 0: //VOLUMEN MUSICA
                            musicVolume.ModifyVolume((int)playerInput.actions["UI/MenuNavHor"].ReadValue<float>());
                            break;
                        case 1: //VOLUMEN SFX
                            sfxVolume.ModifyVolume((int)playerInput.actions["UI/MenuNavHor"].ReadValue<float>());
                            break;
                        case 2: //CREDITOS
                            ChangeMenu(2);
                            break;
                        case 3: //VOVLER ATRÁS
                            ChangeMenu(0);
                            break;
                        default:
                            break;
                    }
                    break;
                case 2: //MENU CREDITOS
                    switch (currentOption)
                    {
                        default: //VOLVER ATRÁS
                            ChangeMenu(1);
                            break;
                    }
                    break;
                default:
                    break;
            }
            //REPRODUCIR SFX
            audioSource.Play();
        }
    }

    public void ChangeOption(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            //OBTENER INT
            int menuNavValue = (int)playerInput.actions["UI/MenuNavVer"].ReadValue<float>();

            //LIMITAR LAS OPCIONES
            currentOption = Mathf.Clamp(currentOption += menuNavValue, 0, optionsAmount);

            //INDICAR OPCION ACTUAL
            SetOptionIndicator();

            //REPRODUCIR SFX
            audioSource.Play();
        }
    }

    private void SetOptionIndicator()
    {
        //MOSTRAR OPCION SELECCIONADA
        for (int i = 0; i < menus[currentMenu].OptionsOutline.Length; i++)
        {
            menus[currentMenu].OptionsOutline[i].SetActive(currentOption == i);
        }
    }

    private void ChangeMenu(int menuToChange)
    {
        //DESACTIVAMOS TODOS LOS MENUS EXCEPTO AL QUE CAMBIAMOS
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].MenuView.SetActive(i == menuToChange);
        }

        //ACTUALIZAMOS EL INDICE I LAS OPCIONES
        currentMenu = menuToChange;
        optionsAmount = menus[menuToChange].OptionsAmount;
        currentOption = 0;

        SetOptionIndicator();
    }

    //---------------------------------------------------------------
    //CLASE MENU
    [System.Serializable]
    private class Menu
    {
        [SerializeField] private int optionsAmount;
        [SerializeField] private GameObject menuView;
        [SerializeField] private GameObject[] optionsIndicator;

        public int OptionsAmount
        {
            get { return optionsAmount; }
            set { optionsAmount = value; }
        }

        public GameObject MenuView
        {
            get { return menuView; }
            set { menuView = value; }
        }

        public GameObject[] OptionsOutline
        {
            get { return optionsIndicator; }
            set { optionsIndicator = value; }
        }
    }
}
