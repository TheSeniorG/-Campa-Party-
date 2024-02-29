using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // SINGLETON INSTANCE
    public static PlayerManager Instance;

    // LISTA DE JUGADORES
    private List<Player> players;

    private void Awake()
    {
        // SINGLETON
        if (Instance == null){Instance = this;}
        else{Destroy(gameObject);}

        // NO DESTRUIR AL CARGAR NUEVAS ESCENAS
        DontDestroyOnLoad(gameObject);

        // INICIALIZAR LA LISTA DE JUGADORES
        players = new List<Player>();
    }

    public int GetPlayerAmount(){return players.Count;}

    public Player GetPlayer(int index){return players[index];}

    public void AddPlayer(Player newPlayer)
    {
        //COMPROBAMOS SI YA EXISTE EL JUGADOR
        foreach (Player p in players) 
        {
            if(p.PlayerID == newPlayer.PlayerID)
            {
                //SI EL JUGADOR YA ESTABA REGISTRADO SOLO SE ACTUALIZA
                UpdatePlayer(newPlayer);
                return;
            }
        }
        //SI NO EXISTE UN JUGADOR CON ESA ID SE REGISTRA UNO NUEVO
        players.Add(newPlayer);
        Debug.Log("NUEVO JUGADOR AÑADIDO");
        PrintPlayerCharacteristics();
    }

    public void RemovePlayer(Player playerToRemove){players.Remove(playerToRemove);}

    public void UpdatePlayer(Player newPlayer)
    {
        if (players != null && newPlayer.PlayerID < players.Count)
        {
            //REASIGNAR EL JUGADOR CON NUEVAS CARACTERÍSTICAS
            players[newPlayer.PlayerID] = newPlayer;
        }
        Debug.Log("JUGADOR ACTUALIZADO");
        PrintPlayerCharacteristics();
    }

    public void IncreasePlayerScore(int id, int newScore){players[id].Score += newScore;}
    //----------------------------------------------------------------------------------------------

    // CLASE INTERNA PARA REPRESENTAR UN JUGADOR
    public class Player
    {
        public int PlayerID { get; set; }
        public int Score { get; set; } = 0;
        public Sprite PlayerIcon { get; set; }
        public Color PlayerColor { get; set; } = Color.white;

        public Player(int playerID, Color playerColor, Sprite playerIcon)
        {
            PlayerID = playerID;
            PlayerColor = playerColor;
            PlayerIcon = playerIcon;
        }
    }
    // TEST
    public void PrintPlayerCharacteristics()
    {
        foreach (Player player in players)
        {
            Debug.Log($"PlayerID: {player.PlayerID}, IconName: {player.PlayerIcon.name}, Score: {player.Score}, Color: {player.PlayerColor}");
        }
    }
}