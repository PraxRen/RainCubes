using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubePool : MonoBehaviour
{
    [SerializeField] private Cube _prefabCube;
    [SerializeField] private int _count;

    private List<Cube> _cubes = new List<Cube>();

    private void Start()
    {
        StartCoroutine(CreateCubes());
    }

    public bool TryGetCube(out Cube cube)
    {
        cube = _cubes.FirstOrDefault(cube => cube.gameObject.activeSelf == false);

        if (cube == null)
            return false;

        cube.gameObject.SetActive(true);
        cube.transform.parent = null;
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

    private void OnDiedCube(Cube cube)
    {
        cube.transform.parent = transform;
        cube.gameObject.SetActive(false);
    }
}