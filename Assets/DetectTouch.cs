using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VectorGraphics;

public class DetectTouch : MonoBehaviour
{
    public GameObject circle;
    public List<Touch> touches = new List<Touch>();

    public float counter = 3;
    public float maxTime = 3;

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
            if (2 <= Input.touchCount)
            {
                counter -= Time.deltaTime;
                if (counter <= 0)
                {
                    int randomIndex = Random.Range(0, Input.touchCount);
                    Touch selectedTouch = touches[randomIndex];
                    Camera.main.backgroundColor = selectedTouch.color;
                    touches.ForEach(x => Destroy(x.gameObject));
                }
            }
            UnityEngine.Touch t = Input.GetTouch(i);
            if (t.phase == TouchPhase.Began)
            {
                Debug.Log("touch began");
                touches.Add(createCircle(t));
                counter = maxTime;
            }
            else if (t.phase == TouchPhase.Ended)
            {
                Touch thisTouch = touches.Find(x => x.fingerId == t.fingerId);
                _colorManager.colors.Add(thisTouch.color);
                Destroy(thisTouch.gameObject);
                touches.RemoveAt(touches.IndexOf(thisTouch));
                counter = maxTime;
            }
            else if (t.phase == TouchPhase.Moved)
            {
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
        //c.transform.Find("Center").GetComponent<SVGImage>().color = _colorManager.colors[0];
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
