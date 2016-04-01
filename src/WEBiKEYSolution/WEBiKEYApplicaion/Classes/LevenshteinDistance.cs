using System;
using System.Linq;

namespace WEBiKEY.Application.Classes
{
    public static class LevenshteinDistance
    {
        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        public static string FindBestMatch(string keyword, string[] items)
        {
            string[] startsWithMatch = items.Where(i => i.StartsWith(keyword) || keyword.StartsWith(i)).ToArray();
            if (startsWithMatch.Count() == 1)
            {
                return startsWithMatch[0];
            }
            else if (startsWithMatch.Count() > 1)
            {
                return startsWithMatch.Where(i => i.Length == startsWithMatch.Min(l => l.Length)).First();
            }

            int minCost = Compute(keyword, items[0]);
            string bestMatch = items[0];

            foreach (var item in items)
            {
                if (item.StartsWith(keyword))
                {
                    bestMatch = item;
                    minCost = 0;
                    break;
                }

                int currentCost = Compute(keyword, item);
                if (currentCost < minCost)
                {
                    minCost = currentCost;
                    bestMatch = item;
                }
            }

            if (minCost > keyword.Length)
            {
                return string.Empty;
            }
            else
            {
                return bestMatch;
            }
        }
    }
}

