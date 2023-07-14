using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public static class global
{
    public static float scale = 0.3334f;
    public static bool grabbing = false;
    public static bool clicking = false;
    public static bool hoverGrab = false;
    public static bool hoverPress = false;

    private static void ForLights(GameObject obj, Action<Light2D> action)
    {
        var lights = obj.GetComponentsInChildren<Light2D>();

        foreach (var light in lights)
        {
            action(light);
        }
    }

    public static void LightsOn(GameObject obj) => ForLights(obj, (light) => light.enabled = true);
    public static void LightsOff(GameObject obj) => ForLights(obj, (light) => light.enabled = false);

    internal static void PlayKeyboardClick()
    {
        mash.Instance.PlayKeyboardClick();
    }
}
