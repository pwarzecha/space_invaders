using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable, IDamageable
{
    [SerializeField] private EnemyDataSO enemyData;
    private int _health;
    private float _fireTimer;
    private bool _isAlive;
    private Vector3 _formationDirection;
    private float _formationSpeed;

    public Action OnDie;
    public Action<int> OnUpdateScoreRequest;
    public Action<Enemy> OnRemovedFromMap;
    public void OnCreated()
    {

    }

    public void OnPooled()
    {
        _health = enemyData.initHealth;
        _fireTimer = 0;
        _isAlive = true;
    }

    public void OnReturn()
    {
        OnDie = null;
        OnUpdateScoreRequest = null;
        OnRemovedFromMap = null;
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
        transform.position += Vector3.down * (enemyData.movementSpeed * Time.deltaTime);

        Vector3 screenMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 screenMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

        if (transform.position.y < screenMin.y)
        {
            OnUpdateScoreRequest?.Invoke(-enemyData.scoreReward);
            OnRemovedFromMap?.Invoke(this);
            EnemyPoolManager.Instance.Return(enemyData.enemyType, this);
        }
    }
    public void MoveWithFormation()
    {
        transform.position += _formationDirection * (_formationSpeed * Time.deltaTime);
    }
    public void SetFormationMovement(Vector3 direction, float speed)
    {
        _formationDirection = direction;
        _formationSpeed = speed;
    }

    public void InvertDirection()
    {
        _formationDirection.x = -_formationDirection.x;
    }
    private void HandleFiring()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= enemyData.fireInterval)
        {
            Fire();
            _fireTimer = 0.0f;
        }
    }

    private void Fire()
    {
        Projectile projectile = ProjectilePoolManager.Instance.Get(ProjectileType.Enemy);
        projectile.Initialize(transform.position, Vector3.down, enemyData._projectileDamage);
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
        if (!_isAlive)
            return;

        _isAlive = false;
        OnDie?.Invoke();
        OnRemovedFromMap?.Invoke(this);
        OnUpdateScoreRequest?.Invoke(enemyData.scoreReward);
        if (UnityEngine.Random.value < enemyData.powerUpDropChance)
        {
            var powerUp = PowerUpPoolManager.Instance.GetRandom();
            powerUp.transform.position = transform.position;
        }

        var vfx = VFXPoolManager.Instance.Get(VFXType.Explosion);
        vfx.transform.position = transform.position;
        EnemyPoolManager.Instance.Return(enemyData.enemyType, this);
    }
}
