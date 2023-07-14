
using UnityEngine;

public abstract class BaseAction
{
    public GameObject AssociatedObject;

    public bool Complete = false;
    public float StartTime = 0f;

    public virtual void ActionStart()
    {
        StartTime = Time.deltaTime;
    }

    public virtual void ActionEnded() { }

    public abstract void CheckComplete();
}
