using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VectorGraphics;

public class DetectTouch : MonoBehaviour
{
    public GameObject circle;
    public List<Touch> touches = new List<Touch>();

    public float counter = 3.99f;
    public float maxTime = 3.99f;

    private ColorManager _colorManager;

    [HideInInspector]
    public bool selectedOne = false;

    private void Start()
    {
        _colorManager = FindObjectOfType<ColorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
        {
            if (selectedOne)
            {
                selectedOne = false;
                //Camera.main.backgroundColor = _colorManager.backgroundColor;
            }
        }

        if (2 <= Input.touchCount)
        {
            counter -= Time.deltaTime;
            if (counter <= 0.85 && !selectedOne)
            {
                selectedOne = true;
                int randomIndex = Random.Range(0, Input.touchCount);
                Touch selectedTouch = touches[randomIndex];
                foreach (Touch touch in touches)
                    if (touch != selectedTouch) touch.Lost();
                    else touch.Won();
            }
        }
        int i = 0;
        while (i < Input.touchCount)
        {
            UnityEngine.Touch t = Input.GetTouch(i);
            if (touches.Find(x => x.fingerId == t.fingerId) == null && selectedOne) return;
            if (t.phase == TouchPhase.Began)
            {
                if (!selectedOne)
                {
                    createCircle(t);
                    counter = maxTime;
                }
            }
            else if (t.phase == TouchPhase.Ended)
            {
                counter = maxTime;
                Touch thisTouch = touches.Find(x => x.fingerId == t.fingerId);
                //if (thisTouch.didWin)
                    //thisTouch.animator.SetTrigger("endWin");
                //else
                thisTouch.animator.SetTrigger("endTouch");
                
                Destroy(thisTouch.gameObject, 1);

            }
            else if (t.phase == TouchPhase.Moved)
            {
                Touch thisTouch = touches.Find(x => x.fingerId == t.fingerId);
                thisTouch.GetComponent<RectTransform>().anchoredPosition = ConvertToCanvasPos(t.position);
            }
            i++;
        }
    }

    void createCircle(UnityEngine.Touch t)
    {
        GameObject c = Instantiate(circle, transform) as GameObject;
        c.name = "Touch_" + t.fingerId;
        c.GetComponent<RectTransform>().anchoredPosition = ConvertToCanvasPos(t.position);
        Touch touchScript = c.GetComponent<Touch>();
        touchScript.fingerId = t.fingerId;
    }


    Vector2 ConvertToCanvasPos(Vector2 touchPos)
    {
        Vector2 convertedPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), touchPos, Camera.main, out convertedPos);
        return convertedPos;
    }
}
