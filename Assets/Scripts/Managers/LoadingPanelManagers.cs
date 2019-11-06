using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadingPanelManagers : MonoBehaviour
{
    private void Awake()
    {
        Messenger.AddListener(GameEvent.MANAGERS_READY, OnManagersReady);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.MANAGERS_READY, OnManagersReady);
    }

    public void OnManagersReady()
    {
        gameObject.SetActive(false);
    }
}
