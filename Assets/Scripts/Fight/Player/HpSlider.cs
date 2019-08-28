using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpSlider : MonoBehaviour
{
    [SerializeField]
    private GameObject middle;
    private float m_value;
    public void Init(int seat)
    {
        Value = 1f;
        if (RoomData.isMainRole(seat))
        {
            middle.GetComponent<SpriteRenderer>().sprite = ResourcesTools.getHpBar(1);
        }
        else
        {
            middle.GetComponent<SpriteRenderer>().sprite = ResourcesTools.getHpBar(2);
        }
    }
    public float Value
    {
        get
        {
            return m_value;
        }
        set
        {
            m_value = value;
            middle.transform.localScale = new Vector3(m_value, 1, 1);
            middle.transform.localPosition = new Vector3((1 - m_value) * -0.5f, 0, 0);
        }
    }
}
