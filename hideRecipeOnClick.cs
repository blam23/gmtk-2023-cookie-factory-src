using UnityEngine;

public class hideRecipeOnClick : MonoBehaviour
{
    private void OnMouseUp()
    {
        mash.Instance.HideRecipe();
    }
}
