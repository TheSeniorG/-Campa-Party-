using NaughtyAttributes;
using UnityEngine;


public class LobbyInputSender : MonoBehaviour
{


    private PlayerCardConfig playerCardConfig;

    [SerializeField] private FunctionType functionType;

    [SerializeField] private string playerPrefabName;
    [SerializeField] private int nextColor = 1;
    [SerializeField] private int nextIcon = 1;

    internal enum FunctionType
    {
        ChangeIcon,
        ChangeColor,
        LockPlayer,
    }

    private void Start()
    {
        //LO LLAMO CON RETRASO PORQUE LA ANIMACIÓN DE CARGA DE TV TARDA EN ACTIVAR LOS OBJETOS
        Invoke("FindPlayerRef",0.5f);
    }

    private void OnMouseDown()
    {
        switch (functionType)
        {
            case FunctionType.ChangeIcon:
                playerCardConfig.ChangeIcon(nextIcon);
                break;
            case FunctionType.ChangeColor:
                playerCardConfig.ChangeColor(nextColor);
                break;
            case FunctionType.LockPlayer:
                playerCardConfig.LockPlayer();
                break;
            default:
                break;
        }
    }
    private void FindPlayerRef() { playerCardConfig = GameObject.Find(playerPrefabName).GetComponent<PlayerCardConfig>(); }
}