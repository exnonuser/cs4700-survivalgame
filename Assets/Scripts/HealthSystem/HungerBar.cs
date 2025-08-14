using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HungerBar : MonoBehaviour
{

    private Slider slider;
    public TMP_Text hungerCounter;
    public GameObject playerState;

    private float currentHunger, maxHunger;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHunger = playerState.GetComponent<PlayerState>().currentHunger;
        maxHunger = playerState.GetComponent<PlayerState>().maxHunger;

        float fill = currentHunger / maxHunger;
        slider.value = Mathf.Lerp(slider.value, fill, 5f * Time.deltaTime);

        hungerCounter.text = currentHunger + "/" + maxHunger;
    }
}
