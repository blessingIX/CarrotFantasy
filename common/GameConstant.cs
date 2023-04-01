using Godot;

namespace CarrotFantasy.common
{
    public class GameConstant
    {
        /* 全局 */
        public class Global
        {
            /* 格子尺寸 */
            public static readonly Vector2 CellSize = new Vector2(64, 64);
        }

        /* 怪物 */
        public class Monster
        {
            /* 怪物场景名称常量 */
            public static readonly string Empty = "Empty";
            public static readonly string BigHair = "BigHair";
            public static readonly string CyanBird = "CyanBird";
            public static readonly string CyanScaleph = "CyanScaleph";
            public static readonly string PinkWorm = "PinkWorm";
            public static readonly string PurpleStar = "PurpleStar";
            public static readonly string YellowFlying = "YellowFlying";
        }
    }
}