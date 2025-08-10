using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirstBar : MonoBehaviour
{

    private Slider slider;

    public GameObject playerState;

    private float currentThirst, maxThirst;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentThirst = playerState.GetComponent<PlayerState>().currentHydration;
        maxThirst = playerState.GetComponent<PlayerState>().maxHydration;

        float fill = currentThirst / maxThirst;
        slider.value = Mathf.Lerp(slider.value, fill, 5f * Time.deltaTime);
    }
}
