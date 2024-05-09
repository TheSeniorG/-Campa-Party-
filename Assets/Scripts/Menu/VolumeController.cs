using UnityEngine;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private GameObject[] bars;
    [SerializeField] private AudioMixer audioMixer;

    private const float MAX_volume = 10f;
    private const float MIN_volume = 0f;

    [SerializeField]private float currentVolume;

    private void Start(){ModifyVolume(0);}

    public void ModifyVolume(int value)
    {
        //AJUSTAR AL RANGO PERMITIDO
        currentVolume = Mathf.Clamp(currentVolume + value, MIN_volume, MAX_volume);
        //Debug.Log("VOLUMEN ACTUAL: " + currentVolume);

        //MODIFICAR EL AUDIOMIXER
        float normalizedVolume = currentVolume / MAX_volume;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(normalizedVolume) * 20);

        //ACTUALZIAR BARRAS DE VOLUMEN
        UpdateBars();
    }

    private void UpdateBars()
    {
        for (int i = 0; i < bars.Length; i++)
        {
            bars[i].SetActive(i < currentVolume);
        }
    }
}