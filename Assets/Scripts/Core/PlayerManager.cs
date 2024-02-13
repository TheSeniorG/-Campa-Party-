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

    // OBTENER LA LONGITUD DE LA LISTA DE JUGADORES
    public int GetListLength(){return players.Count;}

    // OBTENER UN JUGADOR POR ÍNDICE
    public Player GetPlayer(int index){return players[index];}

    // AÑADIR UN NUEVO JUGADOR A LA LISTA
    public void AddPlayer(Player newPlayer){players.Add(newPlayer);}

    // ELIMINAR UN JUGADOR DE LA LISTA
    public void RemovePlayer(Player playerToRemove){players.Remove(playerToRemove);}

    // ACTUALIZAR LA INFORMACIÓN DEL JUGADOR
    public void UpdatePlayerInfo(int playerID, Color color, Sprite newSprite)
    {
        if (players != null && playerID < players.Count)
        {
            // CREAR Y REASIGNAR EL JUGADOR CON NUEVAS CARACTERÍSTICAS
            Player updatedPlayer = new Player(playerID, color, newSprite);
            players[playerID] = updatedPlayer;
            PrintPlayerCharacteristics();
        }
    }

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