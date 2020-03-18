
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider dashUI = null;
    [SerializeField] private GameObject loadingPanel = null;
    private bool init = false;

    void Start()
    {
        Messenger<float, float>.AddListener(GameEvent.DASH_DELAY_UPDATED.ToString(), OnDashDelayUpdated);
        Messenger.AddListener(GameEvent.DEATH.ToString(), OnDeath);
        init = true;
    }

    private void OnDestroy()
    {
        // SAFEGUARD OnDestroy is called before onStart
        if (init) {
            Messenger<float, float>.RemoveListener(GameEvent.DASH_DELAY_UPDATED.ToString(), OnDashDelayUpdated);
            Messenger.RemoveListener(GameEvent.DEATH.ToString(), OnDeath);
        }
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Manager.Progress.Save();
        }
    }
}
