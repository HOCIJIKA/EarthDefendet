using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private List<AudioSource> _rocketSongs;

    private int _indexRocket;
    private float _speedMove; 
    private float _damage;
    private GameObject _exsplosion;
    private Rigidbody _rb;
    private Transform _rocketPath;

    public void Init(float speed, float damage, GameObject exsplosion, int index, Transform rPath)
    {
        if (GameC.Instance.GetData().toggleSoundEffect)
        {
            _rocketSongs[Random.Range(0, _rocketSongs.Count)].Play();
        }

        _exsplosion = exsplosion;
        _speedMove = speed;
        _damage = damage;
        _rb = GetComponent<Rigidbody>();
        _indexRocket = index;
        _rocketPath = rPath;
        //startingAngle = transform.forward;
        //startingPosition = transform.position;

        Destroy(gameObject, 5f);
    }

    private void OnDestroy()
    {
        _rocketPath.GetComponent<RocketPath>().StopParticle();
    }

    public int GetIndex()
    {
        return _indexRocket;
    }

    public void TouchMeteor()
    {
        // ракета досягнула цілі
        LvlStatistic.Instance.AddStats(4, LvlStatistic.Instance.GetValuStats(4) + 1) ;
        Instantiate(_exsplosion, transform.position,transform.rotation);

        Destroy(gameObject);
    }

    public float GetDamageMeteor()
    {
        return _damage;
    }

    private void FixedUpdate()
    {
        _rb.AddForce(transform.localPosition * _speedMove * Time.fixedDeltaTime, ForceMode.VelocityChange);
        _rocketPath.transform.position = transform.position;
    }
}
