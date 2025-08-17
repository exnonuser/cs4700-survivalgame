using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThirstBar : MonoBehaviour
{

    private Slider slider;
    public TMP_Text hydrationCounter;
    public GameObject playerState;

    private float currentHydration, maxHydration;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHydration = playerState.GetComponent<PlayerState>().currentHydration;
        maxHydration = playerState.GetComponent<PlayerState>().maxHydration;

        float fill = currentHydration / maxHydration;
        slider.value = Mathf.Lerp(slider.value, fill, 5f * Time.deltaTime);
        
        hydrationCounter.text = currentHydration + "%";

    }
}
