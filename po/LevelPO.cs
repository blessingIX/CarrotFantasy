using System.Collections.Generic;

namespace CarrotFantasy.po
{
    public class LevelPO
    {
        private int index;
        public int Index { set; get; }

        private string themeCode;
        public string ThemeCode { set; get; }

        private int totalWaves;
        public int TotalWaves { set; get; }

        private List<string> availableTowers;
        public List<string> AvailableTowers { set; get; }

        private bool isUnlocked;
        public bool IsUnlocked { set; get; }
    }
}