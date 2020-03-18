using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadingPanelManagers : MonoBehaviour
{
    private void Awake()
    {
        Messenger.AddListener(GameEvent.MANAGERS_READY.ToString(), OnManagersReady);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.MANAGERS_READY.ToString(), OnManagersReady);
    }

    public void OnManagersReady()
    {
        /* gameObject.SetActive(false); */
        SceneManager.LoadScene("Main Menu");
    }
}
