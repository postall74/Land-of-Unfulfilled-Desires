using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkill : Skill
{
    #region Fields
    [SerializeField] private GameObject _blackholePrefab;
    [Header("Skill info")]
    [SerializeField] private int _amountOfAttack = 5;
    [SerializeField] private float _cloneAttackCooldown = .35f;
    [SerializeField] private float _blackholeDuration;
    [Space]
    [SerializeField] private float _maxSize;
    [SerializeField] private float _growSpeed;
    [SerializeField] private float _shrinkSpeed;
    private BlackholeSkillController _currentBlackhole;
    #endregion

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public bool SkillCompleted()
    {
        if (!_currentBlackhole)
            return false;

        if (_currentBlackhole.PlayerCanExitState)
        {
            _currentBlackhole = null;
            return true;
        }

        return false;
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(_blackholePrefab, player.transform.position, Quaternion.identity);
        _currentBlackhole = newBlackhole.GetComponent<BlackholeSkillController>();
        _currentBlackhole.SetupBlackhole(_maxSize, _growSpeed, _shrinkSpeed, _amountOfAttack, _cloneAttackCooldown, _blackholeDuration);

    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
