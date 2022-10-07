using UnityEngine;
using UnityEngine.SceneManagement;

namespace Seb.UI
{
    public class Menu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void ReStart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void SetNumberOfTeams(int numberOfTeams)
        {
            GameVariables.NumberOfTeams = numberOfTeams + 2;
        }

        public void SetPlayersPerTeam(int playersPerTeam)
        {
            GameVariables.PlayersPerTeam = playersPerTeam + 1;
        }
    }
}
