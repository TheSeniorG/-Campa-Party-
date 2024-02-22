using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] private Image playerBackground;
    [SerializeField] private Image playerIcon;

    public void SetPlayerColor(Color newColor){playerBackground.color = newColor;}
    public void SetPlayerIcon(Sprite icon)
    {
        //CARGA ITEM DE LA CARPETA RECURSOS HABIENDOLE PASADO EL PATH
        playerIcon.sprite = icon;
    }
}
