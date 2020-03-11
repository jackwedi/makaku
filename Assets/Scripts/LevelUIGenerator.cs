using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelUIGenerator : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button[] _buttons = null;

    // Start is called before the first frame update
    void Start()
    {
        setButtons();
    }

    void setButtons()
    {
        for (int i = 0; i < Manager.Progress.getLevel(); i++)
        {
            string level = (i + 1).ToString();
            Debug.Log(level);

            _buttons[i].gameObject.SetActive(true);
            _buttons[i].GetComponentInChildren<UnityEngine.UI.Text>().text = level;
            _buttons[i].onClick.AddListener(() => SceneManager.LoadScene("Level " + level));
        }
    }
}
