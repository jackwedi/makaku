using UnityEngine;
using UnityEngine.SceneManagement;

public class Objective : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int _objectiveCount = 0;
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
            case "Level 3":
            case "Level 7":
                Messenger.AddListener(GameEvent.BABY_SAVED.ToString(), onBabySaved);
                break;
            case "Level 4":
            case "Level 8":
                Messenger.AddListener(GameEvent.FRUIT_COLLECTED.ToString(), onFruitCollected);
                break;
        }

    }
    private void onEnemyKilled() {
        _objectiveCount--;
        if (_objectiveCount == 0) Manager.Progress.NextSeason();
    }

    private void onHotSpringsFound() {
        Manager.Progress.NextSeason();
    }

    private void onBabySaved() {
        _objectiveCount--;
        if (_objectiveCount == 0) Manager.Progress.NextSeason();
    }

    private void onFruitCollected() {
        _objectiveCount--;
        if (_objectiveCount == 0) Manager.Progress.NextSeason();
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
            case "Level 3":
            case "Level 7":
                Messenger.RemoveListener(GameEvent.BABY_SAVED.ToString(), onBabySaved);
                break;
            case "Level 4":
            case "Level 8":
                Messenger.RemoveListener(GameEvent.FRUIT_COLLECTED.ToString(), onBabySaved);
                break;

        }
    }
}
