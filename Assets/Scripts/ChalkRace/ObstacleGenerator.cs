using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclesPrefab;
    [SerializeField]private float generationInterval = 4f;

    private float lastGenerationTime;
    private Transform t_Limit, b_Limit;

    private void Start()
    {
        b_Limit = GameObject.Find("B_Limit").transform;
        t_Limit = GameObject.Find("T_Limit").transform;
    }

    void Update()
    {
        //COUNTDOWN
        if (Time.time - lastGenerationTime > generationInterval)
        {
            GenerateObstacle();
            //RESET TIME
            lastGenerationTime = Time.time;
        }
    }

    void GenerateObstacle()
    {
        //RANDOM POS
        Vector3 generationPosition = new Vector3(0, Random.Range(b_Limit.position.y, t_Limit.position.y), transform.position.z);
        //ESCALA ALEATORIA
        Vector3 randomScale = new Vector3(Random.Range(0.05f, 0.15f), Random.Range(0.05f, 0.1f), 0.1f);

        GameObject newObstacle = Instantiate(obstaclesPrefab[Random.Range(0,obstaclesPrefab.Length-1)], generationPosition, Quaternion.identity);

        //APLICAR ESCALA RANDOM
        newObstacle.transform.localScale = randomScale;

        //GIRAR SPRITE (RANDOM)
        bool flipY = (Random.Range(0, 2) == 1);
        bool flipX = (Random.Range(0, 2) == 1); 

        newObstacle.GetComponent<SpriteRenderer>().flipY = flipY;
        newObstacle.GetComponent<SpriteRenderer>().flipX = flipX;
    }
    public void IncreaseSpawnRate(float timeDecrease = 0.1f)
    {
        generationInterval -= timeDecrease;
        generationInterval = Mathf.Clamp(generationInterval, 0.5f, 10f);
        //Debug.Log("INCREASED SPAWN RATIO");
    }
}
