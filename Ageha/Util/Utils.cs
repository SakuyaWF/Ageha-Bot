using System;
using System.Collections.Generic;
using System.Text;

namespace Ageha.Util
{
    public class Utils
    {
        public static T Choose<T>(params T[] choices) => choices[new Random().Next(0, choices.Length)];
    }
}
