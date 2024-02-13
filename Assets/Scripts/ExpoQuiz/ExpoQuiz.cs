using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpoQuiz : MonoBehaviour
{
    [SerializeField] private GameObject transition;
    [SerializeField] private TextMeshPro slideIndex;

    private float slideChangeDuration;
    private int currentRound = 0, maxRounds = 3;
    private int maxSlides, currentSlide;

    private void Start()
    {
        //INICIA LA EXPO
        StartExpoQuiz();
    }

    private void StartExpoQuiz()
    {
        //EMPIEZA EN RONDA 0 I VA AUMENTADO
        currentRound = 0;
        NextRound();
    }

    private void GenerateSlides()
    {
        currentSlide = 1;
        CalculateSlideParameters();

        StartCoroutine(NextSlide());
    }

    private void CalculateSlideParameters()
    {
        switch (currentRound)
        {
            case 1:
                maxSlides = Random.Range(3, 7);
                slideChangeDuration = 2.75f;
                break;
            case 2:
                maxSlides = Random.Range(4, 8);
                slideChangeDuration = 2f;
                break;
            case 3:
                maxSlides = Random.Range(5, 9);
                slideChangeDuration = 1.25f;
                break;
            default:
                break;
        }
        Debug.Log("CANTIDAD DE DIAPOSITIVAS EN ESTA RONDA: " + maxSlides);
        Debug.Log("TIEMPO ENTRE DIAPOSITIVAS: " + slideChangeDuration);
    }

    private IEnumerator NextSlide()
    {

        transition.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        transition.SetActive(false);


        //SI LA RONDA ES SUPERIOR A LA 1RA, LOS INDICES PUEDEN NO MOSTRARSE CORRECTAMENTE
        if (currentRound > 1)
        {
            int showIndex = Random.Range(0, 3);

            if(showIndex == 0){slideIndex.text = "?";}
            else{ slideIndex.text = currentSlide.ToString();}
        }
        else
        {
            //ACTUALIZAMOS EL INDICE DE SLIDE
            slideIndex.text = currentSlide.ToString();
        }
        yield return new WaitForSeconds(slideChangeDuration);

        if (currentSlide < maxSlides)
        {
            currentSlide++;
            StartCoroutine(NextSlide());
        }
        else
        {
            NextRound();
        }
    }

    private void NextRound()
    {
        if (currentRound < maxRounds)
        {
            currentRound++;
            Debug.Log("COMENZANDO LA RONDA: " + currentRound);
            Invoke("GenerateSlides",3f);
        }
    }
}