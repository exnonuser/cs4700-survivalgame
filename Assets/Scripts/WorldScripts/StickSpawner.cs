using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class StickSpawner : MonoBehaviour
{

    private const float Loop_Time = 2f; // Spawn 1 stick every 2 seconds
    public GameObject stick_model;
    public GameObject land;
    private const int MAX_STICKS = 100;
    private int current_sticks = 0;

    public static StickSpawner Instance { get; set; }
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
        PopulateSticks(30);
    }
    // Update is called once per frame
    void Update()
    {
        Stick_Spawn_Loop();
    }

    IEnumerable Stick_Spawn_Loop()
    {
        while (true)
        {
            SpawnStick();
            yield return new WaitForSeconds(Loop_Time);
        }
    }

    private GameObject SpawnStick()
    {
        if (current_sticks < MAX_STICKS)
        {
            Terrain terrain = land.GetComponent<Terrain>();
            int spawnX = Random.Range(0, 100);
            int spawnY = Random.Range(0, 100);
            int spawnZ = Random.Range(0, 100);
            Vector3 spawn_pos = new Vector3(spawnX, spawnY, spawnZ) + land.transform.position;
            spawn_pos.y = terrain.SampleHeight(spawn_pos);

            GameObject stick = Instantiate(stick_model, spawn_pos, Quaternion.identity);
            current_sticks++;

            return stick;
        }
        return new GameObject(); // Return empty game object if max sticks reached
        
    }

    private void PopulateSticks(int amount)
    {
        for (int i = 0; i < amount; i++) {
            SpawnStick();
        }
    }
    
    
}
