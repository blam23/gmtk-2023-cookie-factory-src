
using UnityEngine;

public enum StepType
{
    Key,
    Lever,
    Valve,
    BlueButton,
    GreenButton,
    RedButton,
}

public class RecipeStep
{
    public string Name;
    public StepType StepType;

    public SpriteRenderer AssociatedSprite;

    // Data
    public KeyCode Key;
    public clickbutton Button = null;
    public lever Lever = null;
    public valve Valve = null;
    public keybutton KeyButton = null;

    public BaseAction CreateActionFromStep()
    {
        switch (StepType)
        {
            case StepType.Key:
                Debug.Log("Creating key action for: " + Key.ToString());
                var keyaction = new KeyAction() { Key = KeyButton };
                return keyaction;

            case StepType.Lever:
                var leveraction = new LeverAction() { Lever = Lever };
                leveraction.PullDown = Lever.PercentPulled <= 0f ? true : false;
                Debug.Log("Creating lever action, pulling down? " + (leveraction.PullDown ? "true" : "false"));
                return leveraction;

            case StepType.Valve:
                Debug.Log("Creating valve action");
                var valveaction = new ValveAction() { Valve = Valve };
                return valveaction;

            case StepType.BlueButton:
            case StepType.GreenButton:
            case StepType.RedButton:
                Debug.Log("Creating button action");
                var buttonaction = new ButtonAction() { Button = Button };
                return buttonaction;

            default:
                Debug.Log("Creating default action??");
                break;
        }

        return null;
    }

    public override string ToString()
    {
        return StepType switch
        {
            StepType.Key => $"Press the [{KeyButton.KeyText.text}] key",
            StepType.Lever => "Flip the lever",
            StepType.Valve => "Spin the valve",
            StepType.BlueButton => "Click the [Blue] Button",
            StepType.GreenButton => "Click the [Green] Button",
            StepType.RedButton => "Click the [Red] Button",
            _ => "Dunno",
        };
    }
}
