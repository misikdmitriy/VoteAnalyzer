﻿using System;

namespace VoteAnalyzer.Common
{
    public class WordSeparator
    {
        private static readonly char[] Delimeters = { ' ', '.', ',', ':', '\n', (char)160 };

        public static string[] Split(string text)
        {
            return string.IsNullOrEmpty(text)
                ? new string[0]
                : text.Split(Delimeters, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
