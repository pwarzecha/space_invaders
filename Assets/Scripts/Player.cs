using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerSettingsSO _playerSettingsSO;

    private int _health;
    private float _fireTimer;
    private float _projectileSpeedBoost;
    private int _projectileDamageBoost;
    private int _projectileAmount;

    public Action OnDie;
    public Action<int> OnHealthUpdated;

    public void Initialize()
    {
        _health = _playerSettingsSO.maxHealth;
        _projectileSpeedBoost = _playerSettingsSO.baseProjectileSpeedBoost;
        _projectileDamageBoost = _playerSettingsSO.baseProjectileDamageBoost;
        _projectileAmount = _playerSettingsSO.baseProjectilesAmount;
    }

    private void Update()
    {
        HandleMovement();
        HandleFiring();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontal, 0, 0) * _playerSettingsSO.movementSpeed *+ Time.deltaTime;
        transform.position += movement;

        float clampedX = Mathf.Clamp(transform.position.x, -5f, 5f);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    private void HandleFiring()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= _playerSettingsSO.fireInterval && Input.GetMouseButton(0))
        {
            Fire();
            _fireTimer = 0.0f;
        }
    }

    private void Fire()
    {
        float angleStep = 15f;
        float startingAngle = -(_projectileAmount - 1) / 2f * angleStep;

        for (int i = 0; i < _projectileAmount; i++)
        {
            float angle = startingAngle + i * angleStep;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.up;

            var projectile = ProjectilePoolManager.Instance.Get(ProjectileType.Player);
            projectile.Initialize(transform.position, direction, _projectileDamageBoost, _projectileSpeedBoost);
        }
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
        OnDie?.Invoke();
    }

    public void TryHealUp(int _healthAmount)
    {
        _health = Mathf.Clamp(_health + _healthAmount, 0, _playerSettingsSO.maxHealth);
    }

    public void IncreaseProjectileDamage(int damageBoostAmount)
    {
        _projectileDamageBoost += damageBoostAmount;
    }

    public void ResetProjectileDamage()
    {
        _projectileDamageBoost = _playerSettingsSO.baseProjectileDamageBoost;
    }
    public void IncreaseProjectileSpeed(float speedBoostAmount)
    {
        _projectileSpeedBoost += speedBoostAmount;
    }

    public void ResetProjectileSpeed()
    {
        _projectileSpeedBoost = _playerSettingsSO.baseProjectileSpeedBoost;
    }
    public void IncreaseProjectilesAmount(int amountBoost)
    {
        _projectileAmount = Mathf.Clamp(_projectileAmount += amountBoost, 1, _playerSettingsSO.maxProjectilesAmount);
    }

    public void ResetProjectileAmount()
    {
        _projectileAmount = _playerSettingsSO.baseProjectilesAmount;
    }
}
