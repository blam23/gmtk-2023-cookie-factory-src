
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public string CookieName;
    public List<RecipeStep> Actions;
    public int Quota;
    public Color CookieColor;
    public bool CookieGlow = false;
}

public static class LevelData
{
    public static Level GetLevel(int level)
    {
        if (level < 0) level = 0;

        level %= _levels.Length;
        return _levels[level];
    }

    public static int LevelCount => _levels.Length;
    private static Level[] _levels = new Level[]
    {
        new Level() {
            CookieName = "Choccy Chip",
            Quota = 30,
            Actions = new List<RecipeStep>()
            {
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.W},
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.F},
                new RecipeStep() { StepType = StepType.RedButton },
                new RecipeStep() { StepType = StepType.Lever },
            },
            CookieColor = new Color(185, 115, 0),
        },
        new Level() {
            CookieName = "Double Choccy Chip",
            Quota = 30,
            Actions = new List<RecipeStep>()
            {
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.R},
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.A},
                new RecipeStep() { StepType = StepType.GreenButton },
                new RecipeStep() { StepType = StepType.Lever },
                new RecipeStep() { StepType = StepType.GreenButton },
            },
            CookieColor = new Color(255, 246, 232),
        },
        new Level() {
            CookieName = "Anchovy Chip",
            Quota = 30,
            Actions = new List<RecipeStep>()
            {
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.E},
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.F},
                new RecipeStep() { StepType = StepType.Valve },
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.W},
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.D},
                new RecipeStep() { StepType = StepType.Valve },
                new RecipeStep() { StepType = StepType.RedButton },
            },
            CookieColor = new Color(255, 0, 222),
        },
        new Level() {
            CookieName = "Double Anchovy Chip",
            Quota = 40,
            Actions = new List<RecipeStep>()
            {
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.A},
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.F},
                new RecipeStep() { StepType = StepType.Lever },
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.Q},
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.R},
                new RecipeStep() { StepType = StepType.Valve },
                new RecipeStep() { StepType = StepType.GreenButton },
                new RecipeStep() { StepType = StepType.BlueButton },
            },
            CookieColor = new Color(200, 0, 200),
        },
        new Level() {
            CookieName = "Unprocessed Uranium",
            Quota = 30,
            Actions = new List<RecipeStep>()
            {
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.Q},
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.S},
                new RecipeStep() { StepType = StepType.Lever },
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.W},
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.D},
                new RecipeStep() { StepType = StepType.BlueButton },
                new RecipeStep() { StepType = StepType.GreenButton },
                new RecipeStep() { StepType = StepType.RedButton },
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.E},
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.F},
                new RecipeStep() { StepType = StepType.Valve },
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.R},
                new RecipeStep() { StepType = StepType.Key, Key = KeyCode.A},
                new RecipeStep() { StepType = StepType.GreenButton },
            },
            CookieColor = new Color(255, 0, 0),
            CookieGlow = true,
        },
    };
}
