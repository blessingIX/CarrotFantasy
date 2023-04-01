using System.Collections.Generic;

namespace CarrotFantasy.def
{
    public class ThemeDef
    {
        private string code;
        public string Code { set; get; }

        private List<LevelDef> levels;
        public List<LevelDef> Levels { set; get; }
    }
}