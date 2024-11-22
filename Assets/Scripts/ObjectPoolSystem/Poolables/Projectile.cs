using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable, IDamageable
{
    [SerializeField] private ProjectileType _type;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private ParticleSystem _particleSystem;
    private float speed = 5f;
    private Vector3 _velocity;
    private int _damage;
    private Vector2 screenBoundsX;
    private Vector2 screenBoundsY;
    public void Initialize(Vector3 initPosition, Vector3 direction, int damage)
    {
        transform.position = initPosition;
        _damage = damage;
        _velocity = direction.normalized * speed;
    }

    public void OnCreated()
    {
        screenBoundsX = GameController.Instance.GameSettingsSO.screenBoundsX;
        screenBoundsY = GameController.Instance.GameSettingsSO.screenBoundsY;
    }

    public void OnPooled()
    {
        _particleSystem.Play();
    }

    public void OnReturn()
    {
        _trailRenderer.Clear();
        _particleSystem.Stop();
        _damage = 0;
        _velocity = Vector3.zero;
    }

    private void Update()
    {
        transform.position += _velocity * Time.deltaTime;

        if (transform.position.x < screenBoundsX.x || transform.position.x > screenBoundsX.y
            || transform.position.y < screenBoundsY.x || transform.position.y > screenBoundsY.y)
            ProjectilePoolManager.Instance.Return(_type, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.Hit(_damage);
            ProjectilePoolManager.Instance.Return(_type, this);
            var vfx = VFXPoolManager.Instance.Get(VFXType.OnHit);
            vfx.transform.position = transform.position;
        }
    }
    public void Hit(int damage)
    {
        ProjectilePoolManager.Instance.Return(_type, this);
    }
}
