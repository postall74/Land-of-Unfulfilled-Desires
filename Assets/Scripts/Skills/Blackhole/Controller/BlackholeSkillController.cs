using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class BlackholeSkillController : MonoBehaviour
{
    #region Fields
    [Header("HotKey info")]
    [SerializeField] private GameObject _hotKeyPrefab;
    [SerializeField] private List<KeyCode> _keyKodeList;
    private List<GameObject> _createHotKey = new();
    private bool _canCreateHotKey = true;
    [Header("Skill info")]
    private float _maxSize;
    private float _growSpeed;
    private float _shrinkSpeed;
    private int _amountOfAttack;
    private float _cloneAttackCooldown;
    private float _blackholeTimer;
    private bool _canGrow = true;
    private bool _canShrink;
    private bool _playerCanDisapear = true;
    private List<Transform> _targets = new();
    private bool _cloneAttackReleased;
    private float _cloneAttackTimer;
    #endregion

    #region Properties
    public bool PlayerCanExitState { get; private set; }
    #endregion

    public void SetupBlackhole(float maxSize, float growSpeed, float shrinkSpeed, int amountOfAttack, float cloneAttackCooldown, float blackholeDuration)
    {
        _maxSize = maxSize;
        _growSpeed = growSpeed;
        _shrinkSpeed = shrinkSpeed;
        _amountOfAttack = amountOfAttack;
        _cloneAttackCooldown = cloneAttackCooldown;
        _blackholeTimer = blackholeDuration;
    }

    public void AddEnemyToList(Transform enemyTransform) => _targets.Add(enemyTransform);

    private void Update()
    {
        _cloneAttackTimer -= Time.deltaTime;
        _blackholeTimer -= Time.deltaTime;

        if (_blackholeTimer < 0)
        {
            _blackholeTimer = Mathf.Infinity;

            if (_targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackholeAbility();
        }

        if (Input.GetKeyDown(KeyCode.R))
            ReleaseCloneAttack();

        CloneAttackLogic();

        if (_canGrow && !_canShrink)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_maxSize, _maxSize), _growSpeed * Time.deltaTime);

        if (_canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), _shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);

    #region Clone Attack Methods
    private void ReleaseCloneAttack()
    {
        if (_targets.Count <= 0)
            return;

        DestroyHotKey();
        _cloneAttackReleased = true;
        _canCreateHotKey = false;

        if (_playerCanDisapear)
        {
            _playerCanDisapear = false;
            PlayerManager.instance.Player.MakeTransprent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (_cloneAttackTimer < 0 && _cloneAttackReleased && _amountOfAttack > 0)
        {
            _cloneAttackTimer = _cloneAttackCooldown;
            int randomIndex = Random.Range(0, _targets.Count);
            float xOffset = Random.Range(0, 100) > 50 ? 2 : -2;
            SkillManager.instance.Clone.CreateClone(_targets[randomIndex], new Vector3(xOffset, 0));
            _amountOfAttack--;

            if (_amountOfAttack <= 0)
                Invoke(nameof(FinishBlackholeAbility), 1f);
        }
    }
    #endregion

    private void FinishBlackholeAbility()
    {
        DestroyHotKey();
        PlayerCanExitState = true;
        _canShrink = true;
        _cloneAttackReleased = false;
    }

    #region HotKeyes Methods
    private void CreateHotKey(Collider2D collision)
    {
        if (_keyKodeList.Count <= 0)
        {
            Debug.Log("Not enough hot keys in a code list!");
            return;
        }

        if (!_canCreateHotKey)
            return;

        GameObject newHotKey = Instantiate(_hotKeyPrefab, collision.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        _createHotKey.Add(newHotKey);

        KeyCode chooseKey = _keyKodeList[Random.Range(0, _keyKodeList.Count)];
        _keyKodeList.Remove(chooseKey);

        BlackholeHotKeyController newHotKeyScript = newHotKey.GetComponent<BlackholeHotKeyController>();

        newHotKeyScript.SetupHotKey(chooseKey, collision.transform, this);
    }

    private void DestroyHotKey()
    {
        if (_createHotKey.Count <= 0)
            return;

        for (int i = 0; i < _createHotKey.Count; i++)
            Destroy(_createHotKey[i]);
    }
    #endregion
}
