using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropTriggerEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
            if (other.tag == "Player") {
            EventManager.Instance.TriggerEvent("AddScore");
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
