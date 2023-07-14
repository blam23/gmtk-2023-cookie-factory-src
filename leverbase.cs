using UnityEngine;

public class leverbase : MonoBehaviour
{
    private SpriteRenderer _sr;

    public Color NormalColor = Color.white;
    public Color HoverColor = new Color(233, 168, 69);
    public Color ClickColor = Color.blue;

    public float MoveSpeedMul = 0.1f;

    public lever lever;

    private bool _hovering = false;
    private bool _dragging = false;

    private Vector3 _dragStart;
    private float _dragSpeed = 0;


    void Start()
    {
        _sr = lever.GetComponent<SpriteRenderer>();
        _sr.color = NormalColor;
    }

    private void Update()
    {
        if (_dragging)
        {
            var mouseDiff = Input.mousePosition - _dragStart;
            _dragSpeed = (mouseDiff.y * MoveSpeedMul) / Screen.height;
        }
        else
            _dragSpeed *= 0.97f;

        if (_dragSpeed <= 0.0001f && _dragSpeed >= -0.0001f)
            _dragSpeed = 0;
        else
            lever.Velocity = -_dragSpeed;
    }

    private void OnMouseOver()
    {
        if (!_hovering)
        {
            _sr.color = HoverColor;
            _hovering = true;
            global.hoverGrab = true;
        }
    }

    private void OnMouseDown()
    {
        _dragging = true;
        global.grabbing = true;
        lever.Pulling = true;
        _dragStart = Input.mousePosition;
        _sr.color = ClickColor;
    }

    private void OnMouseUp()
    {
        _dragging = false;
        global.grabbing = false;
        lever.Pulling = false;
        _sr.color = _hovering ? HoverColor : NormalColor;
    }

    private void OnMouseExit()
    {
        _hovering = false;
        global.hoverGrab = false;

        if (!_dragging)
            _sr.color = NormalColor;
    }
}
