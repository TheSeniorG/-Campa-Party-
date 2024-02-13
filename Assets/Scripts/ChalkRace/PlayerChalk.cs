using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerChalk : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerScore;
    [SerializeField] private float wearRate = 0.1f;
    [SerializeField] private float movSpeed = 5.0f;

    private float score = 0;
    private bool playerAlive = true,gameOn;

    private EmissionModule emissionModule;
    private bool emissionModuleInitialized = false;

    private MeshRenderer mRenderer;
    private ParticleSystem chalkPaint, chalkDestroy;
    private Transform l_Limit, r_Limit, t_Limit, b_Limit;
    private ChalkRaceManager raceManager;

    private void Start()
    {
        //GET ZONE LIMITS
        l_Limit = GameObject.Find("L_Limit").transform;
        r_Limit = GameObject.Find("R_Limit").transform;
        b_Limit = GameObject.Find("B_Limit").transform;
        t_Limit = GameObject.Find("T_Limit").transform;

        mRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        chalkPaint = transform.GetChild(1).GetComponent<ParticleSystem>();
        chalkDestroy = transform.GetChild(2).GetComponent<ParticleSystem>();
        raceManager = GameObject.Find("ChalkRaceManager").GetComponent<ChalkRaceManager>();

        //INICIALIZAR EMISSION MODULE
        SetChalkColor(mRenderer.material.color);

        emissionModuleInitialized = true;
    }

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
        if (gameOn && playerAlive)
        {
            //EL JUGADOR ESTA VIVO SIEMPRE QUE SU ESCALA SEA SUPERIOR A 0.1F
            playerAlive = transform.localScale.z >= 0.1f;

            //PLAY PARTICULAS
            chalkPaint.Play();

            //GUARDAR ENTRADA INPUT
            float hMov = Input.GetAxis("Horizontal");
            float vMov = Input.GetAxis("Vertical");

            //CALCULAR NUEVA POS
            Vector3 newPos = transform.position + new Vector3(hMov * movSpeed * Time.deltaTime, vMov * movSpeed * Time.deltaTime, 0);

            //LIMITAR LA Y
            newPos.x = Mathf.Clamp(newPos.x, l_Limit.position.x, r_Limit.position.x);
            newPos.y = Mathf.Clamp(newPos.y, b_Limit.position.y, t_Limit.position.y);

            //MOVER
            transform.position = newPos;

            // DECREMENTAR ESCALA EN FUNCION DE LA DURABILIDAD
            // SI LA TIZA SE MUEVE, AUMENTA SU DESGASTE
            //CREAR UN VECTOR CON LA ESCALA ACTUAL MENOS EL DESGASTE
            Vector3 newScale = transform.localScale;
            newScale.z -= (wearRate + Mathf.Abs(hMov / 100) + Mathf.Abs(vMov / 100)) * Time.deltaTime;

            // ASEGURAR QUE LA ESCALA NO SEA NEGATIVA
            newScale.z = Mathf.Clamp(newScale.z, 0.01f, 1);

            // ASIGNAR LA NUEVA ESCALA
            transform.localScale = newScale;

            //AÑADIR PUNTUACION
            //SI EL JUGADOR SE MUEVE, GANARA MÁS PUNTACION PERO SE GASTARÁ MAS RÁPIDO
            score += 1 + (Mathf.Abs(hMov) + Mathf.Abs(vMov));

            //ACTUALIZAR LA PUNTUACION DEL JUGADOR
            //F0 = NO MOSTRAR DECIMALES
            playerScore.text = score.ToString("F0") + " pts";

            if (!playerAlive)
            {
                if (emissionModuleInitialized){emissionModule.enabled = false;}

                //QUITAMOS PARTICULAS I PONEMOS LAS DE DESTRUCCION
                chalkDestroy.Play();
                //DESACTIVAMOS EL RENDERER
                mRenderer.enabled = false;

                raceManager.EliminatePlayer();
            }
        }
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