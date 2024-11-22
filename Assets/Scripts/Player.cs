using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerSettingsSO _playerSettingsSO;

    private PlayerInputActions _inputActions;
    private Vector2 _movementInput;
    private bool _canMove = false;
    private int _health;
    private float _fireTimer;
    private float _projectileSpeedBoost;
    private int _projectileDamageBoost;
    private int _projectileAmount;
    public Action OnDie;
    public Action<int> OnHealthUpdated;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
    }
    public void Initialize(Vector3 spawnPosition)
    {
        _health = _playerSettingsSO.maxHealth;
        _projectileSpeedBoost = _playerSettingsSO.baseProjectileSpeedBoost;
        _projectileDamageBoost = _playerSettingsSO.baseProjectileDamageBoost;
        _projectileAmount = _playerSettingsSO.baseProjectilesAmount;

        transform.position = spawnPosition;
        gameObject.SetActive(true);

        _inputActions.Player.Enable();
        _inputActions.Player.Movement.performed += OnMovementPerformed;
        _inputActions.Player.Movement.canceled += OnMovementCanceled;
        _canMove = true;
    }

    private void Update()
    {
        if (_canMove)
        {
            HandleMovement();
            HandleFiring();
        }
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        _movementInput = Vector2.zero;
    }

    private void HandleMovement()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            MoveToScreenPosition(touchPosition);
        }
        else if (Mouse.current != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            MoveToScreenPosition(mousePosition);
        }
    }
    private void MoveToScreenPosition(Vector2 screenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float clampedX = Mathf.Clamp(worldPosition.x, -screenBounds.x, screenBounds.x);
        float clampedY = Mathf.Clamp(worldPosition.y, -screenBounds.y, screenBounds.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
    private void HandleFiring()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= _playerSettingsSO.fireInterval)
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
        _canMove = false;
        _inputActions.Player.Disable();
        _inputActions.Player.Movement.performed -= OnMovementPerformed;
        _inputActions.Player.Movement.canceled -= OnMovementCanceled;

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
