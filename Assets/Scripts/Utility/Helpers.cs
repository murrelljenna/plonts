using System;

namespace game.assets.utility
{
    public static class Helpers
    {
        public static T randomFromArray<T>(this T[] arr)
        {
            Random rn = new Random();
            return arr[rn.Next(0, arr.Length)];
        }
    }
}
