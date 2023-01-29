using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI actionPointsText;

    [SerializeField]
    private Units unit;

    [SerializeField]
    private Image healthBarImage;

    [SerializeField]
    private HealthSystem healthSystem;

    private void Start()
    {
        Units.OnAnyActionPointsChanged += Units_OnAnyActionPointsChanged;
        healthSystem.OnDamage += HealthSystem_OnDamage;

        UpdateActionPointstext();
        UpdateHealthBar();
    }

    private void UpdateActionPointstext()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void Units_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointstext();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void HealthSystem_OnDamage(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}
