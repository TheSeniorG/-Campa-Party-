using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerColor : MonoBehaviour
{
    //SCRIPT QUE SIRVE PARA ASIGNAR EL COLOR DE UN JUGADOR A UN OBJETO

    [SerializeField] private bool isText, isImage, isSprite;
    [SerializeField] private int playerID;

    private TextMeshProUGUI myTxtMeshPro;
    private Image myImage;
    private SpriteRenderer mySpriteRenderer;
    private PlayerManager playerManager;

    private Color myColor;

    private void Start()
    {
        if(GameObject.Find("PlayerManager") != null)
        {
            playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

            //OBTENER COLOR DEL JUGADOR
            myColor = playerManager.GetPlayer(playerID).PlayerColor;

            if (isText)
            {
                myTxtMeshPro = GetComponent<TextMeshProUGUI>();
                myTxtMeshPro.color = myColor;
            }
            else if (isImage)
            {
                myImage = GetComponent<Image>();
                myImage.color = myColor;
            }
            else if (isSprite)
            {
                mySpriteRenderer = GetComponent<SpriteRenderer>();
                mySpriteRenderer.color = myColor;
            }
        }
    }
}
