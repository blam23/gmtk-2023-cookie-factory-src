public class LeverAction : BaseAction
{
    public lever Lever;
    public bool PullDown = true;

    public override void CheckComplete()
    {
        if (PullDown && Lever.PercentPulled >= 1f)
        {
            Complete = true;
        }

        if (!PullDown && Lever.PercentPulled <= 0f)
        {
            Complete = true;
        }
    }

    public override void ActionStart()
    {
        base.ActionStart();

        global.LightsOn(Lever.gameObject);
    }

    public override void ActionEnded()
    {
        base.ActionEnded();

        global.LightsOff(Lever.gameObject);
    }
}
