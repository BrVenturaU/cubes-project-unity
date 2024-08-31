using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public float fireRate = .5f;
    public float fireRange = 50f;
    public float hitForce = 100f;
    public int laserDamage = 100;

    private LineRenderer _laserLine;
    private AudioSource _audioSource;
    private bool _isLaserLineEnabled;
    private WaitForSeconds _laserDuration = new WaitForSeconds(0.05f);
    private float _nextFire;

    // Use this for initialization 
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _laserLine = GetComponent<LineRenderer>();
    }
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > _nextFire)
        {
            Fire();
        }
    }

    private void Fire()
    {
        _audioSource.Play();
        Transform cam = Camera.main.transform;
        _nextFire = Time.time + fireRate;
        Vector3 rayOrigin = cam.position;
        _laserLine.SetPosition(0, transform.up * -10f);

        if (Physics.Raycast(rayOrigin, cam.forward, out RaycastHit hit, fireRange))
        {
            _laserLine.SetPosition(1, hit.point);

            CubeBehaviorScript cubeCtr = hit.collider.GetComponent<CubeBehaviorScript>();
            if (cubeCtr != null)
            {
                if (hit.rigidbody != null)
                {
                    // apply force to the target 
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                    // apply damage the target 
                    cubeCtr.TryDestroyCube(laserDamage);
                }
            }
        }
        else
        {
            _laserLine.SetPosition(1, cam.forward * fireRange);
        }

        StartCoroutine(LaserFx());
    }

    private IEnumerator LaserFx()
    {
        _laserLine.enabled = true;
        yield return _laserDuration;
        _laserLine.enabled = false;
    }
}
