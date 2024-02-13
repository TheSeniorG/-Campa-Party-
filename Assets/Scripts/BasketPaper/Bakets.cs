using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bakets : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject basketParticles;
    private int score = 0;
    private void OnTriggerEnter(Collider other)
    {
        //SE DESTRUYE EL OBJETO QUE ENTRE
        Destroy(other.gameObject);

        //SE SUMA LA PUNTACION DEL JUGADOR ASIGNADO A ESTA PAPELERA
        score++;
        scoreText.text = score.ToString();

        //EFECTO DE ENCESTAR
        Instantiate(basketParticles, transform.position, Quaternion.identity);
    }
}
