using UnityEngine;

public class ValveAction : BaseAction
{
    public valve Valve;

    private float _startingRotations;

    public override void CheckComplete()
    {
        if (Valve.Rotations > _startingRotations)
            Complete = true;
    }

    public override void ActionStart()
    {
        base.ActionStart();
        global.LightsOn(Valve.gameObject);

        _startingRotations = Valve.Rotations;
        Valve.CountRotation();
    }

    public override void ActionEnded()
    {
        base.ActionEnded();
        global.LightsOff(Valve.gameObject);

        LeanTween.value(Valve.gameObject, 1f, 1.1f, 0.1f)
            .setOnUpdate((float val) =>
            {
                Valve.gameObject.transform.localScale = val * global.scale * Vector3.one;
            })
            .setEaseInCubic()
            .setOnComplete(() =>
             {
                 LeanTween.value(Valve.gameObject, 1.1f, 1f, 0.1f)
                    .setOnUpdate((float val) =>
                    {
                        Valve.gameObject.transform.localScale = val * global.scale * Vector3.one;
                    })
                    .setEaseOutCubic();
             });
    }
}
