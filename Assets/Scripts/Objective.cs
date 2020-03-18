using UnityEngine;
using UnityEngine.SceneManagement;

public class Objective : MonoBehaviour
{
    // Start is called before the first frame update
    private int _objectiveCount = 0;
    void Start()
    {
        switch (SceneManager.GetActiveScene().name) {
            case "Level 1":
            case "Level 5": 
                Messenger.AddListener(GameEvent.ENEMY_KILLED.ToString(), onEnemyKilled);
                break;
            case "Level 2":
            case "Level 6":
                Messenger.AddListener(GameEvent.HOT_SPRINGS_FOUND.ToString(), onHotSpringsFound);
                break;
        }

    }
    void onEnemyKilled() {
        _objectiveCount--;
        if (_objectiveCount == 0) Manager.Progress.NextSeason();
    }

    public void onHotSpringsFound() {
        Manager.Progress.NextSeason();
    }

        private void OnDestroy()
    {
        switch (SceneManager.GetActiveScene().name) {
            case "Level 1":
            case "Level 5": 
                Messenger.RemoveListener(GameEvent.ENEMY_KILLED.ToString(), onEnemyKilled);
                break;
            case "Level 2":
            case "Level 6":
                Messenger.RemoveListener(GameEvent.HOT_SPRINGS_FOUND.ToString(), onHotSpringsFound);
                break;

        }
    }
}
