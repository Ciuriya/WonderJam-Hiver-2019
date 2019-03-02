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
        if(Object == null)
        {
            Debug.LogError("Could not load the Resource for " + Entry.Name);
        }
        Points = Entry.Points;
    }
}

public class EnemySpawner : MonoBehaviour
{
    public bool Active = false;
    public SpawnableEnemy[] EnemyChoices;
    public float SpawnWidth = 4f;

    List<System.Tuple<SpawnableEnemy, GameObject>> SpawnedEnemies;

    //Level Spawn Data
    int CurLevel;
    public int LevelPointsAvailable;
    int LevelTotalWeights;
    int LargestPointsAvailable;
    List<SpawnableEnemy>[] ThisLevelChoices;

    Transform SpawnTransform;

    void Spawn(int index)
    {
        if (EnemyChoices.Length > index && EnemyChoices[index].Object != null)
        {
            Vector3 EnemyPos = new Vector3(Random.Range(-SpawnWidth, SpawnWidth), SpawnTransform.position.y, SpawnTransform.position.z);
            GameObject NewEnemy = Instantiate(EnemyChoices[index].Object, EnemyPos, SpawnTransform.rotation);
            SpawnedEnemies.Add(new System.Tuple<SpawnableEnemy, GameObject>(EnemyChoices[index], NewEnemy));
            LevelPointsAvailable -= EnemyChoices[index].Points;
        }
    }

    public void StartNewLevel(int Level, int Points)
    {
        CurLevel = Level;
        LevelPointsAvailable = Points;
        LevelTotalWeights = 0;
        LargestPointsAvailable = EnemyChoices[EnemyChoices.Length - 1].Points;
        ThisLevelChoices = new List<SpawnableEnemy>[LargestPointsAvailable];
        for(int i = 0; i < ThisLevelChoices.Length; i++)
        {
            ThisLevelChoices[i] = new List<SpawnableEnemy>();
        }
        for(int i = 0; i < EnemyChoices.Length; i++)
        {
            if(EnemyChoices[i].Weight > 0)
            {
                LevelTotalWeights += EnemyChoices[i].Weight;
                ThisLevelChoices[EnemyChoices[i].Points - 1].Add(EnemyChoices[i]);
            }
        }
        Active = true;
        Debug.Log("Level " + CurLevel + ": Concurrent" + (int)(Mathf.Log(10 * CurLevel) + 1) + ", Total: " + Points);
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

        //Order the list by the number of points it has
        EnemyDict.Sort(new System.Comparison<EnemyDictEntry>((EnemyDictEntry first, EnemyDictEntry second) => { return first.Points - second.Points; }));

        //Transform the enemy data to actionnable objects here
        EnemyChoices = new SpawnableEnemy[EnemyDict.Count];
        int i = 0;
        foreach (var Enemy in EnemyDict)
        {
            EnemyChoices[i] = new SpawnableEnemy(Enemy);
            i++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnTransform = GetComponent<Transform>();

        SpawnedEnemies = new List<System.Tuple<SpawnableEnemy, GameObject>>();

        EnemyChoices[0].Weight = 1;
        StartNewLevel(1, 14);
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            //Remove Dead objects to keep only the live ones
            SpawnedEnemies.RemoveAll((Enemy) => { return Enemy.Item2 == null; });
            int CurPoints = 0;
            foreach (var Enemy in SpawnedEnemies) { CurPoints += Enemy.Item1.Points; }

            //Spawn logic goes here
            if (LevelPointsAvailable > 0)
            {
                //Make sure LargestPointAvailable reflects reality
                if (LargestPointsAvailable > LevelPointsAvailable)
                {
                    LargestPointsAvailable = LevelPointsAvailable;
                }
                while (LargestPointsAvailable > 0 && ThisLevelChoices[LargestPointsAvailable - 1].Count == 0)
                {
                    LargestPointsAvailable--;
                }
                if (LargestPointsAvailable <= 0) { Debug.LogError("Ran out of enemies to spawn, please make sure the level has low point enemies for filler"); Active = false; return; }
                //Debug.Log("Cur: " + SpawnedEnemies.Count + "(" + CurPoints + ")");
                if (SpawnedEnemies.Count == 0)
                { //Always spawn the biggest enemy you can if the screen was cleared
                    Spawn(Random.Range(0, ThisLevelChoices[LargestPointsAvailable - 1].Count - 1));
                }
                else if (CurPoints < (int)Mathf.Log(10 * CurLevel) + 1)
                {
                    Spawn(Mathf.Max(LargestPointsAvailable - 1, Random.Range(0, ThisLevelChoices[LargestPointsAvailable - 1].Count - 1) + (int)Mathf.Log10(CurLevel)));
                }
            } else if(SpawnedEnemies.Count == 0) { Active = false; }
        }
    }
}
