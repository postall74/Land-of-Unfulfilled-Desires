using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    #region Fields
    [SerializeField] private float _crystalDuration;
    [SerializeField] private GameObject _crystalPrefab;
    private GameObject _currentCrystal;
    [Space, Header("Explosive crystal")]
    [SerializeField] private bool _canExplode;
    [Header("Moving crystal")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool _canMoveToEnemy;
    #endregion

    public override void UseSkill()
    {
        base.UseSkill();

        if (_currentCrystal == null)
        {
            _currentCrystal = Instantiate(_crystalPrefab, player.transform.position, Quaternion.identity);
            CrystalSkillController currentCrystalScript = _currentCrystal.GetComponent<CrystalSkillController>();
            currentCrystalScript.SetupCrystal(_crystalDuration, _canExplode, _canMoveToEnemy, _moveSpeed, FindClosestEnemy(_currentCrystal.transform));
        }
        else
        {
            if (_canMoveToEnemy)
                return;

            Vector2 playerPos = player.transform.position;
            player.transform.position = _currentCrystal.transform.position;
            _currentCrystal.transform.position = playerPos;
            _currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();
        }
    }

}