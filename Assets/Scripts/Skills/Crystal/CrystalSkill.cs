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
    [Header("Multi stacking crystal")]
    [SerializeField] private int _amountOfStacks;
    [SerializeField] private float _multiStackCooldown;
    [SerializeField] private float _useTimeWindow;
    [SerializeField] private List<GameObject> _crystalLeft = new List<GameObject>();
    [SerializeField] private bool _canUseMultiStacks;
    #endregion

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
            return;

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

    private bool CanUseMultiCrystal()
    {
        if (_canUseMultiStacks)
        {
            if (_crystalLeft.Count > 0)
            {
                if (_crystalLeft.Count == _amountOfStacks)
                    Invoke(nameof(ResetAbility), _useTimeWindow);

                cooldown = 0;
                GameObject crystalToSpawn = _crystalLeft[_crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                _crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<CrystalSkillController>().SetupCrystal(_crystalDuration, _canExplode, _canMoveToEnemy, _moveSpeed, FindClosestEnemy(newCrystal.transform));

                if (_crystalLeft.Count <= 0)
                {
                    cooldown = _multiStackCooldown;
                    RefilCrystal();
                }

                return true;
            }
        }

        return false;
    }

    private void RefilCrystal()
    {
        int amountToAdd = _amountOfStacks - _crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            _crystalLeft.Add(_crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldown > 0)
            return;

        cooldown = _multiStackCooldown;
        RefilCrystal();
    }
}