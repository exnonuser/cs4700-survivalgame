using TMPro;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public bool looked_at;
    public Transform plr_camera;
    private Vector3 orig_rotation;
    public TMP_Text text;
    private void Awake()
    {
        orig_rotation = transform.rotation.eulerAngles;
        text.text = "Press E to interact";
    }
    void LateUpdate()
    {
        // Make the UI element always face the main camera
        transform.LookAt(Camera.main.transform.position, Vector3.up);
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

        // Lock x axis since this makes text look weird
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x = orig_rotation.x;
        transform.rotation = Quaternion.Euler(rotation);
    }

    void Update()
    {
        
    }
}