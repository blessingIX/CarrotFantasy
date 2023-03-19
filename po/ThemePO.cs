using System.Collections.Generic;

namespace CarrotFantasy.po
{
    public class ThemePO
    {
        private string code;
        public string Code { set; get; }

        private List<LevelPO> levels;
        public List<LevelPO> Levels { set; get; }
    }
}