using System.Collections;
using TMPro;
using UnityEngine;

public class MinigameSelector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI minigameName, minigameDesc;
    [SerializeField] private GameObject fade, selectionLockedEffect;
    [SerializeField] private UIFadeController fadeController;
    private bool startSelection = true;

    private float currentTick = 0f,previousTick = 0f;
    private float startSpeed;
    private int minigameSelected = 0;

    //MINIJUEGOS
    private Minigame chalkRace = new Minigame("Chalk Race", "Race through an infinite chalkboard with your chalk, skillfully dodging obstacles to score as high as possible in this thrilling and competitive experience!",0);
    private Minigame basketPaper = new Minigame("Basket Paper", "Aim, toss, and score in this minigame by landing your paper balls in strategically placed baskets while avoiding obstacles that may alter your trajectory.",1);
    private Minigame expoQuiz = new Minigame("Expo-Quiz Adventure", "Dive into the world of knowledge and fun with an exciting trivia challenge set within a virtual expo where you put your perception to the test.", 2);
    private Minigame madTower = new Minigame("Mad Tower", "Dive into the world of knowledge and fun with an exciting trivia challenge set within a virtual expo where you put your perception to the test.", 3);

    //DECLARAMOS ARRAY PERO NO LO USAMOS AUN
    private Minigame[] minigames;

    void Start()
    {
        Debug.LogWarning("RECORDATORIO: FALTA IMPLEMENTAR LA CLASE DE LOS MINIJUEGOS CON SU INFO");

        //ASSIGANMOS LOS VALORES AL ARRAY
        minigames = new Minigame[] {chalkRace,basketPaper,expoQuiz, madTower};

        startSpeed = Random.Range(50f, 200f);
        previousTick = currentTick + 1;
    }

    void Update()
    {
        if (startSelection)
        {
            //SUMAMOS EL VALOR DE VELOCIDAD QUE IRA DISMINUYENDO GRADUALMENTE
            currentTick += startSpeed;

            if (currentTick >= previousTick){ChangeMinigame(minigameSelected);}
            else { Invoke("LockMinigame", 0.75f); }
        }
    }

    public void StartSelection(){startSelection = true;}

    private void ChangeMinigame(int minigame)
    {
        //CANCELAMOS LAS LLAMADES A BLOQUEAR EL MINIJUEGO
        CancelInvoke();
        //CAMBIAMOS TITULO I DESCRIPCION
        minigameName.text = minigames[minigame].name;
        minigameDesc.text = minigames[minigame].description;

        //DISMINUIMOS VELOCIDAD
        startSpeed *= .9f;
        previousTick = currentTick + 1f;

        //SI LLEGA AL FINAL DEL INDICE DEL ARRAY VUELVE A 0
        minigameSelected = (minigameSelected + 1) % minigames.Length;
    }
    private void LockMinigame()
    {
        //CANCELO INVOKES POR SI ACASO
        CancelInvoke();
        //ANULAMOS EL UPDATE
        startSelection = false;
        //EFFECTO SELECCION
        selectionLockedEffect.SetActive(true);

        //FALTA IMPLEMENTAR
        //ACTIVAMOS EL FADE PARA CAMBIO DE ESCENA CON EL MINIJUEGO SELECCIONADO
        /*
        fade.SetActive(true);
        fadeController.SetSceneName(minigames[minigameSelected].levelName);
        */
    }
    private class Minigame
    {
        public string name;
        public string description;
        public int minigameImageID;
        //public string levelName;

        public Minigame(string name, string description /* string sceneName*/, int minimgameImageID)
        {
            this.name = name;
            this.description = description;
            this.minigameImageID = minimgameImageID;
            //this.levelName = sceneName;
        }
    }
}