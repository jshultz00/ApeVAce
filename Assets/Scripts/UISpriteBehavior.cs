using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteBehavior : MonoBehaviour
{
    public Image image;
    public Sprite frame1;
    public Sprite frame2;

    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        image.sprite = frame1;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= 1f)
        {
            if(image.sprite == frame1)
            {
                image.sprite = frame2;
            } else
            {
                image.sprite = frame1;
            }
            timer = 0f;
        }
        timer += 1 * Time.deltaTime;
    }
}
