using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Seb.Managers
{
    public class GameManager : MonoBehaviour
    {
        public enum GameState
        {
            SpawnPlayers,
            NewTurn,
            StartTurn,
            EndTurn,
            Wait,
            EndGame,
        };

        public static GameState State;

        public static int CurrentTurn = 0;
        public static int CurrentRound;

        [SerializeField]
        private float _timeBetweenTurns = 5;

        [SerializeField]
        private UnityEvent _onStartTurn;

        [SerializeField]
        private UnityEvent _onEndTurn;

        [SerializeField]
        private List<Transform> _spawnPoints;

        [SerializeField]
        private GameObject _spawnRoot;

        [SerializeField]
        private GameObject _playerPrefab;

        private PlayerManager _activePlayer;
        private List<int> _usedSpawnPoints = new() { };
        private List<GameObject> _teamRoots = new() { };
        
        void Awake()
        {
            UpdateGameState(GameState.SpawnPlayers);
        }

        public void UpdateGameState(GameState newState)
        {
            State = newState;

            switch (newState)
            {
                case GameState.SpawnPlayers:
                    HandleSpawnPlayer();
                    break;
                case GameState.NewTurn:
                    HandleNewTurn();
                    break;
                case GameState.StartTurn:
                    HandleStartTurn();
                    break;
                case GameState.EndTurn:
                    HandleEndTurn();
                    break;
                case GameState.Wait:
                    StartCoroutine(Wait());
                    break;
                case GameState.EndGame:
                    HandleEndGame();
                    break;
                default:
                    break;
            }
        }

        private void HandleSpawnPlayer()
        {
            for (int team = 0; team < GameVariables.NumberOfTeams; team++)
            {
                Color32 teamColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0f, 1f, 1f, 1f);
                GameObject teamRoot = new("Team" + team);

                teamRoot.transform.parent = _spawnRoot.transform;
                _teamRoots.Add(teamRoot);

                for (int player = 0; player < GameVariables.PlayersPerTeam; player++)
                {
                    GameObject playerPrefab = Instantiate(_playerPrefab, _spawnPoints[GetSpawnPointIndex()].position, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                    playerPrefab.transform.parent = teamRoot.transform;

                    if (playerPrefab.TryGetComponent(out PlayerManager playerManager)) playerManager.SetupPlayer(this, teamColor);
                    else Debug.LogError("Error! No PlayerManager on player GameObject: " + player);
                }
            }

            UpdateGameState(GameState.Wait);
        }

        private void HandleNewTurn()
        {
            if (ActiveTeams() <= 1) UpdateGameState(GameState.EndGame);
            else
            {
                UpdateGameState(GameState.StartTurn);
            }
        }

        private void HandleStartTurn()
        {
            CurrentTurn++; 
            _onStartTurn.Invoke();

            _activePlayer = GetNextPlayer();
            _activePlayer.EnablePlayer();
        }

        private void HandleEndTurn()
        {
            _onEndTurn.Invoke();

            _activePlayer.DisablePlayer();
            StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(_timeBetweenTurns);
            UpdateGameState(GameState.NewTurn);
        }

        private void HandleEndGame()
        {
            GameObject TeamWithPlayers = null;

            foreach (var item in _teamRoots)
            {
                if (item.transform.childCount > 0) TeamWithPlayers = item;
            }

            if (!TryGetComponent(out UI.EndScreen endScreen)) return;

            if (TeamWithPlayers != null) endScreen.ShowEndScreen(TeamWithPlayers.name);
            else endScreen.ShowEndScreen("No one! It's a draw!");
        }

        private int GetSpawnPointIndex()
        {
            while (true)
            {
                int spawnPointIndex = Random.Range(0, _spawnPoints.Count);

                if (_usedSpawnPoints.Contains(spawnPointIndex)) continue;
                else
                {
                    _usedSpawnPoints.Add(spawnPointIndex);
                    return spawnPointIndex;
                }
            }
        }

        private int ActiveTeams()
        {
            int teamsWithActivePlayers = 0;

            foreach (var item in _teamRoots)
            {
                if (item.transform.childCount > 0) teamsWithActivePlayers++;
            }

            return teamsWithActivePlayers;
        }

        // TODO Fix error when no players are left
        private PlayerManager GetNextPlayer()
        {
            int nextTeamIndex = 0;
            int nextPlayerIndex;
            int teamOffset = 0;
            List<Transform> players = new() { };

            // Get next team
            while (teamOffset < GameVariables.NumberOfTeams)
            {
                nextTeamIndex = (CurrentTurn - 1 + teamOffset) % GameVariables.NumberOfTeams;

                Transform teamRoot = _teamRoots[nextTeamIndex].transform;

                if (teamRoot.childCount > 0)
                {
                    foreach (Transform player in teamRoot.transform)
                    {
                        players.Add(player);
                    }
                    break;
                }
                else
                {
                    teamOffset++;
                }
            }

            // Get next player and return
            if (nextTeamIndex == 0) CurrentRound++;
            nextPlayerIndex = (CurrentRound - 1) % GameVariables.PlayersPerTeam;
            if (nextPlayerIndex >= players.Count) nextPlayerIndex = 0;

            print("Next Team: " + nextTeamIndex + " Player: " + nextPlayerIndex);
            return players[nextPlayerIndex].GetComponentInChildren<PlayerManager>();
        }
    }
}