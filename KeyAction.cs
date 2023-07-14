using UnityEngine;

public class KeyAction : BaseAction
{
    public keybutton Key;
    private bool _primed = false;

    public override void CheckComplete()
    {
        if (Input.GetKeyDown(Key.Key))
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

        global.LightsOn(Key.gameObject);
    }

    public override void ActionEnded()
    {
        base.ActionEnded();

        global.LightsOff(Key.gameObject);
        global.PlayKeyboardClick();

        LeanTween.value(Key.gameObject, 1f, 0.8f, 0.15f)
        .setOnUpdate((float val) =>
        {
            Key.gameObject.transform.localScale = val * global.scale * Vector3.one;
            Key.KeyText.transform.localScale = val * Vector3.one;
        })
        .setEaseInCubic()
        .setOnComplete(() =>
        {
            LeanTween.value(Key.gameObject, 0.8f, 1f, 0.15f)
            .setOnUpdate((float val) =>
            {
                Key.gameObject.transform.localScale = val * global.scale * Vector3.one;
                Key.KeyText.transform.localScale = val * Vector3.one;
            })
            .setEaseOutCubic();
        });
    }
}
