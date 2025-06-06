using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private int cooldownValue;
    [SerializeField] private Image slotImage;
    [SerializeField] private TMP_Text cooldownText;
    [SerializeField] private UIColor buttonColors;

    private Button button;
    private Outline outline;



    private void Start()
    {
        button = transform.GetComponent<Button>();
        outline = transform.GetComponent<Outline>();

        this.Disable();
    }

    public void Enable()
    {
        if (cooldownText)
        {
            cooldownValue = 0;
            cooldownText.text = cooldownValue.ToString();
            cooldownText.gameObject.SetActive(false);
        }
        button.interactable = true;
        transform.GetComponent<Button>().interactable = true;
        slotImage.gameObject.SetActive(true);

        ApplyButtonColors();
    }

    public void Disable()
    {
        button.interactable = false;
        if(cooldownText) cooldownText.gameObject.SetActive(false);
        ApplyButtonColors();
    }

    public void setCooldown(int cooldown)
    {
        if (!cooldownText) return;
        if (cooldown <= 0)
        {
            cooldown = 0;
            cooldownText.gameObject.SetActive(false);
        }
        if (cooldown > 0)
        {
            cooldownValue = cooldown;
            cooldownText.text = cooldownValue.ToString();
            cooldownText.gameObject.SetActive(true);
        }
    }

    public void HighlightSlot(bool highlight)
    {

        if (!outline || !button) return;

        if (highlight)
        {
            var colors = button.colors;
            colors.normalColor = buttonColors.highlightColor;
            button.colors = colors;
        }
        else
        {
            var colors = button.colors;
            colors.normalColor = buttonColors.normalColor;
            button.colors = colors;
        }

        outline.effectColor = highlight
            ? buttonColors.highlightColor
           : buttonColors.normalOutlineColor;
    }

    private void ApplyButtonColors()
    {
        if (button)
        {
            var colors = button.colors;
            colors.normalColor = buttonColors.normalColor;
            colors.disabledColor = buttonColors.disabledColor;
            button.colors = colors;
        }

        if (outline)
        {
            outline.effectColor = button.interactable
                ? buttonColors.normalOutlineColor
                : buttonColors.disabledOutlineColor;
        }
    }
}
