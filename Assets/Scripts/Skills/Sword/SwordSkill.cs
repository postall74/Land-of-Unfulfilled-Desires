using System;
using UnityEngine;

public enum SwordType 
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill
{
    #region Fields
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    [SerializeField] private int _bounceAmount;
    [SerializeField] private float _bounceGravity;
    [SerializeField] private float _bounceSpeed;

    [Header("Pierce info")]
    [SerializeField] private int _pierceAmount;
    [SerializeField] private float _pierceGravity;

    [Header("Spin info")]
    [SerializeField] private float _hitCooldown = .35f;
    [SerializeField] private float _maxTravelDistance = 7f;
    [SerializeField] private float _spinDuration = 2f;
    [SerializeField] private float _spinGravity = 1f;

    [Header("Skill info")]
    [SerializeField] private GameObject _swordPrefab;
    [SerializeField] private Vector2 _launchForce;
    [SerializeField] private float _swordGravity;
    [SerializeField] private float _returnSpeed;
    [SerializeField] private float _freezeTimeDuration;

    [Header("Aim dots")]
    [SerializeField] private int _numberOfDots = 15;
    [SerializeField] private float _spaceBeetwenDots;
    [SerializeField] private GameObject _dotPrefab;
    [SerializeField] private Transform _dotsParent;

    private GameObject[] _dots;
    private Vector2 _finalDirection;
    #endregion

    public void CreateSword()
    {
        GameObject newSword = Instantiate(_swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();
        

        switch (swordType)
        {
            case SwordType.Bounce:
                newSwordScript.SetupBounce(true, _bounceAmount, _bounceSpeed);
                break;
            case SwordType.Pierce:
                newSwordScript.SetupPierce(_pierceAmount);
                break;
            case SwordType.Spin:
                newSwordScript.SetupSpin(true, _maxTravelDistance, _spinDuration, _hitCooldown);
                break;
        }

        newSwordScript.SetupSword(_finalDirection, _swordGravity, player, _freezeTimeDuration, _returnSpeed);
        player.AssingNewSword(newSword);
        DotsActive(false);
    }

    #region Aim region

    public Vector2 AimDirection()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - playerPos;
        return direction;
    }

    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < _dots.Length; i++)
        {
            _dots[i].SetActive(isActive);
        }
    }

    private void GenereateDots()
    {
        _dots = new GameObject[_numberOfDots];

        for (int i = 0; i < _numberOfDots; i++)
        {
            _dots[i] = Instantiate(_dotPrefab, player.transform.position, Quaternion.identity, _dotsParent);
            _dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position +
            new Vector2(
            AimDirection().normalized.x * _launchForce.x,
            AimDirection().normalized.y * _launchForce.y) * t + .5f * (Physics2D.gravity * _swordGravity) * (t * t);

        return position;
    }
    #endregion

    protected override void Start()
    {
        base.Start();

        GenereateDots();
        SetupGravity();
    }

    protected override void Update()
    {
        base.Update();

        SetupGravity();

        if (Input.GetKeyUp(KeyCode.Mouse1))
            _finalDirection = new Vector2(AimDirection().normalized.x * _launchForce.x, AimDirection().normalized.y * _launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < _dots.Length; i++)
                _dots[i].transform.position = DotsPosition(i * _spaceBeetwenDots);
        }
    }

    private void SetupGravity()
    {
        switch (swordType)
        {
            case SwordType.Regular:
                _swordGravity = 1f;
                break;
            case SwordType.Bounce:
                _swordGravity = _bounceGravity;
                break;
            case SwordType.Pierce:
                _swordGravity = _pierceGravity;
                break;
            case SwordType.Spin:
                _swordGravity = _spinGravity;
                break;
            default:
                _swordGravity = 1f;
                break;
        }
    }
}
