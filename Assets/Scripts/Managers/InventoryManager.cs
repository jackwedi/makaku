using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    private int _collectibleCount;

    public void Startup()
    {
        status = ManagerStatus.Initialized;

        _collectibleCount = 0;

        status = ManagerStatus.Started;
    }

    public void ItemCollected()
    {
        _collectibleCount += 1;
    }
}
