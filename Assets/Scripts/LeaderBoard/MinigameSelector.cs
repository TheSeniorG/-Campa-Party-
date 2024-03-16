using System.Collections;
using TMPro;
using UnityEngine;

public class MinigameSelector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI minigameName, minigameDesc;
    [SerializeField] private GameObject fade, selectionLockedEffect;
    [SerializeField] private UIFadeController fadeController;
    [SerializeField] private ReadyPlayer[] readyPlayerScript;

    private bool startSelection = true;
    private float currentTick = 0f,previousTick = 0f;
    private float startSpeed;
    private int minigameSelected = 0;

    //MINIJUEGOS (IR AÑADIENDO)
    private readonly Minigame chalkRace = new Minigame(0,"Chalk Race", "Race through an infinite chalkboard with your chalk, skillfully dodging obstacles to score as high as possible.","ChalkRace");
    private readonly Minigame basketPaper = new Minigame(1,"Basket Paper", "Aim, toss, and score by landing your paper balls in strategically placed baskets while avoiding obstacles that may alter your trajectory.","BasketPaper");
    private readonly Minigame madTower = new Minigame(2,"Mad Tower", "Test your skill and precision in this tower-building adventure where your goal is to stack objects and reach new heights.","MadTower");


    //DECLARAMOS ARRAY PERO NO LO USAMOS AUN
    private Minigame[] minigames;
    private void Awake()
    {
        minigameDesc.text = "";
        minigameName.text = "";
    }
    void Start()
    {
        //ASSIGANMOS LOS VALORES AL ARRAY
        minigames = new Minigame[] {chalkRace,basketPaper,madTower};

        startSpeed = Random.Range(50f, 300f);
        previousTick = currentTick + 1f;
    }

    void Update()
    {
        if (startSelection)
        {
            //SUMAMOS EL VALOR DE VELOCIDAD QUE IRA DISMINUYENDO GRADUALMENTE
            currentTick += startSpeed;

            if (currentTick >= previousTick) { ChangeMinigame(); }
            else { Invoke(nameof(LockMinigame), 0.75f); }
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
        
        fade.SetActive(true);
        fadeController.SetSceneName(minigames[minigameSelected].LevelName);
        
    }
    private class Minigame
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinigameImageID { get; set; }
        public string LevelName { get; set; }

        public Minigame(int minigameImageID, string name, string description, string levelName)
        {
            Name = name;
            Description = description;
            MinigameImageID = minigameImageID;
            LevelName = levelName;
        }
    }
}