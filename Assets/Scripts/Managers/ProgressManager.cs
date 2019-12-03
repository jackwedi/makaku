using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    [SerializeField] private int currentLevel;
    public int countDown { get; private set; }
    private string currentSeason;

    private Transform checkpoint;

    [SerializeField] private string _initLevel = null;

    public void Startup()
    {
        status = ManagerStatus.Initialized;

        //Load FROM SAVE state
        //currentLevel = 1;

        status = ManagerStatus.Started;
    }

    public void NextSeason()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu") currentLevel += 1;
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

    public int getLevel()
    {
        return this.currentLevel;
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
        Debug.Log(this.checkpoint);
        return this.checkpoint;
    }

}
