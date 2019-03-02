using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDictEntry
{
    public string Name;
    public string Location;
    public int Weight;
}

public class SpawnableEnemy
{
    public GameObject Object;
    public int EnemyPoints;
    public int EnemyWeight;

    public SpawnableEnemy(EnemyDictEntry Entry)
    {
        Object = Resources.Load<GameObject>(Entry.Location);
        EnemyWeight = Entry.Weight;
    }
}

public class EnemySpawner : MonoBehaviour
{
    public SpawnableEnemy[] EnemyChoices;
    public float SpawnWidth = 300;
    public float SpawnCount = 1;

    List<GameObject> SpawnedEnemies;

    Transform SpawnTransform;

    void Spawn(int index)
    {
        if (EnemyChoices.Length > index && EnemyChoices[index].Object != null)
        {
            Vector3 EnemyPos = new Vector3(Random.Range(-1, 1) * SpawnWidth, SpawnTransform.position.y, SpawnTransform.position.z);
            GameObject NewEnemy = Instantiate(EnemyChoices[0].Object, EnemyPos, SpawnTransform.rotation);
            SpawnedEnemies.Add(NewEnemy);
        }
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

        if(SpawnedEnemies.Count < SpawnCount)
        {
            //Spawn(0);
        }

        foreach(var Enemy in SpawnedEnemies)
        { //Action to perform on all enemies each frame
            Debug.Log(Enemy);
        }
    }
}
