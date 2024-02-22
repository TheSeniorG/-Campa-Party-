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
            if(p.playerID == newPlayer.playerID)
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
        if (players != null && newPlayer.playerID < players.Count)
        {
            //REASIGNAR EL JUGADOR CON NUEVAS CARACTERÍSTICAS
            players[newPlayer.playerID] = newPlayer;
        }
        Debug.Log("JUGADOR ACTUALIZADO");
        PrintPlayerCharacteristics();
    }

    public void IncreasePlayerScore(int id, int newScore){players[id].score += newScore;}
//----------------------------------------------------------------------------------------------

    // CLASE INTERNA PARA REPRESENTAR UN JUGADOR
    public class Player
    {
        public int playerID;
        public Sprite playerIcon;
        public Color playerColor = Color.white;
        public int score = 0;

        public Player(int playerID, Color playerColor, Sprite playerIcon)
        {
            this.playerID = playerID;
            this.playerColor = playerColor;
            this.playerIcon = playerIcon;
        }
    }

    // TEST
    public void PrintPlayerCharacteristics()
    {
        foreach (Player player in players)
        {
            Debug.Log($"PlayerID: {player.playerID}, IconName: {player.playerIcon.name}, Score: {player.score}, Color: {player.playerColor}");
        }
    }
}