using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameManager;

public class UISkillTree : MonoBehaviour
{

    public GameManager GM;
    public static UISkillTree TalentTree;

    private void Awake() { 
        TalentTree = this;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    //public int[] SkillLevels;
    //public int[] SkillLimit;
    //public string[] SkillName;
    //public string[] SkillDescription;

    public TMP_Text RemainingSkillPoints;

    public List<PlayerSkills> PlayerSkillList;
    public GameObject PlayerSkillHolder;

    public List<DogfishSkills> DogfishSkillList;
    public GameObject DogfishSkillHolder;

    public KeyCode DebugSkillPoint = KeyCode.P;
    private void Start()
    {
        foreach (var playerSkill in PlayerSkillHolder.GetComponentsInChildren<PlayerSkills>()) PlayerSkillList.Add(playerSkill);
        UpdateAllSkillUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(DebugSkillPoint))
        {
            GainSkillPoint();
        }
    }

    public void GainSkillPoint()
    {
        GM.SkillPoints++;
        UpdateAllSkillUI();
    }

    public void UpdateAllSkillUI() {
        foreach (var playerSkills in PlayerSkillList) {
            playerSkills.UpdateUI();
        }
        RemainingSkillPoints.text = $"Remaining points: {GM.SkillPoints}";
    }
}
