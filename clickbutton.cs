using UnityEngine;

public class clickbutton : MonoBehaviour
{
    private SpriteRenderer _sr;

    public Color NormalColor = Color.white;
    public Color HoverColor = new Color(233, 168, 69);
    public Color ClickColor = Color.blue;

    public AudioSource ClickSound;

    private bool _hovering = false;
    private bool _down = false;
    public bool Down => _down;


    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _sr.color = NormalColor;
    }

    private void OnMouseOver()
    {
        if (!_hovering)
        {
            _sr.color = HoverColor;
            _hovering = true;
            global.hoverPress = true;
        }
    }

    private void OnMouseDown()
    {
        _down = true;
        global.clicking = true;
        _sr.color = ClickColor;

        ClickSound?.Play();
    }

    private void OnMouseUp()
    {
        _down = false;
        global.clicking = false;
        _sr.color = _hovering ? HoverColor : NormalColor;
    }

    private void OnMouseExit()
    {
        _hovering = false;
        global.hoverPress = false;

        if (!_down)
            _sr.color = NormalColor;
    }
}
