using UnityEngine.InputSystem;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerChalk : MonoBehaviour
{
    //CORE
    [SerializeField] private TextMeshProUGUI playerScore;
    [SerializeField] private float wearRate = 0.1f;
    [SerializeField] private float movSpeed = 5.0f;
    [SerializeField] private int playerID;

    private int deviceID;
    private float score = 0;
    private bool playerAlive = true;
    private bool gameOn;
    private float hMov = 0f, vMov = 0f;

    //PARTICULAS
    private EmissionModule emissionModule;
    private readonly bool emissionModuleInitialized = true;

    //COMPONENETES EXTRA
    private MeshRenderer mRenderer;
    private ParticleSystem chalkPaint, chalkDestroy;
    private Transform l_Limit, r_Limit, t_Limit, b_Limit;
    [SerializeField] private ChalkRaceManager chalkRaceManager;

    //INPUT SYSTEM
    private PlayerInput playerInput;
    private Vector2 movementInput;
    private InputDevice playerInputDevice;

    //AUDIO
    [SerializeField] private AudioClip death_SFX, damage_SFX;
    private AudioSource audioSource;
    private void Awake()
    {
        //CUIDADO QUE EL GAME MANAGER USA ESTE SCRIPT ANTES DE QUE SE INICIALIZEN ALGUANS VARAIBLES (COLOR)
        mRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        chalkPaint = transform.GetChild(1).GetComponent<ParticleSystem>();
        chalkDestroy = transform.GetChild(2).GetComponent<ParticleSystem>();
        playerInput = GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        //GET ZONE LIMITS
        l_Limit = GameObject.Find("L_Limit").transform;
        r_Limit = GameObject.Find("R_Limit").transform;
        b_Limit = GameObject.Find("B_Limit").transform;
        t_Limit = GameObject.Find("T_Limit").transform;

        //INPUT SYSTEM
        playerInputDevice = InputSystem.GetDeviceById(deviceID);
    }

    public void AssignDeviceID(int assignedID){ deviceID = assignedID; }

    public void SetChalkColor(Color playerColor)
    {
        //PONER TIZA DEL COLOR DEL PLAYER
        mRenderer.material.color = playerColor;
        emissionModule = chalkPaint.emission;

        //PONER COLOR DE PARTICULAS
        //PARA ACCEDER AL PARTICLE HAY QUE GUARDAR EL MAIN MODULE
        var mainModule = chalkPaint.main;
        mainModule.startColor = playerColor;
    }

    void Update()
    {
        if (gameOn)
        {
            playerAlive = transform.localScale.z >= 0.1f;

            if (!playerAlive)
            {
                //QUITAR PARTICULAS DE RASTRO
                emissionModule.enabled = false;
                //PARTICULAS DE DESTRUCCIÓN
                chalkDestroy.Play();
                //DESACTIVAR RENDERER
                mRenderer.enabled = false;
                //INDICAR AL MANAGER QUE UN JUGADOR HA MUERTO
                chalkRaceManager.EliminatePlayer(playerID, ((int)score));

                //SFX
                audioSource.PlayOneShot(death_SFX);

                Destroy(this);
            }
            else
            {
                UpdatePlayerScale();
                UpdateScore();
                MovePlayer();
            }
        }
    }

    public void MovePlayer()
    {
        //COMPARAR ID DEL CONTROLADOR
        if (playerInputDevice.deviceId == deviceID)
        {
            //GUARDAR ENTRADA DE MOVIMIENTO
            movementInput = playerInput.actions["Movement"].ReadValue<Vector2>();

            hMov = movementInput.x;
            vMov = movementInput.y;
        }

        //CALUCLO DE NUEVA POS
        Vector3 newPos = transform.position + new Vector3(hMov * (movSpeed * Time.deltaTime), vMov * (movSpeed * Time.deltaTime), 0);

        //LIMITAR POSICION
        newPos.x = Mathf.Clamp(newPos.x, l_Limit.position.x, r_Limit.position.x);
        newPos.y = Mathf.Clamp(newPos.y, b_Limit.position.y, t_Limit.position.y);

        //APLICAR
        transform.position = newPos;
    }

    void UpdatePlayerScale()
    {
        //IR DECREMENTANDO LA ESCALA
        Vector3 newScale = transform.localScale;
        newScale.z -= (wearRate + Mathf.Abs(hMov / 100) + Mathf.Abs(vMov / 100)) * Time.deltaTime;

        //LIMITAR ESCALA
        newScale.z = Mathf.Clamp(newScale.z, 0.01f, 1);
        //APLICAR
        transform.localScale = newScale;
    }

    void UpdateScore()
    {
        //AÑADIR PUNTUACION
        score += 1 + (Mathf.Abs(hMov) + Mathf.Abs(vMov));
        //ACTUALIZAR SCORE
        playerScore.text = score.ToString("F0") + " pts";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && playerAlive)
        {
            // DESACTIVAMOS LA EMISIÓN DEL SISTEMA DE PARTÍCULAS
            emissionModule.enabled = false;
            //CREAMOS VECTOR DE DAÑO
            Vector3 damageScale = Vector3.zero;
            //BUSCAMOS EL DAÑO QUE HACE I SE LO RESTAMOS A NUESTRA ESCALA
            damageScale.z += other.GetComponent<ChalkObstacle>().GetDamage();
            //QUITAMOS ESCALA
            transform.localScale -= damageScale;
            //Debug.Log("ESCALA REDUCIDA");
            //SFX
            audioSource.PlayOneShot(damage_SFX);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && playerAlive)
        {
            if (emissionModuleInitialized)
            {
                //REACTIVAMOS LA EMISIÓN DEL SISTEMA DE PARTÍCULAS
                emissionModule.enabled = true;
            }
        }
    }

    public void ToggleGame(bool newState) 
    {
        gameOn = newState;

        //ACTIVAMOS LAS PARTICULAS
        chalkPaint.Play();
    }
}