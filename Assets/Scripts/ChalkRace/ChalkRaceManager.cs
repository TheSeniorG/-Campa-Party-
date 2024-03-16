    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using static UnityEditor.Experimental.GraphView.GraphView;

    public class ChalkRaceManager : MonoBehaviour
    {
        [SerializeField] private ObstacleGenerator obstacleGenerator;
        [SerializeField] private ScrollMatOffset chalkboardMatSlider;
        [SerializeField] private GameObject fade;
        [SerializeField] private GameObject[] players, playersCards;

        private List<PlayerChalk> playerChalks = new List<PlayerChalk>();
        private int playersRemaining;
        private PlayerManager playerManager;
        private Dictionary<int, int> playersScore = new Dictionary<int, int>();

    //SIEMPRE HAY 1 JUGADOR
    //SERIALIZADO DURANTE TESTEO
    [Header("SOLO TESTEO")]
        [SerializeField] [Range(1,4)]private int playerAmount = 1;

        private void Start()
        {
            //COMPROBAR QUE EXISTA PLAYER MANAGER
            if (GameObject.Find("PlayerManager"))
            {
                playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

                //OBTENEMOS LA LISTA DE JUGADORES
                playerAmount = playerManager.GetPlayerAmount();
            }
            else { Debug.LogWarning("NO SE HA ENCONTRADO PLAYER MANAGER"); }

            playersRemaining = playerAmount;

            for (int i = 0; i < playerAmount; i++)
            {
                //ACTIVAR JUGADORES
                players[i].SetActive(true); 
                playersCards[i].SetActive(true);

                //GUARDAR COMPONENTE DEL PLAYER EN LA LISTA
                //playerChalks.Add(players[i].GetComponent<PlayerChalk>());

                if (playerManager != null)
                {
                    //ESTABLCEMOS DISE�O DE LA TARJETA
                    PlayerCard card = playersCards[i].GetComponent<PlayerCard>();
                    card.SetPlayerColor(playerManager.GetPlayer(i).PlayerColor);
                    card.SetPlayerIcon(playerManager.GetPlayer(i).PlayerIcon);
                
                    //CAMBIAR COLOR TIZA
                    playerChalks[i].SetChalkColor(playerManager.GetPlayer(i).PlayerColor);
                }
                players[i].GetComponent<PlayerChalk>().SetChalkColor(Color.blue);
            }
            // INICIA LA CORRUTINA PARA HABILITAR EL JUEGO
            StartCoroutine(SetGame(13f));

            // CADA 5 SEGUNDOS AUMENTA EL SPAWN RATIO DE OBST�CULOS
            InvokeRepeating(nameof(IncreaseDifficulty), 12f, 5f);
        }

        //AUMENTA EL SPAWN RATE DE OBSTACULOS
        private void IncreaseDifficulty(){if(obstacleGenerator != null)obstacleGenerator.IncreaseSpawnRate(0.25f);}

        public IEnumerator SetGame(float waitCall = 0f)
        {
            // ESPERA ANTES DE REALIZAR CAMBIOS
            yield return new WaitForSeconds(waitCall);

            bool newState = playersRemaining >0;

            // HABILITA EL SPAWNER DE OBSTACULOS
            obstacleGenerator.enabled = newState;

            // HABILITAR BOLEANOS DEL JUGADOR
            foreach (PlayerChalk playerChalk in playerChalks)
                playerChalk.ToggleGame(newState);

            //HACER QUE LA PIZARRA SE DESPLACE
            chalkboardMatSlider.enabled = true;
        }

        public void EliminatePlayer(int playerId, int score)
        {
            // DECREMENTA EL N�MERO DE JUGADORES RESTANTES
            playersRemaining--;

            if(playerManager != null)
            {
                //GUARDAR LA PUNTUACION HECHA POR EL JUGADOR
                playersScore.Add(playerId, score);
            }

            // SI NO HAY JUGADORES RESTANTES, DESACTIVA EL JUEGO
            if (playersRemaining == 0) 
            {
                if(playerManager != null)
                {
                    //ORDENAR PUNTUACIONES DEL JUGADOR
                    playersScore = playersScore.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                    for (int i = 0; i < playerAmount; i++)
                    {
                        //ESTABLECER NUEVAS PUNTUACIONES
                        playersScore[i] = playerAmount - i;
                        //ACTUALIZARLAS EN EL PLAYER MANAGER
                        int dictionaryKey = playersScore.ElementAt(i).Key;
                        playerManager.IncreasePlayerScore(dictionaryKey,playerAmount-i);
                    }
                }

                //DESACTIVAR CONTROLES
                StartCoroutine(SetGame());
                StartCoroutine(Fade(3f));
            }
        }
        private IEnumerator Fade(float waitTime = 0f)
        {
            yield return new WaitForSeconds(waitTime);
            fade.SetActive(true);
        }
    }
