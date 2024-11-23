using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable, IDamageable
{
    [SerializeField] private ProjectileType _type;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private ParticleSystem _particleSystem;
    private float speed = 5f;
    private Vector3 _velocity;
    private int _damage;
    public void Initialize(Vector3 initPosition, Vector3 direction, int damage)
    {
        transform.position = initPosition;
        _damage = damage;
        _velocity = direction.normalized * speed;
    }

    public void OnCreated()
    {
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
        Vector3 screenMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 screenMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

        if (transform.position.x < screenMin.x || transform.position.x > screenMax.x ||
            transform.position.y < screenMin.y || transform.position.y > screenMax.y)
        {
            ProjectilePoolManager.Instance.Return(_type, this);
        }

        transform.position += _velocity * Time.deltaTime;
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
