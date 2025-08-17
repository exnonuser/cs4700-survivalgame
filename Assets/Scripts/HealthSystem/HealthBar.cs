using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public TMP_Text healthCounter;

    public GameObject playerState;

    private float currentHealth, maxHealth;
    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

        float fill = currentHealth / maxHealth; //0-1
        //slider.value = Mathf.Lerp(slider.value, fill, 5f * Time.deltaTime); // Lerp for smoothing value changes
        slider.value = fill;
        healthCounter.text = currentHealth + "/" + maxHealth;
    }

    
}
