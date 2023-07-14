using UnityEngine;
public class ButtonAction : BaseAction
{
    public clickbutton Button;
    private bool _primed = false;

    public override void CheckComplete()
    {
        if (Button.Down)
            _primed = true;
        else
        {
            if (_primed)
                Complete = true;
        }
    }

    public override void ActionStart()
    {
        base.ActionStart();

        global.LightsOn(Button.gameObject);
    }

    public override void ActionEnded()
    {
        base.ActionEnded();

        global.LightsOff(Button.gameObject);
    }
}
