using Godot;
using System;

namespace CarrotFantasy.autoload
{
    public static class Autoload
    {
        public static T _<T>(this Node node) where T : class
        {
            return node.GetNode<T>($"/root/{typeof(T).Name}");
        }

        public static T _<T>(this Node node, string name) where T : class
        {
            return string.IsNullOrEmpty(name) ? node._<T>() : node.GetNode<T>($"/root/{name}");
        }
    }
}
