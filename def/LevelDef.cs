using System.Collections.Generic;

namespace CarrotFantasy.def
{
    public class LevelDef
    {
        public int Index;

        public string ThemeCode;

        public int TotalWaves;

        public List<string> AvailableTowers;

        public bool IsUnlocked;

        public List<WaveDef> Waves;
    }
}