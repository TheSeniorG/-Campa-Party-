using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PlayerManager : MonoBehaviour
{
    // SINGLETON INSTANCE
    public static PlayerManager Instance;

    // LISTA DE JUGADORES
    private List<Player> players;

    private void Awake()
    {
        // SINGLETON
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return; // Agregamos este return para salir del método Awake si ya hay una instancia existente
        }

        // NO DESTRUIR AL CARGAR NUEVAS ESCENAS
        DontDestroyOnLoad(gameObject);

        // INICIALIZAR LA LISTA DE JUGADORES
        players = new List<Player>();
    }

    public int GetPlayerAmount() { return players.Count; }

    public Player GetPlayer(int index) { return players[index]; }

    public void AddPlayer(Player newPlayer)
    {
        //COMPROBAMOS SI YA EXISTE EL JUGADOR
        for(int i = 0; i < players.Count; i++)
        {
            //SI EL JUGADOR YA ESTABA REGISTRADO SOLO SE ACTUALIZA
            if (players[i].PlayerID == newPlayer.PlayerID)
            {
                UpdatePlayer(newPlayer, i);
                //Debug.Log("EL JUGADOR " + newPlayer.PlayerID + " YA EXISTE");
                return;
            }
        }
        //SI NO EXISTE UN JUGADOR CON ESA ID SE REGISTRA UNO NUEVO
        players.Add(newPlayer);
        Debug.Log("NUEVO JUGADOR AÑADIDO");
        PrintPlayerCharacteristics();
    }

    public void RemovePlayer(Player playerToRemove) { players.Remove(playerToRemove); Debug.Log("JUGADOR ELIMINADO"); }

    public void UpdatePlayer(Player newPlayer, int playerIndex)
    {
        //REASIGNAR EL JUGADOR CON NUEVAS CARACTERÍSTICAS
        players[playerIndex] = newPlayer;
        Debug.Log("JUGADOR ACTUALIZADO");
        PrintPlayerCharacteristics();
    }

    public void IncreasePlayerScore(int id, int newScore)
    {
        //GUARDAMOS LA PUNTUACION ANTIGUA I LA OBTENIDA PARA EL LEADERBOARD
        players[id].OldScore = players[id].Score;
        players[id].ScoreObatined = newScore;

        players[id].Score += newScore;
    }

    //----------------------------------------------------------------------------------------------

    // CLASE INTERNA PARA REPRESENTAR UN JUGADOR
    public class Player
    {
        //ID
        public int DeviceID { get; set; }
        public int PlayerID { get; set; }
        //SCORE
        public int Score { get; set; } = 0;
        public int OldScore { get; set; } = 0;
        public int ScoreObatined { get; set; } = 0;
        //VISUAL
        public Sprite PlayerIcon { get; set; }
        public Color PlayerColor { get; set; } = Color.white;
        public string PlayerName { get; set; }

        public Player(int playerID, Color playerColor, Sprite playerIcon, int deviceID)
        {
            //EL ID DEL JUGADOR ES EL MISMO QUE EL ID DEL MANDO
            PlayerID = playerID;
            DeviceID = deviceID;
            PlayerColor = playerColor;
            PlayerIcon = playerIcon;
            PlayerName = $"PLAYER {PlayerID}";
        }
    }
    // TESTING
    public void PrintPlayerCharacteristics()
    {
        foreach (Player player in players)
        {
            Debug.Log($"PlayerID: {player.PlayerID}, IconName: {player.PlayerIcon.name}, Score: {player.Score}, Color: {player.PlayerColor}, Device ID: {player.DeviceID}");
        }
    }
    /*private void OnGUI()
    {
        GUILayout.Label("Lista de Jugadores:");

        foreach (Player player in players)
        {
            GUILayout.Label($"PlayerID: {player.PlayerID}, Score: {player.Score}");
        }
    }
    */
}