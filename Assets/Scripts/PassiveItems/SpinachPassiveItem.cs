using UnityEngine;

public class SpinachPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentStrength *= 1 + passiveItemData.Multiplier / 100f;
    }
}
