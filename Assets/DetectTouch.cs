using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DetectTouch : MonoBehaviour
{
    public GameObject circle;
    public List<Touch> touches = new List<Touch>();

    private ColorManager _colorManager;

    private void Start()
    {
        _colorManager = FindObjectOfType<ColorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        while (i < Input.touchCount)
        {
            UnityEngine.Touch t = Input.GetTouch(i);
            if (t.phase == TouchPhase.Began)
            {
                Debug.Log("touch began");
                touches.Add(createCircle(t));
            }
            else if (t.phase == TouchPhase.Ended)
            {
                Debug.Log("touch ended");
                Touch thisTouch = touches.Find(x => x.fingerId == t.fingerId);
                _colorManager.colors.Add(thisTouch.color);
                Destroy(thisTouch.gameObject);
                touches.RemoveAt(touches.IndexOf(thisTouch));
            }
            else if (t.phase == TouchPhase.Moved)
            {
                Debug.Log("touch is moving");
                Touch thisTouch = touches.Find(x => x.fingerId == t.fingerId);
                thisTouch.GetComponent<RectTransform>().anchoredPosition = ConvertToCanvasPos(t.position);
            }
            ++i;
        }
    }

    Touch createCircle(UnityEngine.Touch t)
    {
        GameObject c = Instantiate(circle, transform) as GameObject;
        c.name = "Touch" + t.fingerId;
        c.GetComponent<RectTransform>().anchoredPosition = ConvertToCanvasPos(t.position);
        Touch touchScript = c.GetComponent<Touch>();        
        c.transform.Find("Center").GetComponent<Image>().color = _colorManager.colors[0];
        touchScript.fingerId = t.fingerId;
        touchScript.color = _colorManager.colors[0];
        _colorManager.colors.RemoveAt(0);
        return c.GetComponent<Touch>();
    }


    Vector2 ConvertToCanvasPos(Vector2 touchPos)
    {
        Vector2 convertedPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), touchPos, Camera.main, out convertedPos);
        return convertedPos;
    }
}
