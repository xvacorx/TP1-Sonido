using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        public bool spawnMonster;
        public GameObject spawnEffect;
    }

    [Header("References")]
    [SerializeField] private SpawnableObject[] objects;    

    public void Spawn()
    {
        foreach (var obj in objects)
        {
            if (obj.spawnMonster)
            {
                StartCoroutine(SpawnEnemy(obj.spawnEffect, obj.prefab));
                break;
            }
        }
    }

    private IEnumerator SpawnEnemy(GameObject effect, GameObject prefab)
    {
        GameObject smoke = Instantiate(effect, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.2f);

        Instantiate(prefab, transform.position, Quaternion.identity);

        Destroy(smoke, 1f);
    }
}
