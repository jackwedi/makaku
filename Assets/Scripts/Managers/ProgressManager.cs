using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    [SerializeField] private int currentLevel;
    public int countDown { get; private set; }
    private string currentSeason;

    private Vector3 checkpoint;

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
        SceneManager.LoadScene("Main Menu");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        this.checkpoint = checkpointTransform.position;
    }

    public Vector3 GetCheckPoint()
    {
        return this.checkpoint;
    }

    public void Save()
    {
        //TODO SAVE the game
        SceneManager.LoadScene("Main Menu");
    }

}
