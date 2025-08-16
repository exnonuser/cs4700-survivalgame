using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ItemsSpawner : MonoBehaviour
{

    private const float Loop_Time = 2f; // Spawn 1 stick every 2 seconds
    public GameObject stick_model;
    public GameObject apple_model;
    public GameObject stones_model;
    public GameObject coal_model;
    public GameObject land;

    // Max values
    private const int MAX_STICKS = 100;
    private const int MAX_APPLES = 20;
    private const int MAX_STONES = 50;
    private const int MAX_COAL = 20;
    // Current values
    private int current_sticks = 0;
    private int current_stones = 0;
    private int current_apples = 0;
    private int current_coals = 0;

    public static ItemsSpawner Instance { get; set; }
    void Awake()

    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        StartCoroutine("Stick_Spawn_Loop");
        StartCoroutine("Stone_Spawn_Loop");
        StartCoroutine("Apple_Spawn_Loop" );
        StartCoroutine("Coal_Spawn_Loop" );
    }
   

    IEnumerator Stick_Spawn_Loop()
    {
        while (true)
        {
            SpawnItem(stick_model, ref current_sticks, MAX_STICKS);
            yield return new WaitForSeconds(Loop_Time);
        }
    }
    IEnumerator Apple_Spawn_Loop()
    {
        while (true)
        {
            SpawnItem(apple_model, ref current_apples, MAX_APPLES);
            yield return new WaitForSeconds(Loop_Time * 5);
        }
    }
    IEnumerator Stone_Spawn_Loop()
    {
        while (true)
        {
            SpawnItem(stones_model, ref current_stones, MAX_STONES);
            yield return new WaitForSeconds(Loop_Time * 2);
        }
    }

    IEnumerator Coal_Spawn_Loop()
    {
        while (true)
        {
            SpawnItem(coal_model, ref current_coals, MAX_COAL);
            yield return new WaitForSeconds(Loop_Time * 5);
        }
    }

    private GameObject SpawnItem(GameObject model, ref int current_val, int max_val)
    {
        if (current_val < max_val)
        {
            Terrain terrain = land.GetComponent<Terrain>();
            int spawnX = Random.Range(0, 100);
            int spawnY = Random.Range(0, 100);
            int spawnZ = Random.Range(0, 100);
            Vector3 spawn_pos = new Vector3(spawnX, spawnY, spawnZ) + land.transform.position;
            spawn_pos.y = terrain.SampleHeight(spawn_pos);

            GameObject stick = Instantiate(model, spawn_pos, Quaternion.identity);
            current_val++;

            return stick;
        }
        return new GameObject(); // Return empty game object if max sticks reached

    }

    private void PopulateItems(GameObject model, int amount, ref int current_val, int max_val)
    {
        for (int i = 0; i < amount; i++) {
            SpawnItem(model, ref current_val, max_val);
        }
    }
    
    
}
