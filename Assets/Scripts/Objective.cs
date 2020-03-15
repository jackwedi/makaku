using UnityEngine;
using UnityEngine.SceneManagement;

public class Objective : MonoBehaviour
{
    // Start is called before the first frame update
    private int _objectiveCount = 0;
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level 1" || SceneManager.GetActiveScene().name == "Level 5") {
            Messenger.AddListener(GameEvent.ENEMY_KILLED, onEnemyKilled);
            this._objectiveCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            Debug.Log(_objectiveCount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onEnemyKilled() {
        _objectiveCount--;
        if (_objectiveCount == 0) Manager.Progress.NextSeason();
    }
}
