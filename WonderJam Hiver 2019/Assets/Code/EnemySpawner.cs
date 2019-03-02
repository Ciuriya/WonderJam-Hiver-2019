using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDictEntry
{
    public string Name;
    public string Location;
    public int Points;
}

public class SpawnableEnemy
{
    public GameObject Object;
    public int Points;
    public int Weight;

    public SpawnableEnemy(EnemyDictEntry Entry)
    {
        Object = Resources.Load<GameObject>(Entry.Location);
        Points = Entry.Points;
    }
}

public class EnemySpawner : MonoBehaviour
{
    public SpawnableEnemy[] EnemyChoices;
    public float SpawnWidth = 4f;

    List<GameObject> SpawnedEnemies;

    //Level Spawn Data
    public int LevelPointsAvailable;
    int LevelTotalWeights;

    Transform SpawnTransform;

    void Spawn(int index)
    {
        if (EnemyChoices.Length > index && EnemyChoices[index].Object != null)
        {
            Vector3 EnemyPos = new Vector3(Random.Range(-SpawnWidth, SpawnWidth), SpawnTransform.position.y, SpawnTransform.position.z);
            Debug.Log(EnemyPos.x);
            GameObject NewEnemy = Instantiate(EnemyChoices[index].Object, EnemyPos, SpawnTransform.rotation);
            SpawnedEnemies.Add(NewEnemy);
            LevelPointsAvailable -= EnemyChoices[index].Points;
        }
    }

    public void StartNewLevel(int Points)
    {
        LevelPointsAvailable = Points;
        LevelTotalWeights = 0;
        for(int i = 0; i < EnemyChoices.Length; i++)
        {
            if(EnemyChoices[i].Weight > 0)
            {
                LevelTotalWeights += EnemyChoices[i].Weight;
            }
        }
    }

    public bool IsLevelOver()
    {
        return SpawnedEnemies.Count == 0 && LevelPointsAvailable == 0;
    }

    //Run once before any Start has been run
    private void Awake()
    {
        List<EnemyDictEntry> EnemyDict = new List<EnemyDictEntry>();
        //Load the enemy data here
        System.IO.StreamReader Reader = new System.IO.StreamReader("Assets/Data/EnemyDictionary.JSON");
        for (string JSON = Reader.ReadLine(); JSON != null; JSON = Reader.ReadLine())
        {
            EnemyDict.Add(JsonUtility.FromJson<EnemyDictEntry>(JSON));
        }

        //Transform the enemy data to actionnable objects here
        EnemyChoices = new SpawnableEnemy[EnemyDict.Count];
        int i = 0;
        foreach (var Enemy in EnemyDict)
        {
            Debug.Log(Enemy.Name);
            EnemyChoices[i] = new SpawnableEnemy(Enemy);
            i++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnTransform = GetComponent<Transform>();

        SpawnedEnemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //Remove Dead objects to keep only the live ones
        SpawnedEnemies.RemoveAll((Enemy) => { return Enemy == null; });

        //Spawn logic goes here
        if(LevelPointsAvailable > 0)
        {
            if(SpawnedEnemies.Count == 0)
            { //TODO: Always spawn in the biggest class you can when the screen gets cleared
                Spawn(0);
            }
        }

        foreach(var Enemy in SpawnedEnemies)
        { //Action to perform on all enemies each frame
            Debug.Log(Enemy);
        }
    }
}
