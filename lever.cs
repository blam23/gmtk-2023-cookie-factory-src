using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class lever : MonoBehaviour
{
    // Editable Fields
    public float Length = 0.1f;

    [Range(0.0f, 1.0f)]
    public float PercentPulled = 0;
    public float Velocity = 0;
    public bool Pulling = false;
    public float SnapSpeed = 0.0001f;
    public AudioSource[] Sounds;
    public GameObject HitBoxObject = null;

    public Vector3 OriginalPosition = Vector3.zero;
    public Vector3 EndPosition = Vector3.zero;

    private bool _primeSound;
    private bool _primeTop;

    // Start is called before the first frame update
    void Start()
    {
        OriginalPosition = transform.position;
        EndPosition = transform.position + Vector3.down * Length;
    }

    void PlayAudio()
    {
        if (Sounds.Length == 0)
            return;

        var sound = Sounds[Random.Range(0, Sounds.Length)];
        sound.Play();
    }

    public float CalculatePercent()
    {
        var t = PercentPulled;
        t = t * t * (3f - 2f * t);
        return t;
    }

    public Vector3 CalculatePosition() => Vector3.Lerp(OriginalPosition, EndPosition, CalculatePercent());

    // Update is called once per frame
    void Update()
    {
        transform.position = CalculatePosition();
        transform.localScale = (1f - Mathf.Abs((CalculatePercent() - 0.5f) / 2f)) * 0.5f * Vector3.one;

        if (!Pulling)
        {
            if (PercentPulled > 0f && PercentPulled < 1f)
            {
                if (PercentPulled < 0.5f)
                    Velocity -= SnapSpeed;

                if (PercentPulled > 0.5f)
                    Velocity += SnapSpeed;
            }
            else
            {
                Velocity = 0f;
            }
        }

        if (PercentPulled < 0.2f || PercentPulled > 0.8f)
        {
            if (_primeSound)
            {
                PlayAudio();
                _primeSound = false;
                _primeTop = PercentPulled > 0.8f;
            }
            else
            {
                if (_primeTop && PercentPulled < 0.2f)
                    _primeSound = true;
                else if (!_primeTop && PercentPulled > 0.8f)
                    _primeSound = true;
            }
        }

        PercentPulled += Velocity;
        PercentPulled = Mathf.Clamp(PercentPulled, 0f, 1f);
    }
}
