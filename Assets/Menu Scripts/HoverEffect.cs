using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;
    private Color normalColor;
    public Color hoverColor = Color.red; // Customize the hover color as needed

    void Start()
    {
        buttonImage = GetComponent<Image>();
        normalColor = buttonImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Hover effect
        buttonImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset to normal color when not hovering
        buttonImage.color = normalColor;
    }
}
