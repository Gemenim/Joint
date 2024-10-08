using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime;

    private Pool<Bullet> _pool;

    public void SetPool(Pool<Bullet> pool)
    {
        _pool = pool;
    }

    public void ReternPool()
    {
        StartCoroutine(Life());
    }

    private IEnumerator Life()
    {
        WaitForSeconds time = new WaitForSeconds(_lifeTime);

        yield return time;

        _pool.Return(this);
    }
}
