namespace YG
{
    public partial class SavesYG
    {
        //public int[] levelStars = new int[] { -1, -1, -1, -1 };
        //public int points = 0; // wat
        //public int level = 0;
        //public int[] achievements; // useless
        //public int keys = 0;
        //public int donateCash = 0;
        public int[] supplies = new int[] { 2, 1, 0 };
        public bool playedBefore = false;
        public int cash = 0;
        public int wallLevel = 0;
        public int[] weaponLevels = new int[] { 0, -1, -1, -1, -1 };
        public int selectedWeapon = 0;
        public bool soundOn = true;
        public int localeId = -1;
        public int energyLeft;
        public long lastEnergySpentTimeStamp;
        public int record;
    }
}
