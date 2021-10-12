using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopRTFLibrary
{
    public static class RTFCharacterMap
    {
        // Replacement dictionary.
        public static Dictionary<char, string> Map = new Dictionary<char, string>()
        {
            {'¿', @"\'bf"},
            {'¡', @"\'a1"},

            {'á', @"\'e1"},
            {'é', @"\'e9"},
            {'í', @"\'ed"},
            {'ó', @"\'f3"},
            {'ú', @"\'fa"},

            {'Á', @"\'c1"},
            {'É', @"\'c9"},
            {'Í', @"\'cd"},
            {'Ó', @"\'d3"},
            {'Ú', @"\'da"},

            {'ñ', @"\'f1"},
            {'Ñ', @"\'d1"},

            {'ü', @"\'fc"},
            {'Ü', @"\'dc"},

            {'ö', @"\'f6"},
            {'Ö', @"\'d6"},

            {'ä', @"\'e4"},
            {'Ä', @"\'c4"},

            {'ª', @"\'aa"},
            {'º', @"\'ba"},

            {'ç', @"\'e7"},
            {'Ç', @"\'c7"},

            {'²', @"\'b2"},

            {'´', @"\'27"},
            {'“', @"\'93"},
            {'”', @"\'94"},
            {'…', @"\'85"},
            {'’', @"\'92"},
            {'°', @"\'b0"}
        };
    }
}
