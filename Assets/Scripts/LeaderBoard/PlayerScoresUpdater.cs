using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoresUpdater : MonoBehaviour
{
    //ESTO ES BASTANTE FEO POR MI PARTE, MIRAR DE CAMBIARLO EN EL FUTURO -E

    //SCRIPT UTILIZADO EN EL LEADERBOARD MANAGER PARA QUE ESTE ACTUALIZE
    //LAS PUNTUACIONES EN LOS JUGADORES
    [SerializeField] private LeaderboardManager leaderboardManager;

    public void CallTempScoreUpdate() { leaderboardManager.UpdateTempScores(); }
    public void CallMaxScoreUpdate() { leaderboardManager.UpdateMaxScores(); }
    public void UpdateCardsPositions() { leaderboardManager.UpdatePlayersPositions(); }
}
