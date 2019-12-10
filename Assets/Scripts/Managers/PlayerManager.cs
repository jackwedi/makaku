using UnityEngine;
using System.Collections;


public class PlayerManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    [SerializeField] private GameObject _player;
    private Vector3 startingPosition;

    public void Startup()
    {
        status = ManagerStatus.Initialized;
        status = ManagerStatus.Started;
    }

    public void RespawnAtCheckPoint()
    {
        // ADD ANIM
        _player.transform.position = Manager.Progress.GetCheckPoint();
        _player.GetComponent<Platformer>().resetAnim();
        StartCoroutine("test");
    }

    public void SetPlayer(GameObject player)
    {
        this._player = player;
    }

    public IEnumerator test()
    {
        yield return new WaitForSeconds(1);
        _player.GetComponent<Platformer>().SetStatic(true);
    }

}

