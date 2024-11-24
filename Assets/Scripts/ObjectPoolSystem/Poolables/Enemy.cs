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
    private bool _isFormation = false;

    public Action<int> OnUpdateScoreRequest;
    public Action OnDie;
    public void OnCreated()
    {

    }

    public void OnPooled()
    {
        _health = enemyData.initHealth;
        _fireTimer = 0;
        _isAlive = true;
        _isFormation = false;
    }

    public void OnReturn()
    {
        OnUpdateScoreRequest = null;
        OnDie = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable) && other.CompareTag("Player"))
        {
            damageable.Hit(1);
            Die();
        }
    }
    public void SetFormationMovement(Vector3 direction, float speed)
    {
        _isFormation = true;
        _formationDirection = direction;
        _formationSpeed = speed;
    }
    private void FixedUpdate()
    {
        if (_isFormation)
        {
            MoveInFormation();
        }
        else
        {
            Move();
        }

        HandleFiring();
    }

    private void MoveInFormation()
    {
        transform.position += _formationDirection.normalized * (_formationSpeed * Time.deltaTime);
    }
    private void Move()
    {
        transform.position += Vector3.down * (enemyData.movementSpeed * Time.deltaTime);

        Vector3 screenMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 screenMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

        if (transform.position.x < screenMin.x || transform.position.x > screenMax.x ||
            transform.position.y < screenMin.y || transform.position.y > screenMax.y)
        {
            OnUpdateScoreRequest?.Invoke(-enemyData.scoreReward);
            EnemyPoolManager.Instance.Return(enemyData.enemyType, this);
        }
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
