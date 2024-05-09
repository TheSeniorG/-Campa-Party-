using System.Collections;
using TMPro;
using UnityEngine;

public class MinigameSelector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI minigameName, minigameDesc;
    [SerializeField] private TextMeshProUGUI[] controlsDescripions;
    [SerializeField] private GameObject fade, selectionLockedEffect;
    [SerializeField] private UIFadeController fadeController;
    [SerializeField] private ReadyPlayer[] readyPlayerScript;

    [Header("SONIDO")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip tick_SFX;

    private DatabaseAccess databaseAccess;
    private bool startSelection = true;
    private float currentTick = 0f,previousTick = 0f;
    private float startSpeed;
    private int minigameSelected = 0;

    //MINIJUEGOS (IR AÑADIENDO)
    private readonly Minigame chalkRace = new (0,"Chalk Race", "Race through an infinite chalkboard with your chalk, skillfully dodging obstacles to score as high as possible.",
        "ChalkRace","8Y", "UIOP");
    private readonly Minigame basketPaper = new (1,"Basket Paper", "Aim, toss, and score by landing your paper balls in strategically placed baskets while avoiding obstacles that may alter your trajectory.",
        "BasketPaper","",",");
    private readonly Minigame madTower = new (2,"Mad Tower", "Test your skill and precision in this tower-building adventure where your goal is to stack objects and reach new heights.",
        "MadTower","",",");
    private readonly Minigame codeWars = new(2, "Code Wars", "Challenge your reaction speed and agility in 'Code Wars'! Tap the buttons as they appear on screen to score as most points as you can.",
    "CodeWars", "", "UIOP", "qw");


    //DECLARAMOS ARRAY PERO NO LO USAMOS AUN
    private Minigame[] minigames;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        //ACCESO A LA BASE DE DATOS
        databaseAccess = GameObject.Find("PlayerManager").GetComponent<DatabaseAccess>();
    }
    void Start()
    {
        //ASSIGANMOS LOS VALORES AL ARRAY
        minigames = new Minigame[] {basketPaper, madTower, codeWars};
        //ALEATORIZAR VELOCIDAD DE SELECCION
        startSpeed = Random.Range(50f, 500f);
        previousTick = currentTick + 1f;
    }

    void Update()
    {
        if (startSelection)
        {
            //SUMAMOS EL VALOR DE VELOCIDAD QUE IRA DISMINUYENDO GRADUALMENTE
            currentTick += startSpeed;

            if (currentTick >= previousTick) { ChangeMinigame(); }
            else { Invoke(nameof(LockMinigame), Random.Range(0.65f,0.95f)); }
        }
    }

    private void ChangeMinigame()
    {
        //CANCELAMOS LAS LLAMADAS DE BLOQUEAR EL MINIJUEGO
        CancelInvoke();

        //DISMINUIMOS VELOCIDAD DE CAMBIO DE MINIJUEGO
        startSpeed *= .9f;
        previousTick = currentTick + 1f;

        //SI LLEGA AL FINAL DEL INDICE DEL ARRAY VUELVE A 0
        minigameSelected = (minigameSelected + 1) % minigames.Length;

        //Debug.Log("Minigame Index:" + minigameSelected + minigames[minigameSelected].LevelName);

        //CAMBIAMOS TITULO I DESCRIPCION EN EL PANEL
        minigameName.text = minigames[minigameSelected].Name;
        minigameDesc.text = minigames[minigameSelected].Description;

        //CONTROLES DE LOS MINIJUEGOS
        controlsDescripions[0].text = minigames[minigameSelected].Control1Input; 
        controlsDescripions[1].text = minigames[minigameSelected].Control2Input; 
        controlsDescripions[2].text = minigames[minigameSelected].Control3Input;

        //SFX
        audioSource.PlayOneShot(tick_SFX);
    }
    private void LockMinigame()
    {
        //CANCELO INVOKES
        CancelInvoke();

        //ANULAMOS EL UPDATE
        startSelection = false;

        //EFFECTO SELECCION
        selectionLockedEffect.SetActive(true);

        //ACTIVAMOS QUE LOS JUGADORES PUEDEN PREPARARSE
        foreach(ReadyPlayer rp in readyPlayerScript){rp.enabled = true;}
    }
    public void StartMinigame()
    {
        //ACTIVAMOS EL FADE PARA CAMBIO DE ESCENA CON EL MINIJUEGO SELECCIONADO
        fadeController.SetSceneName(minigames[minigameSelected].LevelName);
        fade.SetActive(true);

        //REGISTRAMOS MINIJUEGO EN LA BASE DE DATOS
        if(databaseAccess != null) { databaseAccess.SetMinigameStart(minigames[minigameSelected].LevelName); }
    }

    private class Minigame
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinigameImageID { get; set; }
        public string LevelName { get; set; }

        public string Control1Input { get; set; }
        public string Control2Input { get; set; }
        public string Control3Input { get; set; }

        public Minigame(int minigameImageID, string name, string description, string levelName,
            string newInput1 = "", string newInput2 = "", string newInput3 = "")
        {
            Name = name;
            Description = description;
            MinigameImageID = minigameImageID;
            LevelName = levelName;

            Control1Input = newInput1;
            Control2Input = newInput2;
            Control3Input = newInput3;
        }
    }
}