using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(ProgressManager))]
public class Manager : MonoBehaviour
{
    public static Manager instance = null;
    public static PlayerManager Player { get; private set; }
    public static InventoryManager Inventory { get; private set; }
    public static ProgressManager Progress { get; private set; }

    private List<IGameManager> _managers;

    private void Awake()
    {

        DontDestroyOnLoad(gameObject);

        _managers = new List<IGameManager>();

        Player = GetComponent<PlayerManager>();
        Inventory = GetComponent<InventoryManager>();
        Progress = GetComponent<ProgressManager>();

        _managers.Add(Player);
        _managers.Add(Inventory);
        _managers.Add(Progress);

        StartCoroutine(StartupManagers());
    }

    private void Start()
    {

        if (Manager.instance == null)
        {
            Debug.Log("NO");
            Manager.instance = this;
        }
        else
        {
            Debug.Log("YES");
            Messenger.Broadcast(GameEvent.MANAGERS_READY);
            Object.Destroy(gameObject);
        }
    }

    private IEnumerator StartupManagers()
    {

        foreach (IGameManager manager in _managers)
        {
            manager.Startup();
        }

        yield return null;

        int numManager = 0;
        int numManagerReady = 0;

        while (numManagerReady < numManager)
        {
            int previousNumManagerReady = numManagerReady;
            numManagerReady = 0;

            foreach (IGameManager manager in _managers)
            {
                if (manager.status == ManagerStatus.Started) numManagerReady++;
            }
            yield return null;
        }
        Messenger.Broadcast(GameEvent.MANAGERS_READY);
    }
}
