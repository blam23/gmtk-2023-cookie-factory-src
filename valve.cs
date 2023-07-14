using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class valve : MonoBehaviour
{
    // Editable Fields
    [Range(0.0f, 1.0f)]
    public float PercentRotated = 0.5f;
    public float MoveSpeedMul = 0.001f;
    public float Velocity = 0;
    public float Friction = 0.98f;
    public bool Pulling = false;
    public Color NormalColor = Color.white;
    public Color HoverColor = new(233, 168, 69);
    public Color ClickColor = Color.blue;
    public Camera cam;
    public AudioSource RotateSound;
    public float PlayVolume = 0.5f;
    public AudioClip Clips;

    public ulong Rotations = 0;

    private bool _hovering = false;
    private float _angle;
    private float _dragStartAngle;
    private float _startPercent;
    private float _halfPercent;
    private bool _checkUp = false;
    private bool _reset;
    private bool _counting = false;
    private SpriteRenderer _sr;
    private LTDescr _fadeOutTween;

    public Quaternion CalculateRotation()
    {
        return Quaternion.AngleAxis(PercentRotated * 360, Vector3.back);
    }

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _sr.color = NormalColor;
    }

    private Quaternion ConvertAngle(float a)
    {
        return Quaternion.AngleAxis(Mathf.Rad2Deg * a, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        if (Pulling)
        {
            var mouseDiff = Input.mousePosition - cam.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(mouseDiff.y, mouseDiff.x);

            //Debug.Log(angle);
            //Debug.Log(_dragStartAngle);

            angle = angle - _dragStartAngle;

            //if (mouseDiff.y < 0)
            //    Velocity = (mouseDiff.magnitude * MoveSpeedMul) / Screen.height;
            _angle = angle;

            //transform.rotation = Quaternion.AngleAxis(angle * 180.0f / Mathf.PI, Vector3.forward);
            
            var targetPercent = ((-angle + Mathf.PI * 2) / (Mathf.PI * 2));
            PercentRotated = targetPercent;
            //Velocity = (targetPercent - PercentRotated) * 0.1f;

            PercentRotated %= 1f;

            //Debug.Log($"{targetPercent} => {PercentRotated} => {Velocity}");
        }

        PercentRotated += Velocity;

        //Debug.Log(_counting);
        //Debug.Log(PercentRotated >= _halfPercent);
        //Debug.Log(PercentRotated > _startPercent);
        //Debug.Log($"{_startPercent} -> {_halfPercent} -> {PercentRotated}");

        if (!_reset && _counting
            && ((_checkUp && PercentRotated >= _halfPercent)
            || (!_checkUp && PercentRotated <= _halfPercent)))
        {
            _reset = true;
            Debug.Log("Armed");
        }

        if (_reset && PercentRotated + 0.05f > _startPercent && PercentRotated - 0.05f < _startPercent)
        { 
            Debug.Log("Spun!");
            _reset = false;
            _counting = false;
            Rotations++;
        }

        if (!Pulling)
        {
            Velocity *= Friction;
        }

        if (Velocity < 0.000001f || Velocity > -0.000001f)
            Velocity = 0f;

        transform.rotation = CalculateRotation();
    }

    private void OnMouseOver()
    {
        if (!_hovering)
        {
            _hovering = true;
            global.hoverGrab = true;

            if (!Pulling)
                _sr.color = HoverColor;
        }
    }

    private void OnMouseDown()
    {
        Pulling = true;
        global.grabbing = true;
        var mouseDiff = Input.mousePosition - cam.WorldToScreenPoint(transform.position);
        _dragStartAngle = Mathf.Atan2(mouseDiff.y, mouseDiff.x) - _angle;
        _dragStartAngle %= Mathf.PI * 2;
        _dragStartAngle = _dragStartAngle > Mathf.PI ? _dragStartAngle - (Mathf.PI * 2f) : _dragStartAngle;
        _dragStartAngle = _dragStartAngle < -Mathf.PI ? _dragStartAngle + (Mathf.PI * 2f) : _dragStartAngle;
        _sr.color = ClickColor;

        if (_fadeOutTween is not null)
            LeanTween.cancel(_fadeOutTween.id);

        _fadeOutTween = null;

        RotateSound.volume = PlayVolume;
        RotateSound.Play();
    }

    private void OnMouseUp()
    {
        Pulling = false;
        global.grabbing = false;
        _sr.color = _hovering ? HoverColor : NormalColor;

        _fadeOutTween = LeanTween.value(gameObject, PlayVolume, 0f, 0.3f)
            .setOnUpdate((float val) =>
            {
                RotateSound.volume = val;
            })
            .setEaseOutCubic();
    }

    private void OnMouseExit()
    {
        _hovering = false;
        global.hoverGrab = false;
        if (!Pulling)
            _sr.color = NormalColor;
    }

    internal void CountRotation()
    {
        _startPercent = PercentRotated;
        _halfPercent = (_startPercent + 0.5f) % 1f;
        _reset = false;
        _counting = true;

        _checkUp = PercentRotated < _halfPercent;
    }
}
