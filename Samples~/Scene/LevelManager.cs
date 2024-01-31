using Blackboard;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuestDesigner.Examples
{
    public class LevelManager : MonoBehaviour
    {
        private InputManager _inputManager;

        private void Start()
        {
            _inputManager = ServiceLocator.Get<InputManager>();
            _inputManager.OnRestartPerformed += RestartLevel;
        }

        public void RestartLevel()
        {
            BlackboardManager.RevertChanges();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnDestroy()
        {
            _inputManager.OnRestartPerformed -= RestartLevel;
        }
    }

}