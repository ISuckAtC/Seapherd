using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UISkillTree;

public class PlayerSkills : MonoBehaviour
{
    [Header("Each Skill needs a unique ID that tells the GameManager which skills are unlocked.")]
    public string SkillID;
    [Space(10f)]
    public int Value = 1;

    public int SkillLevel, SkillCap;

    public string SkillName, Description;
    public TMP_Text TitleText, DescriptionText;

    public GameObject[] ConnectedSkills;
    public Color NotBought = Color.white, BuyMore = Color.green, MaxedOut = Color.yellow;

    private void Awake()
    {
        if (SkillID == null)
        {
            Debug.LogWarning(gameObject.name + " is missing an ID.");
        }

        if (ConnectedSkills != null)
        {
            if (SkillLevel == 0)
            {
                foreach (var ConSkills in ConnectedSkills)
                {
                    ConSkills.SetActive(false);
                }
            }
        }
    }

    public void UpdateUI()
    {
        TitleText.text = $"{SkillName} - {SkillLevel}/{SkillCap}";
        DescriptionText.text = $"{Description} ";  // \n Cost: {Value}";

        GetComponent<Image>().color = SkillLevel >= SkillCap ? MaxedOut : SkillLevel >= 1 ? BuyMore : NotBought;
    }

    public void BuySkill()
    {
        if (TalentTree.GM.SkillPoints < 1 || SkillLevel >= SkillCap) return;
        TalentTree.GM.SkillPoints -= Value;
        SkillLevel++;
        if (ConnectedSkills != null)
        {
            if (SkillLevel == 1)
            {
                foreach (var ConSkills in ConnectedSkills)
                {
                    ConSkills.SetActive(true);
                }
            }
        }
        TalentTree.GM.UnlockSkill($"{SkillID}{SkillLevel}");

        TalentTree.UpdateAllSkillUI();
    }
}
