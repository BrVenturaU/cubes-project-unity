using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
public class SpawnScript : MonoBehaviour
{

    public GameObject cubeEl;

    public int totalCubes = 10;
    public float timeToSpawn = 1f;
    private GameObject[] _cubes;
    private bool _isCameraPositionSet;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnLoop());
        _cubes = new GameObject[totalCubes];
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool SetPosition()
    {
        Transform cam = Camera.main.transform;
        transform.position = cam.forward * 10;
        return true;
    }

    private IEnumerator ChangePosition()
    {
        yield return new WaitForSeconds(0.2f);

        if(!_isCameraPositionSet)
        {
            if(VuforiaBehaviour.Instance.enabled)
                _isCameraPositionSet = SetPosition();
        }
    }

    private IEnumerator SpawnLoop()
    {
        StartCoroutine(ChangePosition());
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < totalCubes; i++)
        {
            _cubes[i] = SpawnElement();
            yield return new WaitForSeconds(Random.Range(timeToSpawn, timeToSpawn * 3));
        }
    }

    private GameObject SpawnElement()
    {
        GameObject cube = Instantiate(cubeEl, (Random.insideUnitSphere * 2) + transform.position, transform.rotation);
        float scale = Random.Range(0.5f, 2f);
        cube.transform.localScale = new Vector3(scale, scale, scale);
        return cube;
    }
}

