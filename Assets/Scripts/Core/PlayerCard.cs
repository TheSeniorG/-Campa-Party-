using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] private Image playerBackground;
    [SerializeField] private Image playerIcon;
    [SerializeField] private TextMeshProUGUI playerName;

    public void SetPlayerColor(Color newColor){playerBackground.color = newColor;}
    public void SetPlayerIcon(Sprite icon){playerIcon.sprite = icon;}
    public void SetPlayerName(string name) { playerName.text = name; }
}