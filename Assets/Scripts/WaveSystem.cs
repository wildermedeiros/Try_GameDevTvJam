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
        public Transform enemyType2;
        public int count;
        public int countType2;
        public float rate;
    }

    [SerializeField] float textPadding = 3f;
    //[SerializeField] GameObject textGameObject;
    [SerializeField] TextMeshProUGUI remainTimeText;
    [SerializeField] Wave[] waves;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] Transform[] spawnTurretsPoint;
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
        //textGameObject.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + textPadding);

        if (timeLeftToWinTheWave <= 0)
        {
            remainTimeText.text = "0.0f";
            // e reseta a cena
            // "lose" sequence
            print("end game");
        }

        if(state == spawnState.Waiting)
        {
            timeLeftToWinTheWave -= Time.deltaTime;
            remainTimeText.text = timeLeftToWinTheWave.ToString("0.0");

            if(!EnemyIsAlive() && timeLeftToWinTheWave > 0) // e o tempo não acabou
            {
                // next wave
                //Debug.Log("Wave completed");
                SpawnNextWave();
                //timeLeftToWinTheWave = timeToBeatTheWave;
            }
            else
            {
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
            // "win" sequence
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
            yield return new WaitForSeconds(1 / wave.rate);
        }

        for (int i = 0; i < wave.countType2; i++)
        {
            SpawnTurret(wave.enemyType2);
            yield return new WaitForSeconds(1 / wave.rate);
        }

        state = spawnState.Waiting;

        yield break;
    }

    private void SpawnEnemy(Transform enemy)
    {
        Debug.Log("Spawn enemy");  
        if(enemy != null)
        {
            int index = Random.Range(0, spawnPositions.Length); 
            Instantiate(enemy, spawnPositions[index].position, Quaternion.identity);
        }
    }

    void SpawnTurret(Transform enemyType2)
    {
        Debug.Log("Spawn enemy");
        if (enemyType2 != null)
        {
            int index = Random.Range(0, spawnTurretsPoint.Length);
            Instantiate(enemyType2, spawnTurretsPoint[index].position, Quaternion.identity);
        }
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
    
    void IEnumerator () 
    {

    }
}
