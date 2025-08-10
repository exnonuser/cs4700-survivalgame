using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    // Player Health
    public float currentHealth;
    public float maxHealth = 100f;
    // Player Hunger
    public float currentHunger;
    public float maxHunger = 100f;
    // Player Hydration
    public float currentHydration;
    public float maxHydration = 100f;
    // Create singleton
    public static PlayerState Instance { get; set; }
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
    }

    // Update is called once per frame
    void Update()
    {
        // Regenerate health every 10 seconds
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            
            currentHealth -= 10f;
        }
    }

    IEnumerator HealthRegen(float regen_time, float regen_amount)
    {
        while (true) {
            
            currentHealth += regen_amount;
            yield return new WaitForSeconds(regen_time);
        }
    }
}
