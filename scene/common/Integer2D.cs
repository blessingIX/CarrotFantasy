using Godot;
using Godot.Collections;
using GodotUtilities;

namespace CarrotFantasy.scene.common
{
    public partial class Integer2D : Node2D
    {
        private int value;
        [Export(PropertyHint.Range, "0,2147483647")]
        public int Value
        {
            get => this.value;
            set
            {
                this.value = value;
                this.UpdateDisplay();
                NotifyPropertyListChanged();
            }
        }

        private readonly Dictionary<char, Texture2D> dict = new();

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            char[] numberStrings = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            foreach (char number in numberStrings)
            {
                Texture2D texture2D = GD.Load<Texture2D>($"res://resource/common/WhiteNumber{number}.tres");
                dict.Add(number, texture2D);
            }

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            string numberString = this.value.ToString();
            char[] numberCharArray = numberString.ToCharArray();

            Array<Node> nodes = GetChildren();
            if (nodes != null && nodes.Count > 0)
            {
                foreach (var node in nodes)
                {
                    if (node == null)
                    {
                        continue;
                    }
                    node.QueueFree();
                }
                nodes.Clear();
            }
            
            if (numberCharArray.Length == 0)
            {
                return;
            }

            int step = 20;
            for (int i = 0; i < numberCharArray.Length; i++)
            {
                char numberChar = numberCharArray[i];

                Texture2D texture2D;
                bool have = dict.TryGetValue(numberChar, out texture2D);
                Sprite2D sprite2D = new();
                if (have)
                {
                    sprite2D.Texture = texture2D;
                }
                sprite2D.Position = new Vector2(step * i, 0);
                // sprite2D.Name = $"{i}_{numberChar}";
                AddChild(sprite2D);
            }
        }
    }
}
