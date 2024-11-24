using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerDataSO _playerSettingsSO;
    private Vector3 _targetPosition;
    private bool _canMove = false;
    private float _currentTilt = 0f;
    private float _tiltAngle = 30f;
    private float _tiltSpeed = 10;

    private int _health;
    private float _fireTimer;
    private float _fireInterval;
    private int _projectileDamage;
    private int _projectileAmount;

    public Action OnDie;
    public Action<int> OnHealthUpdated;

    public PlayerDataSO PlayerSettingsSO => _playerSettingsSO;

    public void Initialize(Vector3 spawnPosition)
    {
        _health = _playerSettingsSO.maxHealth;
        _fireInterval = _playerSettingsSO.baseFireInterval;
        _projectileDamage = _playerSettingsSO.baseDamage;
        _projectileAmount = _playerSettingsSO.baseProjectilesAmount;
        _tiltAngle = _playerSettingsSO.tiltAngle;
        _tiltSpeed = _playerSettingsSO.tiltSpeed;

        transform.position = spawnPosition;
        _targetPosition = transform.position;
        gameObject.SetActive(true);

        _canMove = true;
    }

    private void Update()
    {
        if (_canMove)
        {
            HandleMovement();
            UpdateTilt();
            HandleFiring();
        }
    }


    private void HandleMovement()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            MoveToScreenPosition(touchPosition);
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            MoveToScreenPosition(mousePosition);
        }
        SmoothMoveToTarget();
    }

    private void UpdateTilt()
    {
        float targetAngle;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed ||
            Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            targetAngle = _currentTilt * _tiltAngle;
        }
        else
        {
            _currentTilt = Mathf.Lerp(_currentTilt, 0f, _tiltSpeed * Time.deltaTime);
            targetAngle = 0f; 
        }

        float smoothedAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, targetAngle, _tiltSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, smoothedAngle, 0);
    }

    private void MoveToScreenPosition(Vector2 screenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float clampedX = Mathf.Clamp(worldPosition.x, -screenBounds.x, screenBounds.x);
        float clampedY = Mathf.Clamp(worldPosition.y, -screenBounds.y, screenBounds.y);
        _currentTilt = Mathf.Clamp(transform.position.x - clampedX, -1f, 1f);
        _targetPosition = new Vector3(clampedX, clampedY, transform.position.z);
    }
    private void SmoothMoveToTarget()
    {
        if (Vector3.Distance(transform.position, _targetPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, _playerSettingsSO.moveSpeed * Time.deltaTime);
        }
    }
    private void HandleFiring()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= _fireInterval)
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

            var projectileInitPosition = transform.position + _playerSettingsSO.projectileSpawnOffset;
            var projectile = ProjectilePoolManager.Instance.Get(ProjectileType.Player);
            projectile.Initialize(projectileInitPosition, direction, _projectileDamage);

            var vfx = VFXPoolManager.Instance.Get(VFXType.Muzzle);
            vfx.transform.position = projectileInitPosition;
        }
    }

    public void Hit(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }

        OnHealthUpdated?.Invoke(_health);
    }

    private void Die()
    {
        _canMove = false;

        var vfx = VFXPoolManager.Instance.Get(VFXType.Explosion);
        vfx.transform.position = transform.position;

        OnDie?.Invoke();
    }

    public void TryHealUp(int _healthAmount)
    {
        _health = Mathf.Clamp(_health + _healthAmount, 0, _playerSettingsSO.maxHealth);
        OnHealthUpdated?.Invoke(_health);
        Debug.Log($"New health value: {_health}");
    }

    public void IncreaseProjectileDamage(int damageBoostAmount)
    {
        _projectileDamage = Mathf.Clamp(_projectileDamage + damageBoostAmount, _playerSettingsSO.baseDamage, _playerSettingsSO.maxDamage);
        Debug.Log($"New projectile damage boost value: {_projectileDamage}");
    }

    public void ResetProjectileDamage()
    {
        _projectileDamage = _playerSettingsSO.baseDamage;
    }
    public void IncreaseFireRate(float speedBoostAmount)
    {
        _fireInterval = Mathf.Clamp(_fireInterval - speedBoostAmount, _playerSettingsSO.minFireInterval, _playerSettingsSO.baseFireInterval);
        Debug.Log($"New projectile speed boost value: {_fireInterval}");
    }

    public void ResetFireRate()
    {
        _fireInterval = _playerSettingsSO.baseFireInterval;
    }
    public void IncreaseProjectilesAmount(int amountBoost)
    {
        _projectileAmount = Mathf.Clamp(_projectileAmount + amountBoost, 1, _playerSettingsSO.maxProjectilesAmount);
        Debug.Log($"New projectile amount value: {_projectileAmount}");
    }

    public void ResetProjectileAmount()
    {
        _projectileAmount = _playerSettingsSO.baseProjectilesAmount;
    }
}
