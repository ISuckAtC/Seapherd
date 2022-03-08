using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UISkillTree;

public class PlayerSkills : MonoBehaviour
{
    public int id, Value = 1;

    public int SkillLevel, SkillCap;

    public string SkillName, Description;
    public TMP_Text TitleText, DescriptionText;

    public string[] ConnectedSkills;
    public Color NotBought = Color.white, BuyMore = Color.green, MaxedOut = Color.yellow; 

    public void UpdateUI()
    {
        TitleText.text = $"{SkillName} - {SkillLevel}/{SkillCap}";
        DescriptionText.text = $"{Description} ";  // \n Cost: {Value}";

        GetComponent<Image>().color = SkillLevel >= SkillCap ? MaxedOut : SkillLevel >= 1 ? BuyMore : NotBought;
    }

    public void BuySkill()
    {
        if (TalentTree.SkillPoints < 1 || SkillLevel >= SkillCap) return;
        TalentTree.SkillPoints -= Value;
        SkillLevel++;
        TalentTree.UpdateAllSkillUI();
    }
}
