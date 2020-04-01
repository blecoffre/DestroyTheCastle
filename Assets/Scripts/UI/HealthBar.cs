using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image m_lifeBar = default;

    public void UpdateHealthBar(float percent)
    {
        m_lifeBar.fillAmount = percent;
    }
}
