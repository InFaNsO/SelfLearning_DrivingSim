using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGroupController : MonoBehaviour
{
    [SerializeField] GameObject mCarPrefab;
    [SerializeField] List<Transform> mSpawnPositions;
    [SerializeField] float mMutationRate = 0.4f;













    [System.Serializable]
    public struct Spawner
    {
        public GameObject enemyType;
        public Transform spawnMarker;
    }

    [SerializeField] float mFirstSpawnTime = 2.0f;
    [SerializeField] float mSpawnDelay = 1.0f;
    [SerializeField] List<Spawner> mSpawners;
    private float mNextSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        mNextSpawnTime = mFirstSpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(mNextSpawnTime < Time.time)
        {
            foreach (var spawner in mSpawners)
            {
                //var enemy = Instantiate(spawner.enemyType, spawner.spawnMarker).GetComponent<EnemyController>();
                //enemy.GetComponent<Team>().faction = team.faction;
                //enemy.Start();
                //enemy.SetPath(mPath);
            }
            mNextSpawnTime += mSpawnDelay;
        }
    }
}
