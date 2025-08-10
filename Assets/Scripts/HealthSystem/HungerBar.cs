using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{

    private Slider slider;

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
    }
}
