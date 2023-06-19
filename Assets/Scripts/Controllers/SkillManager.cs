using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DashSkill))]
[RequireComponent(typeof(CloneSkill))]
[RequireComponent(typeof(SwordSkill))]
[RequireComponent(typeof(BlackholeSkill))]
public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    #region Skills
    public DashSkill Dash { get; private set; }
    public CloneSkill Clone { get; private set; }
    public SwordSkill Sword { get; private set; }
    public BlackholeSkill Blackhole { get; private set; }
    #endregion


    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        Dash = GetComponent<DashSkill>();
        Clone = GetComponent<CloneSkill>();
        Sword = GetComponent<SwordSkill>();
        Blackhole = GetComponent<BlackholeSkill>();
    }
}
