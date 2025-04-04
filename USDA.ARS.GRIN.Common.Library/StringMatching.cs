﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.Common.Library
{
    public static class StringMatching
    {
        /// <summary>
        ///     Calculate the difference between 2 strings using the Levenshtein distance algorithm
        /// </summary>
        /// <param name="source1">First string</param>
        /// <param name="source2">Second string</param>
        /// <returns></returns>
        public static int CalculateLevenshteinDistance(string source1, string source2) //O(n*m)
        {
            var source1Length = source1.Length;
            var source2Length = source2.Length;

            var matrix = new int[source1Length + 1, source2Length + 1];

            // First calculation, if one entry is empty return full length
            if (source1Length == 0)
                return source2Length;

            if (source2Length == 0)
                return source1Length;

            // Initialization of matrix with row size source1Length and columns size source2Length
            for (var i = 0; i <= source1Length; matrix[i, 0] = i++) { }
            for (var j = 0; j <= source2Length; matrix[0, j] = j++) { }

            // Calculate rows and collumns distances
            for (var i = 1; i <= source1Length; i++)
            {
                for (var j = 1; j <= source2Length; j++)
                {
                    var cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }
            // return result
            return matrix[source1Length, source2Length];
        }

        public static double CalculateDiceCoefficient(string str1, string str2)
        {
            // Convert strings to sets of characters
            var set1 = new HashSet<char>(str1);
            var set2 = new HashSet<char>(str2);

            // Calculate intersection size
            int intersectionSize = 0;
            foreach (char c in set1)
            {
                if (set2.Contains(c))
                {
                    intersectionSize++;
                }
            }

            // Calculate Dice Coefficient
            double diceCoefficient = (2.0 * intersectionSize) / (set1.Count + set2.Count);

            return diceCoefficient;
        }

        public static int CalculateHammingDistance(string str1, string str2)
        {
            // Check if the strings have the same length
            if (str1.Length != str2.Length)
            {
                throw new ArgumentException("Strings must be of equal length");
            }

            int hammingDistance = 0;

            // Iterate through each character and compare
            for (int i = 0; i < str1.Length; i++)
            {
                if (str1[i] != str2[i])
                {
                    hammingDistance++;
                }
            }

            return hammingDistance;
        }
    }
}
