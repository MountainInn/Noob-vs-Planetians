public class GunSlot : EquipmentSlot<Gun>
{
    protected override void OnToggleOff(Gun current)
    {
        if (current.isShooting)
            current.ToggleShooting(false);
    }

    protected override void OnToggleOn(Gun previous, Gun current)
    {
        if (previous.isShooting)
            current.ToggleShooting(true);
    }
}
