using UnityEngine;

public class UsableItem:Item, IUsable
{
    // Эффекты
    public BaseEffect[] effects = null;
    public void Use()
    {
        // to-do анимация
        foreach (var effect in effects)
        {
            effect.Apply();
        }
        Debug.Log("Предмет использован");
    }
}