using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] planets;
    public GameObject blackHole;
    public GameObject ufo;
    public int level;

    public float xBounds;
    public float yBound;

    void Start()
    {
        StartCoroutine(SpawnRandomObject());
    }

    IEnumerator SpawnRandomObject()
    {
        yield return new WaitForSeconds(Random.Range(1, 2));

        int randomPlanet = Random.Range(0, planets.Length);

        if (Random.value <= 0.6f)
            Instantiate(planets[randomPlanet],
                new Vector2(Random.Range(-xBounds, xBounds), yBound), Quaternion.identity);
        else
            Instantiate(blackHole,
                new Vector2(Random.Range(-xBounds, xBounds), yBound), Quaternion.identity);

        if (Random.value <= 0.6f && level != 1)
            Instantiate(ufo,
                new Vector2(Random.Range(-xBounds, xBounds), yBound), Quaternion.identity);

        StartCoroutine(SpawnRandomObject());
    }
}