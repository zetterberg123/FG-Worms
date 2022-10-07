using UnityEditor;
using UnityEngine;

namespace Seb.Managers
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (!Application.isPlaying) return;

            GameManager gameManager = (GameManager)target;
            if (GUILayout.Button("End turn"))
            {
                gameManager.UpdateGameState(GameManager.GameState.EndTurn);
            }
        }
    }
}
