using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class snapRect : MonoBehaviour
{
    public Transform content;

    public Scrollbar scrollbar;
    public float[] pos;
    float distance;
    float oldPos;

    public Button next;
    public Button prev;
    int currentPos;

    public Sprite selected;

    public Sprite unselected;

    public Transform pageContent;



    // Start is called before the first frame update
    void Start()
    {
        distance = 1f / (content.childCount - 1);
        pos = new float[content.childCount];

        for (int i = 0; i < content.childCount; i++)
        {
            pos[i] = distance * i;
            GameObject img = new GameObject();
            img.AddComponent<Image>();
            img.transform.localScale = Vector3.one;
            img.transform.SetParent(pageContent);
        }

        next.onClick.AddListener(() =>
        {
            StartCoroutine(nextBtn());
        });

        prev.onClick.AddListener(() =>
        {
            StartCoroutine(prevBtn());
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            oldPos = scrollbar.value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {

                if (oldPos < pos[i] + (distance / 2) && oldPos > pos[i] - (distance / 2))
                {
                    pageContent.GetChild(i).GetComponent<Image>().sprite = selected;
                    scrollbar.value = Mathf.Lerp(scrollbar.value, pos[i], 0.3f);
                    currentPos = i;
                }
                else
                {
                    pageContent.GetChild(i).GetComponent<Image>().sprite = unselected;

                }
            }
        }

    }

    IEnumerator nextBtn()
    {

        while (scrollbar.value < pos[currentPos + 1] - 0.2f)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, pos[currentPos + 1], 0.2f);

        }
        oldPos = scrollbar.value;
        yield return null;
    }

    IEnumerator prevBtn()
    {

        while (scrollbar.value > pos[currentPos - 1] + 0.2f)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, pos[currentPos - 1], 0.2f);

        }
        oldPos = scrollbar.value;
        yield return null;
    }

}
