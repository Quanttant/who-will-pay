using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetTouchColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = transform.GetComponentInParent<Touch>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
