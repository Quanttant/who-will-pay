using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Touch : MonoBehaviour
{
    public int fingerId;
    public Color color;

    private ColorManager _colorManager;
    private DetectTouch _touchManager;

    private void Start()
    {
        _colorManager = FindObjectOfType<ColorManager>();
        _touchManager = FindObjectOfType<DetectTouch>();
        _touchManager.touches.Add(this);

        //transform.Find("Center").GetComponent<SVGImage>().color = _colorManager.colors[0];
        color = _colorManager.colors[0];
        transform.Find("Center").GetComponent<Image>().color = color;
        _colorManager.colors.RemoveAt(0);
    }

    public void Won()
    {
        Camera.main.backgroundColor = color;
        transform.Find("Background").gameObject.SetActive(true);
    }

    public void Lost()
    {
        transform.Find("Center").GetComponent<Image>().enabled = false;
    }

    private void OnDestroy()
    {
        _colorManager.colors.Add(color);
        _touchManager.touches.Remove(this);
    }
}
