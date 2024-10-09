using System.Collections;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    [SerializeField] private SpringJoint _spring, _trigger;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Bullet _prefab;
    [SerializeField] private Transform _spawnPoint;

    private const float _timeCharging = 1.5f;
    private Pool<Bullet> _pool;
    private Bullet _bullet;
    private bool _isCharging = false;

    private void Awake()
    {
        _pool = new Pool<Bullet>(Preload, GetAction, ReturnAction);
    }

    private void Start()
    {
        _spring.connectedBody = null;
        _trigger.connectedBody = _rigidbody;
    }

    public void Spawn()
    {
        if (!_isCharging)
        {
            _isCharging = true;
            _spring.connectedBody = null;
            _trigger.connectedBody = _rigidbody;
            StartCoroutine(Charge());
        }
    }

    public void Shot()
    {
        if (_bullet != null)
        {
            _spring.connectedBody = _rigidbody;
            _trigger.connectedBody = null;
            _bullet.ReternPool();
        }
    }

    private Bullet Preload()
    {
        Bullet bullet = Instantiate(_prefab);
        bullet.SetPool(_pool);

        return bullet;
    }

    private void GetAction(Bullet drop) => drop.gameObject.SetActive(true);
    private void ReturnAction(Bullet drop) => drop.gameObject.SetActive(false);

    private IEnumerator Charge()
    {
        WaitForSeconds timeCharging = new WaitForSeconds(_timeCharging);

        yield return timeCharging;

        _bullet = _pool.Get();
        _bullet.transform.position = _spawnPoint.position;
        _bullet.transform.rotation = new Quaternion(0, 0, 0, 0);
        _isCharging = false;
    }
}
