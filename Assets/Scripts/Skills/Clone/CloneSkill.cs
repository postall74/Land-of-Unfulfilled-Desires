using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    #region Fields
    [Header("Clone info")]
    [SerializeField] private GameObject _clonePrefab;
    [SerializeField] private float _cloneDuration;
    [Space]
    [SerializeField] private bool _canAttack;
    [SerializeField] private bool _createCloneOnDashStart;
    [SerializeField] private bool _createCloneOnDashOver;
    [SerializeField] private bool _canCreateCloneOnCounterAttack;
    [SerializeField, Range(0, 2)] private float _waitTimeCreateCloneOnCounterAttack;
    [Header("Clone can duplicate")]
    [SerializeField] private bool _canDuplicateClone;
    [SerializeField, Range(0, 100)] private float _chanceToDuplicateClone;
    [Header("Crystal instead of clone")]
    [SerializeField] private bool _crystalInsteadOfClone;
    #endregion

    #region Properties
    public bool CrystalInsteadOfClone => _crystalInsteadOfClone;
    #endregion

    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        if (_crystalInsteadOfClone)
        {
            SkillManager.instance.Crystal.CreateCrystal();
            return;
        }


        GameObject newClone = Instantiate(_clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, _cloneDuration, _canAttack, offset, FindClosestEnemy(newClone.transform), _canDuplicateClone, _chanceToDuplicateClone);
    }

    public void CreateCloneOnDashStart()
    {
        if (_createCloneOnDashStart)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnDashOver()
    {
        if (_createCloneOnDashOver)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnCounterAttack(Transform enemyTransform)
    {
        if (_canCreateCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(enemyTransform, new Vector3(2 * player.FacingDirection, 0)));
    }

    private IEnumerator CreateCloneWithDelay(Transform transform, Vector3 offset)
    {
        yield return new WaitForSeconds(_waitTimeCreateCloneOnCounterAttack);
        CreateClone(transform, offset);
    }
}
