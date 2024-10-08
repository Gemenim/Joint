using System.Collections;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    [SerializeField] private GameObject _spring;
    [SerializeField] private GameObject _trigger;
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
        SetTrigger(false, true);
    }

    public void Spawn()
    {
        if (!_isCharging)
        {
            _isCharging = true;
            SetTrigger(false, true);
            StartCoroutine(Charge());
        }
    }

    public void Shot()
    {
        if (_bullet != null)
        {
            SetTrigger(true, false);
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

    private void SetTrigger(bool spring, bool trigger)
    {
        _spring.SetActive(spring);
        _trigger.SetActive(trigger);
    }

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
