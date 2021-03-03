using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    // Start is called before the first frame update

    enum Emotion
    {
        NEUTRAL,
        ANGRY,
        HAPPY
    }

    Emotion emotion;
    public bool emotionIsDirty;
    private Renderer renderer;
    private float timerTillNextChange = 0;

    void Awake()
    {
        emotion = Emotion.ANGRY; 
        renderer = GetComponent<Renderer>();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (timerTillNextChange <= 0)
        {
            emotionIsDirty = true;
        }
        else
        {
            timerTillNextChange -= Time.deltaTime;
        }



        if (emotionIsDirty)
        {
            // gameObject.SetActive(!gameObject.activeSelf);
            renderer.enabled = !renderer.enabled;
            emotionIsDirty = false;
            timerTillNextChange = Random.Range(1f, 5f);
        }        
    }
}
