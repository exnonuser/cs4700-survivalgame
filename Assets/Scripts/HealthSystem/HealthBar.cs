using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private Slider slider;

    public GameObject playerState;

    private float currentHealth, maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

        float fill = currentHealth / maxHealth;
        slider.value = Mathf.Lerp(slider.value, fill, 5f * Time.deltaTime); // Lerp for smoothing value changes
    }
}
