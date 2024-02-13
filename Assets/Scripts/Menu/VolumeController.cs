using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private GameObject[] bars;

    private const int MAX_volume = 10, MIN_volume = 0;
    private float currentVolume;
    private AudioSource audioSource;

    private void Start()
    {
        currentVolume = MAX_volume; 
        UpdateBars();
        audioSource = Camera.main.GetComponent<AudioSource>();

        Debug.Log("RECORDATORIO: FALTA PLANTEAR EL SISTEMA DE AUDIO");
    }


    public void ModifyVolume(int value)
    {
        
        //CLAMP DE MAX I MIN
        currentVolume = Mathf.Clamp(currentVolume + value, MIN_volume, MAX_volume);
        Debug.Log(currentVolume);

        //FALTA PLATEAR QUE AUDIO SYSTEMS SE USARÁ
        //MODIFY VOLUME
        //audioSource.volume = currentVolume / 10;
        

        //PONER BARRAS
        UpdateBars();
    }
    private void UpdateBars()
    {
        for (int i = 0; i < bars.Length; i++){bars[i].SetActive(i < currentVolume);}
    }
}
