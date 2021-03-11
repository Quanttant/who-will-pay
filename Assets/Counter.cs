using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Counter : MonoBehaviour
{
    DetectTouch _touchManager;
    int counter = 0;

    // Start is called before the first frame update
    void Awake()
    {
        _touchManager = FindObjectOfType<DetectTouch>();
    }
    private void Update()
    {
        counter = _touchManager.counter > 0 ? (int)_touchManager.counter : 0;
        GetComponent<TextMeshProUGUI>().text = counter.ToString();
        if (_touchManager.selectedOne) GetComponent<TextMeshProUGUI>().enabled = false;
        else GetComponent<TextMeshProUGUI>().enabled = true;
    }
}
