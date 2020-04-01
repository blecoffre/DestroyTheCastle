using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Manager;

public class UnitUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_followingUIPrefab;

    private Transform m_followingUITransform;
    private Vector3 targetPos;

    private HealthBar m_healthBar;

    public void Initialize()
    {
        //Temp : TODO Get Canvas from manager ? Controller ?

        if (m_followingUIPrefab && MatchUIManager.Instance.GetEntityUIContainer())
            m_followingUITransform = Instantiate(m_followingUIPrefab, MatchUIManager.Instance.GetEntityUIContainer()).transform;

        m_healthBar = m_followingUITransform?.GetComponent<HealthBar>();
        m_healthBar?.UpdateHealthBar(1.0f);
    }

    public HealthBar GetHealthBar()
    {
        return m_healthBar;
    }

    private void Update()
    {
        targetPos = transform.position;
        targetPos.y += 1; //Offset

        if (m_followingUITransform)
            m_followingUITransform.position = Camera.main.WorldToScreenPoint(targetPos);
    }

    public void DestroyUI()
    {
        Destroy(m_followingUITransform.gameObject);
    }
}
