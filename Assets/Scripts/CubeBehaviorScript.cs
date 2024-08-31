using UnityEngine;
using System.Collections;
public class CubeBehaviorScript : MonoBehaviour
{
    public float maxScale = 2f;
    public float minScale = 0.5f;
    public float orbitMaxSpeed = 30f;
    private float _orbitSpeed;
    public float growingSpeed = 10f;
    public int cubeHealth = 100;

    private AudioSource _audioSource;
    private ParticleSystem _particleSystem;
    private Transform _orbitAnchor;
    private Vector3 _orbitDirection;
    private Vector3 _cubeMaxScale;
    private bool _isCubeScaled = false;
    private bool _isAlive = true;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _particleSystem = GetComponent<ParticleSystem>();
        CubeSettings();
    }


    private void CubeSettings()
    {
        _orbitAnchor = Camera.main.transform;

        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        _orbitDirection = new Vector3(x, y, z);

        _orbitSpeed = Random.Range(5f, orbitMaxSpeed);
        float scale = Random.Range(minScale, maxScale);
        _cubeMaxScale = new Vector3(scale, scale, scale);
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame 
    void Update()
    {
        RotateCube();
        if (!_isCubeScaled)
            ScaleObj();
    }

    private void RotateCube()
    {
        transform.RotateAround(
            _orbitAnchor.position, _orbitDirection, _orbitSpeed * Time.deltaTime);
        transform.Rotate(30 * Time.deltaTime * _orbitDirection);
    }
    private void ScaleObj()
    {
        if (transform.localScale != _cubeMaxScale)
            transform.localScale = Vector3.Lerp(transform.localScale, _cubeMaxScale, Time.deltaTime * growingSpeed);
        else
            _isCubeScaled = true;
    }

    public bool TryDestroyCube(int hitDamage)
    {
        cubeHealth -= hitDamage;
        if (cubeHealth > 0 && _isAlive)
            return false;

        StartCoroutine(DestroyCube());
        return true;
    }

    // Destroy Cube 
    private IEnumerator DestroyCube()
    {
        _isAlive = false;

        GetComponent<Renderer>().enabled = false;

        _audioSource.Play();
        _particleSystem.Play();

        yield return new WaitForSeconds(_audioSource.clip.length);
        Destroy(gameObject, _particleSystem.main.duration);
    }
}