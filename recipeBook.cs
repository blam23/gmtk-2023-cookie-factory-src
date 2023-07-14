using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recipeBook : MonoBehaviour
{
    private void OnMouseDown()
    {
        mash.Instance.ToggleRecipeBook();
    }
}
