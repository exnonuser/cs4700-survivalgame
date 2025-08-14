using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    // Player Health
    public float currentHealth;
    public float maxHealth = 100f;
    // Player Hunger
    public float currentHunger;
    public float maxHunger = 100f;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;

    // Player Hydration
    public float currentHydration;
    public float maxHydration = 100f;
    //public bool isHydrationActive;

    // Create singleton
    public static PlayerState Instance { get; set; }

    private const float danger_threshold = 0.3f;
    private const float danger_damage = 1f;
    private bool in_danger = false;
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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentHydration = maxHydration;
        StartCoroutine(HealthRegen(2, 1)); // Start regen loop
        StartCoroutine(Hydration_Degrade(20, 5)); // Start losing thirst loop
    }

    // Update is called once per frame
    void Update()
    {

        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTravelled >= 5)
        {
            distanceTravelled = 0;
            currentHunger -= 1;
        }
        // Regenerate health every 10 seconds

        if (Input.GetKeyDown(KeyCode.N))
        {

            currentHealth -= 10f;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            currentHydration += 10f;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            currentHunger += 10f;
        }
    }

    IEnumerator HealthRegen(float regen_time, float regen_amount)
    {
        while (true)
        {
            // Regen or taking damage if in danger
            if (!in_danger)
            {
                currentHealth += regen_amount;
            }
            else
            {
                currentHealth -= danger_damage;
            }
            
            // Make sure current health is within bounds
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth; // Make sure regen doesnt go above max health
            }
            else if (currentHealth < 0)
            {
                currentHealth = 0;
            }

            // Get out of danger if hunger and thirst are sufficient
            float hunger_percent = currentHunger / maxHunger;
            float hydration_percent = currentHydration / maxHydration;
            if (hunger_percent >= danger_threshold && hydration_percent >= danger_threshold) {
                in_danger = false;
            }
            yield return new WaitForSeconds(regen_time);
        }
    }

    IEnumerator Hydration_Degrade(float degradation_time, float degradation_amount)
    {
        while (true)
        //while (isHydrationActive)
        {

            currentHydration -= degradation_amount;
            if (currentHydration < 0)
            {
                currentHydration = 0; // Make sure hydration is never below 0
            }

            // Take damage when too low
            if ((currentHydration / maxHydration) < danger_threshold)
            {
                in_danger = true;
            }
            yield return new WaitForSeconds(degradation_time);
        }
    }

    IEnumerator Hunger_Degrade(float hunger_time, float hunger_amount)
    {
        while (true)
        {

            currentHunger -= hunger_amount;
            if (currentHunger < 0)
            {
                currentHunger = 0; // Make sure hunger is never below 0
            }

            // Take damage when too low
            if ((currentHunger / maxHunger) < danger_threshold)
            {
                in_danger = true;
            }
            yield return new WaitForSeconds(hunger_time);
        }
    }

    public void setHealth(float newHealth) 
    {
        currentHealth = newHealth;
    }

    public void setHunger(float newHunger) 
    {
        currentHunger = newHunger;
    }

    public void setHydration(float newHydration) 
    {
        currentHydration = newHydration;
    }
}
