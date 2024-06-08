using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Codec : MonoBehaviour
{
    private readonly char[] codecChars = new char[] { 'P', 'O', 'U', 'I' };

    [SerializeField] private TextMeshProUGUI codecText;
    [SerializeField] private TextMeshProUGUI correctText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject flashEffect;

    private int codecLength = 5;
    private int deviceID;
    private int score = 0;
    private int currentIndex = 0;

    private string codecCode;
    private string playerCodec;

    private Animator animator;

    private bool canWrite = false;

    //AUDIO
    private AudioSource audioSource;
    [SerializeField] private AudioClip resetCode_SFX, correct_SFX, incorrect_SFX;

    private void Awake(){animator = GetComponent<Animator>(); audioSource = GetComponent<AudioSource>(); }

    // GENERA UN NUEVO CÓDIGO DE LONGITUD ALEATORIA Y LO MUESTRA EN EL TEXTO
    // INVOCADO POR UN EVENTO DE ANIMACIÓN
    public void GenerateCodec()
    {
        ResetAnswer();

        //GENERAR CODIGO
        codecCode = "";

        //ALEATORIZAR CODIGO
        for (int i = 0; i < codecLength; i++)
        {
            int randomIndex = Random.Range(0, codecChars.Length);
            codecCode += codecChars[randomIndex];
        }

        //MOSTRAR CODIGO
        codecText.text = codecCode;
    }

    // VERIFICA LA RESPUESTA DEL JUGADOR Y ACTUALIZA EL PUNTAJE
    void CheckAnswer(char answer)
    {
        //AÑADIMOS A LA RESPUESTA DEL JUGADOR
        playerCodec += answer;

        //SI SE EQUIVOCA, SE REINICIA SU RESPUESTA
        if (playerCodec[currentIndex] == codecCode[currentIndex])
        {
            //AUMENTAMOS A LA SIGUIENTE INSTRUCCION
            currentIndex++;
            //MARCAMOS COMO CORRECTA
            correctText.text += 'O';

            //SFX
            audioSource.PlayOneShot(correct_SFX);

            //SI EL PLAYER YA HA SUPEARADO EL LIMITE, SUMA PUNTO
            if (currentIndex >= codecCode.Length)
            {
                //AUMENTAR LA PUNTUACION
                score++;
                scoreText.text = score.ToString();

                //QUE NO PUEDA INTERACTUAR EN UN RATO
                canWrite = false;

                //EFECTO VISUAL
                flashEffect.SetActive(true);

                //SIGUIENTE ANIMACION
                animator.SetTrigger("next");

                //SFX
                audioSource.PlayOneShot(resetCode_SFX);
                //Debug.Log("RESPUESTA CORRECTA");
            }
        }
        else
        {
            ResetAnswer();
            //SFX
            audioSource.PlayOneShot(incorrect_SFX);
        }
    }

    // REINICIA LA RESPUESTA DEL JUGADOR
    private void ResetAnswer()
    {
        //REINICIAR PARAMETROS
        currentIndex = 0;
        playerCodec = "";
        correctText.text = "";
        canWrite = true;

        //Debug.Log("RESPUESTA REINICIADA");
    }

    // TRANSCRIBE LA ENTRADA DEL JUGADOR Y VERIFICA LA RESPUESTA
    public void TranscribeInput(InputAction.CallbackContext callbackContext)
    {
        if (canWrite)
        {
            if (callbackContext.started && callbackContext.control.device.deviceId.Equals(deviceID))
            {
                //COMPROBAR INPUT REALIZADO
                Vector2 value = callbackContext.ReadValue<Vector2>();
                //Debug.Log(value);
                if (value.x > 0) { CheckAnswer('I'); }
                else if (value.x < 0) { CheckAnswer('P'); }
                else if (value.y > 0) { CheckAnswer('O'); }
                else if (value.y < 0) { CheckAnswer('U'); }
            }
        }
    }

    // ASIGNA EL ID DEL DISPOSITIVO AL JUGADOR
    public void AssignDeviceID(int assignedID) { deviceID = assignedID; }
    public int GetScore() { return score; }
}