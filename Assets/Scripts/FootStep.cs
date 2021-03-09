using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour, IPoolObject
{
    // public float timeLife;

    private float timeBeforeFade;
    private float timeFade;

    private float m_timer;
    private SpriteRenderer m_renderer;

    void Awake()
    {
        m_renderer = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer > timeBeforeFade)
        {
            float alpha = 1 - Mathf.Lerp(0, 1, (m_timer - timeBeforeFade)/timeFade);
            m_renderer.color = new Color (m_renderer.color.r, m_renderer.color.g, m_renderer.color.b, alpha);
            if (m_timer > timeBeforeFade + timeFade)
            {
                m_timer = 0.0f;
                Deactivate();
            }
        }
    }

    public void Initialize(float timeBeforeFade, float timeFade)
    {
        this.timeBeforeFade = timeBeforeFade;
        this.timeFade = timeFade;
    }

    public void Reset()
    {
        m_renderer.color = new Color (m_renderer.color.r, m_renderer.color.g, m_renderer.color.b, 1);
        m_timer = 0.0f;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }
    public void Deactivate()
    {        
        gameObject.SetActive(false);
    }


    // static void Deactivate_RendererDisable(GameObject obj)
    // {
    //     obj.GetComponent<Renderer>().enabled = false;
    // }


}
