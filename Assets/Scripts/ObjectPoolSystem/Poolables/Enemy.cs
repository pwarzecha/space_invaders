using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable, IDamageable
{
    [SerializeField] private EnemySettingsSO settings;
    private int _health;
    private float _fireTimer;
    private bool isAlive;

    public Action<int> OnUpdateScoreRequest;
    public void OnCreated()
    {

    }

    public void OnPooled()
    {
        _health = settings.initHealth;
        _fireTimer = 0;
        isAlive = true;
    }

    public void OnReturn()
    {
        OnUpdateScoreRequest = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable) && other.CompareTag("Player"))
        {
            damageable.Hit(1);
            Die();
        }
    }
    private void FixedUpdate()
    {
        Move();
        HandleFiring();
    }

    private void Move()
    {
        transform.position += Vector3.down * (settings.movementSpeed * Time.deltaTime);

        Vector3 screenMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 screenMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

        if (transform.position.x < screenMin.x || transform.position.x > screenMax.x ||
            transform.position.y < screenMin.y || transform.position.y > screenMax.y)
        {
            EnemyPoolManager.Instance.Return(settings.enemyType, this);
        }
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
        projectile.Initialize(transform.position, Vector3.down, settings._projectileDamage);
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
        if (!isAlive)
            return;

        isAlive = false;
        OnUpdateScoreRequest?.Invoke(settings.scoreReward);
        if (UnityEngine.Random.value < settings.powerUpDropChance)
        {
            var powerUp = PowerUpPoolManager.Instance.GetRandom();
            powerUp.transform.position = transform.position;
        }

        var vfx = VFXPoolManager.Instance.Get(VFXType.Explosion);
        vfx.transform.position = transform.position;
        EnemyPoolManager.Instance.Return(settings.enemyType, this);
    }
}
