using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDictEntry
{
    public string Name;
    public string Location;
    public int Points;
    public int Level;
}

[System.Serializable]
public class SpawnableEnemy
{
    public GameObject Object;
    public int Points;
    public int Level;
    public int Weight;

    public SpawnableEnemy(EnemyDictEntry Entry)
    {
        Object = Resources.Load<GameObject>(Entry.Location);
        if(Object == null)
        {
            Debug.LogError("Could not load the Resource for " + Entry.Name);
        }
        Points = Entry.Points;
        Level = Entry.Level;
        Weight = Level + Points;
    }
}

[System.Serializable]
public class ChoiceStruct<T>
{
    public int Weight;
    public List<T> ChoiceList;

    public ChoiceStruct()
    {
        Weight = 0;
        ChoiceList = new List<T>();
    }
};

public class EnemySpawner : MonoBehaviour
{
    public bool Active = false;
    public Dictionary<string, SpawnableEnemy> EnemyChoices;
    //TODO: SpawnWeights
    public float SpawnWidth = 3f;
    public float SpawnTimer = 1f;
    float LevelSpawnTimer;
    float CurSpawnTimer;

    //Level Spawn Data
    int CurLevel;
    public int LevelPointsAvailable;
    int LevelTotalWeights;
    int LargestPointsAvailable;
    ChoiceStruct<SpawnableEnemy>[] ThisLevelChoices;

    int LargestRatingInList = 0;

    public float RatingIncreaseRate = .2f;

    public GameEvent LevelEndEvent;

    Transform SpawnTransform;

    void Spawn(SpawnableEnemy Enemy)
    {
        Vector3 EnemyPos = new Vector3(Random.Range(-SpawnWidth, SpawnWidth), SpawnTransform.position.y, SpawnTransform.position.z);

        GameObject NewEnemy = Instantiate(Enemy.Object, EnemyPos, SpawnTransform.rotation);
        LevelPointsAvailable -= Enemy.Points;
        if(LevelPointsAvailable == 0) { Active = false; }
    }

    public void StartLevel(int Level)
    {
        CurLevel = Level;
        LevelPointsAvailable = Level * 14;
        LevelSpawnTimer = 1 / (Mathf.Log(CurLevel * 0.2f) + 2) + 0.4f;
        CurSpawnTimer = LevelSpawnTimer;
        LevelTotalWeights = 0;
        LargestPointsAvailable = LargestRatingInList;
        ThisLevelChoices = new ChoiceStruct<SpawnableEnemy>[LargestRatingInList];
        for(int i = 0; i < ThisLevelChoices.Length; i++)
        {
            ThisLevelChoices[i] = new ChoiceStruct<SpawnableEnemy>();
        }

        foreach(var Enemy in EnemyChoices)
        {
            if(CurLevel >= Enemy.Value.Level)
            {
                ThisLevelChoices[Enemy.Value.Points - 1].Weight += Enemy.Value.Weight;
                ThisLevelChoices[Enemy.Value.Points - 1].ChoiceList.Add(Enemy.Value);
            }
        }
        Active = true;
        Debug.Log("Level" + CurLevel + "(DifficultyOffset: " + (int)Mathf.Log(10 * CurLevel) + ", Total: " + LevelPointsAvailable + ")");
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
        EnemyChoices = new Dictionary<string, SpawnableEnemy>();
        int i = 0;
        foreach (var Enemy in EnemyDict)
        {
            EnemyChoices[Enemy.Name] = new SpawnableEnemy(Enemy);
            if(Enemy.Points > LargestRatingInList) { LargestRatingInList = Enemy.Points; }
            i++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnTransform = GetComponent<Transform>();
        
        StartLevel(3);
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            CurSpawnTimer -= Time.deltaTime;

            //Debug.Log(CurSpawnTimer);
            //Spawn logic goes here
            if (CurSpawnTimer <= 0 && LevelPointsAvailable > 0)
            {
                // Reset the timer with additional time it might have been missing, to be more consistent
                CurSpawnTimer += LevelSpawnTimer;

                //Make sure LargestPointAvailable reflects reality
                if (LargestPointsAvailable > LevelPointsAvailable)
                {
                    LargestPointsAvailable = LevelPointsAvailable;
                }
                while (LargestPointsAvailable > 0 && ThisLevelChoices[LargestPointsAvailable - 1].ChoiceList.Count == 0)
                {
                    LargestPointsAvailable--;
                }
                if (LargestPointsAvailable <= 0) { Debug.LogError("Ran out of enemies to spawn, please make sure the level has low point enemies for filler"); Active = false; return; }

                //Do the actual spawning here 

                //Choose enemy rating here
                int EnemyRating = Mathf.Min(Random.Range(0, LargestPointsAvailable) + CurLevel * (int)RatingIncreaseRate, LargestPointsAvailable - 1);
                while(ThisLevelChoices[EnemyRating].ChoiceList.Count == 0)
                { // Make sure there are enemies to spawn of that rating or lower
                    EnemyRating--;
                    if(EnemyRating == 0)
                    {
                        Debug.LogError("Ran out of enemies to spawn, please make sure the level has low point enemies for filler"); Active = false; return;
                    }
                }

                //If here then there are choices to pick from
                int EnemyChoice = Random.Range(0, ThisLevelChoices[EnemyRating].Weight);
                foreach(var Enemy in ThisLevelChoices[EnemyRating].ChoiceList)
                {
                    EnemyChoice -= Enemy.Weight;
                    if(EnemyChoice <= 0)
                    {
                        Spawn(Enemy);
                        break;
                    }
                }
            }
        }
    }
}
