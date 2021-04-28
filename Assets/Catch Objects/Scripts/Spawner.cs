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

    public GameObject infoPanel;
    public GameObject exitPanel;
    public GameObject pausePanel;

    void Start()
    {
        DeactivatePanel(infoPanel);
        DeactivatePanel(exitPanel);
        DeactivatePanel(pausePanel);
        
        StartCoroutine(SpawnRandomObject());
    }

    public void ActivatePanel(GameObject panel)
    {
        foreach (var planet in planets)
            planet.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        blackHole.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        ufo.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        
        panel.SetActive(true);
    }
    
    public void DeactivatePanel(GameObject panel)
    {
        foreach (var planet in planets)
            planet.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        blackHole.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        ufo.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        
        panel.SetActive(false);
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