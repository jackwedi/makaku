using UnityEngine;
using System.Collections;


public class PlayerManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    [SerializeField] private GameObject _player;
    private Vector3 startingPosition;

    private bool _alive = true;

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
        StartCoroutine("DelayedInputs");
    }

    public void SetPlayer(GameObject player)
    {
        this._player = player;
    }

    public IEnumerator DelayedInputs()
    {
        yield return new WaitForSeconds(0.5f);
        _player.GetComponent<Platformer>().SetStatic(false);
        this._alive = true;
    }

    public void setAlive(bool alive)
    {
        this._alive = alive;
    }

    public bool getAlive()
    {
        return this._alive;
    }

}

