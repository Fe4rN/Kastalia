using UnityEngine;

[CreateAssetMenu(fileName = "UIColor", menuName = "UIHelper/UIColor")]
public class UIColor : ScriptableObject
{
    public Color normalColor;
    public Color disabledColor;
    public Color normalOutlineColor;
    public Color disabledOutlineColor;
    public Color highlightColor;
    public Color highlightOutlineColor;
}
