using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    private GameObject _player;
    private int _health;
    private int _maxHealth;
    private Vector3 startingPosition;
    

    public void Startup() {
        status = ManagerStatus.Initialized;

        _health = 3;
        _maxHealth = 3;

        status = ManagerStatus.Started;
    }

    public void UpdateStats(int health, int maxHealth) {
        _health = health;
        _maxHealth = maxHealth;
        Messenger<int, int>.Broadcast(GameEvent.HEALTH_UPDATED, _health, _maxHealth);
    }

    public void Hurt(int damage) {
        _health -= damage;

        if(_health <= 0) {
            Messenger.Broadcast(GameEvent.DEATH);
        }
        else {
            Messenger<int, int>.Broadcast(GameEvent.HEALTH_UPDATED, _health, _maxHealth);
        }
    }

    public void RespawnAtCheckPoint() {
        _player.transform.position = Manager.Progress.GetCheckPoint().position;
    }

    public void SetPlayer(GameObject player) {
        this._player = player;
    }

    public void ResetHealth() {
        _health = 3;
        _maxHealth = 3;
    }

}
