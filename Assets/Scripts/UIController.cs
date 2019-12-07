
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider dashUI = null;
    [SerializeField] private GameObject loadingPanel = null;

    void Start()
    {
        Messenger<float, float>.AddListener(GameEvent.DASH_DELAY_UPDATED, OnDashDelayUpdated);
        Messenger.AddListener(GameEvent.DEATH, OnDeath);
    }

    private void OnDestroy()
    {
        Messenger<float, float>.RemoveListener(GameEvent.DASH_DELAY_UPDATED, OnDashDelayUpdated);
        Messenger.RemoveListener(GameEvent.DEATH, OnDeath);
    }

    private void OnDashDelayUpdated(float currentTime, float timeToWait)
    {
        dashUI.value = currentTime / timeToWait;
    }

    private void OnLevelReady()
    {
        loadingPanel.SetActive(false);
    }

    private void OnDeath()
    {
        // Create another animation on Death and set up the menu to either restart or quit
        Manager.Progress.RestartLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }
}
