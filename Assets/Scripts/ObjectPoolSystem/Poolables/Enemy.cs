using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable
{
    [SerializeField] private EnemySettingsSO settings;
    private int _health;
    private float _fireTimer;

    public Action<int> OnUpdateScoreRequest;
    public void OnCreated()
    {
        _health = settings.initHealth;
    }

    public void OnPooled()
    {
        _fireTimer = 0;
    }

    public void OnReturn()
    {

    }

    private void FixedUpdate()
    {
        Move();
        HandleFiring();
    }

    private void Move()
    {
        transform.position += Vector3.down * (settings.movementSpeed * Time.deltaTime);
    }

    private void HandleFiring()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= settings.fireInterval)
        {
            Fire();
            _fireTimer = 0.0f;
        }
    }

    private void Fire()
    {
        Projectile projectile = ProjectilePoolManager.Instance.Get(ProjectileType.Enemy);
        projectile.Initialize(transform.position, Vector3.down, settings.projectileDamageBoost, settings.projectileSpeedBoost);
    }

    public void Hit(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnUpdateScoreRequest?.Invoke(settings.scoreReward);
        if (UnityEngine.Random.value < settings.powerUpDropChance)
        {
            var powerUp = PowerUpPoolManager.Instance.GetRandom();
            powerUp.transform.position = transform.position;
        }
        EnemyPoolManager.Instance.Return(settings.enemyType, this);
    }
}
