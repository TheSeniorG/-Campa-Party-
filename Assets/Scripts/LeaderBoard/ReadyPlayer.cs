using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyPlayer : MonoBehaviour
{
    //SCRIPT UTILZIADO EN EL LEADERBOARD PARA INDICAR QUE TODOS LOS JUGAORES ESTAN LISTOS
    [SerializeField] private Image playerImage;
    [SerializeField] private GameObject readyTxt;
    private LeaderboardManager manager;

    [SerializeField] private Image playerCheckboxImage;
    private void Start()
    {
        manager = GameObject.Find("LeaderboardManager").GetComponent<LeaderboardManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //CAMBIAMOS EL HECKBOX DE LISTO AL COLOR DEL JUGADOR
            playerCheckboxImage.color = playerImage.color;
            //TEXTO READY
            readyTxt.SetActive(true);
            //INDICAMOS AL MANAGER QUE EL JUADOR ESTA LISTO
            manager.PlayerReady();

            //DESTRUIMOS, ASI NO USA EL UPDATE
            Destroy(this);
        }
    }
}
