using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubePool : MonoBehaviour
{
    [SerializeField] private Cube _prefabCube;
    [SerializeField] private int _count;

    private List<Cube> _cubes = new List<Cube>();

    private void OnEnable()
    {
        foreach (var cube in _cubes)
        {
            cube.Died += OnDiedCube;
            ResetCube(cube);
        }
    }

    private void OnDisable()
    {
        foreach (var cube in _cubes)
            cube.Died -= OnDiedCube;
    }

    private void Start()
    {
        StartCoroutine(CreateCubes());
    }

    public bool TryGetCube(out Cube cube)
    {
        cube = null;

        if (enabled == false)
            return false;

        cube = _cubes.FirstOrDefault(cube => cube.gameObject.activeSelf == false);

        if (cube == null)
            return false;

        cube.gameObject.SetActive(true);
        return true;
    }

    private IEnumerator CreateCubes()
    {
        for (int i = 0; i < _count; i++)
        {
            Cube newCube = Instantiate(_prefabCube, transform);
            newCube.gameObject.SetActive(false);
            newCube.Died += OnDiedCube; 
            _cubes.Add(newCube);
            yield return null;
        }
    }

    private void OnDiedCube(Cube cube) => ResetCube(cube);

    private void ResetCube(Cube cube)
    {
        cube.transform.localPosition = Vector3.zero;
        cube.gameObject.SetActive(false);
    }
}