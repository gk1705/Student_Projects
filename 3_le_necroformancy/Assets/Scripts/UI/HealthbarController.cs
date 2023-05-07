using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
    [SerializeField]
    private int m_maxHealth;

    [SerializeField]
    private int m_currentHealth;

    [SerializeField]
    private Canvas m_prefab;
    private Canvas m_healthBar;
    private Image bar;



    public int health
    {
        get { return m_currentHealth; }
        set { m_currentHealth = value; }
    }

    // Use this for initialization
    void Start()
    {
        m_healthBar = Instantiate(m_prefab);

        foreach (var image in m_healthBar.GetComponentsInChildren<Image>())
        {
            if (image.name == "Bar")
                bar = image;
        }
    }


    // Update is called once per frame
        void Update ()
    {
        var position = gameObject.transform.position;
        position.y += 1.5f;
        m_healthBar.transform.position = position;

        var angles = Camera.main.transform.eulerAngles;
        m_healthBar.transform.eulerAngles = angles;

        bar.fillAmount = Mathf.InverseLerp(0, m_maxHealth, m_currentHealth);
    }

}
