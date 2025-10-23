using UnityEngine;
using UnityEngine.UI;

public class ClientOrder : MonoBehaviour
{
    [Header("État")]
    public string orderedDish;

    [Header("UI Bulle")]
    [SerializeField] private Image dishIcon; // L'Image UI dans la bulle

    [Header("Sprite du burger")]
    [SerializeField] private Sprite burgerSprite; 

    public void SetRandomOrder()
    {
    
        orderedDish = "Burger";

        if (!dishIcon)
        {
            Debug.LogWarning("DishIcon non assigné dans ClientOrder sur " + gameObject.name);
            return;
        }

        dishIcon.sprite = burgerSprite;
        dishIcon.enabled = dishIcon.sprite != null;
    }
}
