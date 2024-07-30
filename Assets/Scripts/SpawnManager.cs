using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Objetos SpawnPoint
    public GameObject[] spawnPoint;
    // Objetos dos inimigos a serem instanciados
    public GameObject[] enemyPrefabs;

    // Numero de spawnPoints no mapa
    public int spawnsTotal;
    // Numero maximo de monstros no prefab
    int maxPrefab;


    // Funcao recebe o numero de spawns.
    void SpawnEnemy(int numSpawns)
    {
        // Contador do array de inimigos para instanciacao
        int arrayPos = 0;
        // Posicao do spawnPoint no eixo X
        float posX;
        // Posicao da spawnPoint no eixo Y
        float posY;

        maxPrefab = enemyPrefabs.Length;

        // Entra na funcao até o numero de spawns atigir o requirido.
        if (arrayPos < numSpawns)
        {
            // Pegar o numero de spawnPoints
            spawnsTotal = spawnPoint.Length;

            // verifica um spawnPoint randomicamente
            int randomPos = Random.Range(0, spawnsTotal);

            // Verifica se o spawnPoint ecolhido nao esta ocupado
            if (spawnPoint[randomPos].activeInHierarchy) //tiramos o hide pois não tinha contexto no script
            {
                int difMonster = Random.Range(0, maxPrefab);

                // Pega a posicao do spawnPoint
                Vector2 spawnPos = new Vector2(spawnPoint[randomPos].transform.position.x, spawnPoint[randomPos].transform.position.y);

                // Spawn do objeto
                GameObject newEnemy = Instantiate(enemyPrefabs[1], spawnPos, Quaternion.identity);

                arrayPos++;
            }
            else
            {
                SpawnEnemy(numSpawns);
            }
        }
    }
}
