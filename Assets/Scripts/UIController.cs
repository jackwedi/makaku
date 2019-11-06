
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField]private Slider dashUI;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Image[] hearts; // Needs to be sorted by : 1st Heart at 0 ...
    [SerializeField] private float counterMultiplier = 12;

    void Start()
    {
        Messenger<float, float>.AddListener(GameEvent.DASH_DELAY_UPDATED, OnDashDelayUpdated);
        //Messenger.AddListener(GameEvent.LEVEL_READY, OnLevelReady);
        Messenger<int, int>.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.AddListener(GameEvent.DEATH, OnDeath);
    }

    private void OnDestroy()
    {
        Messenger<float, float>.RemoveListener(GameEvent.DASH_DELAY_UPDATED, OnDashDelayUpdated);
        //Messenger.RemoveListener(GameEvent.LEVEL_READY, OnLevelReady);
        Messenger<int, int>.RemoveListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
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

    private void OnHealthUpdated(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (currentHealth - 1 < i) hearts[i].gameObject.SetActive(false);
            else hearts[i].gameObject.SetActive(true);
        }
    }

    private void OnDeath()
    {
        // Create another animation on Death and set up the menu to either restart or quit
        Manager.Progress.RestartLevel();
    }
}
