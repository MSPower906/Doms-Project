using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameUI;
    [SerializeField] private Image healthBarUI;
    [SerializeField] private Image manaBarUI;
    public AbilityUIs[] abilityUI;


    public void SetUI(CharacterStatsScript stats)
    {
        nameUI.text = stats.name;
        healthBarUI.fillAmount = stats.currentHealth / stats.maxHealth;
        manaBarUI.fillAmount = stats.currentMana / stats.maxMana;

        for (int i = 0; i < stats.abilities.Length; i++)
        {
            abilityUI[i].abilityNameUI.text = stats.abilities[i].abilName;
            abilityUI[i].abilityDescUI.text = stats.abilities[i].abilDesc;
            abilityUI[i].abilityDamageNumber.text = stats.abilities[i].damage.ToString("f0");
            abilityUI[i].abilityManaCost.text = stats.abilities[i].manaCost.ToString("f0");
        }
    }


    public void TargetSelected()
    {
        //Remove the UI options from the battlefield 
    }
}

[Serializable]
public class AbilityUIs
{
    public TextMeshProUGUI abilityNameUI;
    public Image abilityTypeUI;
    public TextMeshProUGUI abilityDescUI;
    public TextMeshProUGUI abilityDamageNumber;
    public TextMeshProUGUI abilityManaCost;
}
