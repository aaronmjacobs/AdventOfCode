namespace _8
{
    public class Program
    {
        public static Dictionary<char, char> DecodePatterns(List<string> patterns)
        {
            Dictionary<char, List<char>> possiblePositions = new();
            possiblePositions.Add('a', new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' });
            possiblePositions.Add('b', new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' });
            possiblePositions.Add('c', new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' });
            possiblePositions.Add('d', new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' });
            possiblePositions.Add('e', new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' });
            possiblePositions.Add('f', new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' });
            possiblePositions.Add('g', new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' });

            foreach (string pattern in patterns)
            {
                if (pattern.Length == 2) // 1
                {
                    possiblePositions['c'].RemoveAll(x => !pattern.Contains(x));
                    possiblePositions['f'].RemoveAll(x => !pattern.Contains(x));

                    possiblePositions['a'].RemoveAll(x => pattern.Contains(x));
                    possiblePositions['b'].RemoveAll(x => pattern.Contains(x));
                    possiblePositions['d'].RemoveAll(x => pattern.Contains(x));
                    possiblePositions['e'].RemoveAll(x => pattern.Contains(x));
                    possiblePositions['g'].RemoveAll(x => pattern.Contains(x));
                }
                else if (pattern.Length == 4) // 4
                {
                    possiblePositions['b'].RemoveAll(x => !pattern.Contains(x));
                    possiblePositions['c'].RemoveAll(x => !pattern.Contains(x));
                    possiblePositions['d'].RemoveAll(x => !pattern.Contains(x));
                    possiblePositions['f'].RemoveAll(x => !pattern.Contains(x));

                    possiblePositions['a'].RemoveAll(x => pattern.Contains(x));
                    possiblePositions['e'].RemoveAll(x => pattern.Contains(x));
                    possiblePositions['g'].RemoveAll(x => pattern.Contains(x));
                }
                else if (pattern.Length == 3) // 7
                {
                    possiblePositions['a'].RemoveAll(x => !pattern.Contains(x));
                    possiblePositions['c'].RemoveAll(x => !pattern.Contains(x));
                    possiblePositions['f'].RemoveAll(x => !pattern.Contains(x));

                    possiblePositions['b'].RemoveAll(x => pattern.Contains(x));
                    possiblePositions['d'].RemoveAll(x => pattern.Contains(x));
                    possiblePositions['e'].RemoveAll(x => pattern.Contains(x));
                    possiblePositions['g'].RemoveAll(x => pattern.Contains(x));
                }
                else if (pattern.Length == 7) // 8
                {
                }
            }

            foreach (string pattern in patterns)
            {
                if (pattern.Length == 6)
                {
                    // Pattern represents either 0, 6, or 9

                    // D missing = 0
                    // C missing = 6
                    // E missing = 9

                    {
                        bool isZero = false;
                        char missingChar = '\0';
                        foreach (char x in possiblePositions['d'])
                        {
                            if (!pattern.Contains(x))
                            {
                                missingChar = x;
                                isZero = true;
                                break;
                            }
                        }
                        if (isZero)
                        {
                            possiblePositions['d'].RemoveAll(x => x != missingChar);

                            possiblePositions['a'].RemoveAll(x => x == missingChar);
                            possiblePositions['b'].RemoveAll(x => x == missingChar);
                            possiblePositions['c'].RemoveAll(x => x == missingChar);
                            possiblePositions['e'].RemoveAll(x => x == missingChar);
                            possiblePositions['f'].RemoveAll(x => x == missingChar);
                            possiblePositions['g'].RemoveAll(x => x == missingChar);
                        }
                    }

                    {
                        bool isSix = false;
                        char missingChar = '\0';
                        foreach (char x in possiblePositions['c'])
                        {
                            if (!pattern.Contains(x))
                            {
                                missingChar = x;
                                isSix = true;
                                break;
                            }
                        }
                        if (isSix)
                        {
                            possiblePositions['c'].RemoveAll(x => x != missingChar);

                            possiblePositions['a'].RemoveAll(x => x == missingChar);
                            possiblePositions['b'].RemoveAll(x => x == missingChar);
                            possiblePositions['d'].RemoveAll(x => x == missingChar);
                            possiblePositions['e'].RemoveAll(x => x == missingChar);
                            possiblePositions['f'].RemoveAll(x => x == missingChar);
                            possiblePositions['g'].RemoveAll(x => x == missingChar);
                        }
                    }

                    {
                        bool isNine = false;
                        char missingChar = '\0';
                        foreach (char x in possiblePositions['e'])
                        {
                            if (!pattern.Contains(x))
                            {
                                missingChar = x;
                                isNine = true;
                                break;
                            }
                        }
                        if (isNine)
                        {
                            possiblePositions['e'].RemoveAll(x => x != missingChar);

                            possiblePositions['a'].RemoveAll(x => x == missingChar);
                            possiblePositions['b'].RemoveAll(x => x == missingChar);
                            possiblePositions['c'].RemoveAll(x => x == missingChar);
                            possiblePositions['d'].RemoveAll(x => x == missingChar);
                            possiblePositions['f'].RemoveAll(x => x == missingChar);
                            possiblePositions['g'].RemoveAll(x => x == missingChar);
                        }
                    }
                }
            }

            Dictionary<char, char> mappings = new();
            mappings[possiblePositions['a'][0]] = 'a';
            mappings[possiblePositions['b'][0]] = 'b';
            mappings[possiblePositions['c'][0]] = 'c';
            mappings[possiblePositions['d'][0]] = 'd';
            mappings[possiblePositions['e'][0]] = 'e';
            mappings[possiblePositions['f'][0]] = 'f';
            mappings[possiblePositions['g'][0]] = 'g';

            return mappings;
        }

        public static string Translate(Dictionary<char, char> mappings, string text)
        {
            string translated = "";
            foreach (char c in text)
            {
                translated += mappings[c];
            }
            return translated;
        }

        public static List<int> DetermineDigits(Dictionary<char, char> mappings, List<string> outputValues)
        {
            string[] digitTable = new string[] { "abcefg", "cf", "acdeg", "acdfg", "bcdf", "abdfg", "abdefg", "acf", "abcdefg", "abcdfg" };

            List<int> digits = new();

            foreach (string outputValue in outputValues)
            {
                string translatedOutput = Translate(mappings, outputValue);

                for (int i = 0; i < digitTable.Length; i++)
                {
                    if (digitTable[i].Length == translatedOutput.Length)
                    {
                        bool containsAll = true;
                        for (int j = 0; j < translatedOutput.Length; j++)
                        {
                            if (!digitTable[i].Contains(translatedOutput[j]))
                            {
                                containsAll = false;
                                break;
                            }
                        }

                        if (containsAll)
                        {
                            digits.Add(i);
                        }
                    }
                }
            }

            return digits;
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            int numTimes = 0;
            int total = 0;
            foreach (string line in lines)
            {
                List<string> halves = line.Split('|').ToList();
                List<string> patterns = halves[0].Trim().Split(' ').ToList();
                List<string> outputValues = halves[1].Trim().Split(' ').ToList();

                foreach (string outputValue in outputValues)
                {
                    if (outputValue.Length == 2 || outputValue.Length == 4 || outputValue.Length == 3 || outputValue.Length == 7)
                    {
                        ++numTimes;
                    }
                }

                Dictionary<char, char> mappings = DecodePatterns(patterns);
                List<int> digits = DetermineDigits(mappings, outputValues);

                int decodedOutput = 0;
                int power = 1;
                for (int i = digits.Count - 1; i >= 0; --i)
                {
                    decodedOutput += digits[i] * power;
                    power *= 10;
                }

                total += decodedOutput;
            }

            Console.WriteLine(numTimes);
            Console.WriteLine(total);

            Console.ReadLine();
        }
    }
}
