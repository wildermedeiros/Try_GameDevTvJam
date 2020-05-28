using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.UI;

public class WaveSystem : MonoBehaviour
{
    [System.Serializable]
    public class Wave {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    [SerializeField] float textPadding = 3f;
    [SerializeField] GameObject textGameObject;
    [SerializeField] TextMeshPro remainTimeText;
    [SerializeField] Wave[] waves;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] float timeBetweenWaves = 1f;
    [SerializeField] float waveCountdown;
    [SerializeField] float timeToBeatTheWave = 30f;
    [SerializeField] float timeLeftToWinTheWave;

    enum spawnState
    {
        Couting,
        Spawning,
        Waiting
    }

    spawnState state;
    Transform player;
    Transform textPosition;

    int nextWave = 0;
    float searchCountdown = 1f;
    
    private void Start() 
    {
        waveCountdown = timeBetweenWaves;
        timeLeftToWinTheWave = timeToBeatTheWave;
        remainTimeText.text = timeLeftToWinTheWave.ToString("0.0");
        player = GameObject.FindGameObjectWithTag("Player").transform;

        
    }

    private void Update() 
    {
        textGameObject.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + textPadding);
        if(state == spawnState.Waiting)
        {
            timeLeftToWinTheWave -= Time.deltaTime;
            remainTimeText.text = timeLeftToWinTheWave.ToString("0.0");

            if(!EnemyIsAlive() && timeLeftToWinTheWave > 0) // e o tempo não acabou
            {
                // next wave
                //Debug.Log("Wave completed");
                //return;
                SpawnNextWave();
                //timeLeftToWinTheWave = timeToBeatTheWave;
            }
            else
            {
                Debug.Log("Morreu");
                // deathsequence
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != spawnState.Spawning) 
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else 
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void SpawnNextWave()
    {
        state = spawnState.Couting;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            // finalsequence
            //nextWave = 0; caso queira resetar 
            Debug.Log("Cabou-se, looping");
            this.enabled = false;
        }
        else
        {
            nextWave++;
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        state = spawnState.Spawning;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1 / wave.rate); ;
        }

        state = spawnState.Waiting;

        yield break;
    }

    private void SpawnEnemy(Transform enemy)
    {
        Debug.Log("Spawn enemy");  
        int index = Random.Range(0, spawnPositions.Length); 
        Instantiate(enemy, spawnPositions[index].position, Quaternion.identity);
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }
}
