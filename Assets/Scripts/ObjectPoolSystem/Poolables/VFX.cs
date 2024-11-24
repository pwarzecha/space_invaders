using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;
public class VFX : MonoBehaviour, IPoolable
{
    [SerializeField] private VFXType _type;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private bool returnAfterPlay;
    public void OnCreated()
    {
        gameObject.SetActive(false);
    }

    public void OnPooled()
    {
        if (_particleSystem != null)
        {
            _particleSystem.Play();
            if (returnAfterPlay)
                WaitForParticleEnd();
        }
    }

    public void OnReturn()
    {
        if (_particleSystem != null)
        {
            _particleSystem.Stop();
            _particleSystem.Clear();
        }
    }
    private async UniTask WaitForParticleEnd()
    {
        var mainModule = _particleSystem.main;
        if (mainModule.loop)
            return;

        await UniTask.WaitUntil(() => !_particleSystem.IsAlive(true));

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        VFXPoolManager.Instance.Return(_type, this);
    }
}
