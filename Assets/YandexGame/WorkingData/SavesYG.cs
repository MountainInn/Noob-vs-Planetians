
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;


        ///
        ///

        public int healthUpgradeLevel = 0;
        public int damageUpgradeLevel = 0;
        public int attackRateUpgradeLevel = 0;
        public int attackRangeUpgradeLevel = 0;
        public int lastWeaponIndex = 0;
        public int weaponExpirience = 0;
        public int totalCoins;
        public int levelCount;
        public int currentLevelIndex;

        public SavesYG()
        {
        }
    }
}
