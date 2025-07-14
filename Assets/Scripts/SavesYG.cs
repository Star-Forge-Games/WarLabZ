namespace YG
{
    public partial class SavesYG
    {
        public int[] supplies = new int[] { 2, 1, 0 };
        public int playedBefore = -1;
        public int cash = 0;
        public int wallLevel = 0;
        public int[] weaponLevels = new int[] { 0, -1, -1, -1, -1 };
        public int selectedWeapon = 0;
        public bool soundOn = true;
        public int localeId = -1;
        public int energyLeft;
        public long nextEnergyRechargeTimeStamp;
        public int record;
        public bool hasLabel;
        public bool reviewed;
    }
}
