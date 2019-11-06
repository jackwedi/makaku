using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    private int currentLevel;
    public int countDown { get; private set; }
    private string currentSeason;

    private Transform checkpoint;

    [SerializeField] private string _initLevel = null;

    public void Startup()
    {
        status = ManagerStatus.Initialized;

        //Load FROM SAVE state
        currentLevel = 1;

        status = ManagerStatus.Started;
    }

    public void NextSeason()
    {
        if (_initLevel != null) 
        {
            SceneManager.LoadScene(_initLevel);
            return;
        }
        
        if(SceneManager.GetActiveScene().name != "Main Menu") currentLevel += 1;
        string nextLevel = "Level " + currentLevel.ToString();
        SceneManager.LoadScene(nextLevel);
    }

    public void RestartLevel()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Manager.Player.ResetHealth();
    }

    public void UpdateLevel(int level)
    {
        currentLevel = level;
    }

    public void SetCountDown(int time)
    {
        countDown = time;
    }

    public int Level()
    {
        return currentLevel;
    }

    public void SetCheckPoint(Transform checkpointTransform)
    {
        this.checkpoint = checkpointTransform;
    }

    public Transform GetCheckPoint()
    {
        return this.checkpoint;
    }

}
