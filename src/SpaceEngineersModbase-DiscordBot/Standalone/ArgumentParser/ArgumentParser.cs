using System.Collections.ObjectModel;

namespace MarcoZechner.Standalone.ArgumentParser {
    public abstract class ArgumentParser {
        /// <summary>
        /// Parses the given arguments according to the given pattern.
        /// "-fj -e value1 --this value2 --unit pos1 pos2 pos3" 
        /// -> FlagArgumentPairs: { "f", "j", ("e", "value1"), ("this", "value2"), "unit" }
        /// -> PositionalArguments: { "pos1", "pos2", "pos3" }
        /// -> ErrorMessage: ""
        /// <- Min Pattern for the Arguments above to be valid: "fje:[this]:[unit];0-3" // order of flags and positional arguments is not checked
        /// Explanation:
        /// "fj" -> single flags without values
        /// "e:" -> single flag with value
        /// "[this]:" -> long flag with value
        /// "[unit]" -> long flag without value
        /// ";" -> separates flags from positional arguments
        /// "0-3" -> 0 to 3 positional arguments
        /// alternative:
        /// "0,1,3-5" -> 0, 1, 3, 4 or 5 positional arguments (2 would be invalid)
        /// </summary>
        /// <param name="args"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static ArgumentParserResult ParseArgs(Collection<string> args, string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return ParseArgs(args, new ArgumentParserPattern([], []));
            }

            int separatorCount = pattern.Where(c => c == ';').Count();
            if (separatorCount > 1)
            {
                return new ArgumentParserResult(false, $"Invalid pattern: {pattern}\nonly one separator allowed", [], []);
            }

            var parts = pattern.Split(';');
            string flags = parts[0];
            string positionalArguments = parts.Length > 1 ? parts[1] : string.Empty;

            Dictionary<string, bool> flagMap = [];
            HashSet<int> validPositionalArgumentCounts = [];

            var result = TryParseFlags(flags, flagMap);
            if (!result.Success) {
                return result;
            }

            result = TryParsePositionalArguments(positionalArguments, validPositionalArgumentCounts);
            if (!result.Success) {
                return result;
            }

            return ParseArgs(args, new ArgumentParserPattern(flagMap, validPositionalArgumentCounts));
        }

        private static ArgumentParserResult TryParseFlags(string flags, Dictionary<string, bool> flagMap)
        {
            List<char> flagParts = [.. flags];

            bool isLongFlag = false;
            string currentFlag = string.Empty;

            while (flagParts.Count > 0)
            {
                char current = flagParts[0];
                bool needsValue = false;
                flagParts = flagParts[1..];

                if (current == '[')
                {
                    if (isLongFlag)
                    {
                        return new ArgumentParserResult(false, $"Invalid pattern: {flags}\nlong flag cannot be nested", [], []);
                    }
                    isLongFlag = true;
                    continue;
                }

                if (current == ']')
                {
                    if (!isLongFlag)
                    {
                        return new ArgumentParserResult(false, $"Invalid pattern: {flags}\nlong flag not opened before closed", [], []);
                    }
                    isLongFlag = false;

                    flagMap[currentFlag] = needsValue;
                    currentFlag = string.Empty;
                    continue;
                }

                if (flagParts.Count > 0 && flagParts[0] == ':')
                {
                    if (isLongFlag)
                    {
                        return new ArgumentParserResult(false, $"Invalid pattern: {flags}\n':' needs to come after the long flag is closed", [], []);
                    }
                    needsValue = true;
                    flagParts = flagParts[1..];
                    continue;
                }

                if (current == ':')
                {
                    return new ArgumentParserResult(false, $"Invalid pattern: {flags}\n':' needs a flag before it", [], []);
                }

                if (isLongFlag)
                {
                    currentFlag += current;
                    continue;
                }

                flagMap[current.ToString()] = needsValue;
            }

            return new ArgumentParserResult(true, string.Empty, [], []);
        }

        private static ArgumentParserResult TryParsePositionalArguments(string positionalArguments, HashSet<int> validPositionalArgumentCounts)
        {
            return null;
        }

        public static ArgumentParserResult ParseArgs(Collection<string> args, ArgumentParserPattern pattern) {
            var flagArgumentPairs = new Dictionary<string, string>();
            var positionalArguments = new List<string>();
            var errorMessage = string.Empty;
            
            //TODO:
            // Check that all flags are before positional arguments
            // 

            return null;
        }

        public record ArgumentParserPattern(Dictionary<string, bool> Flags, HashSet<int> ValidPositionalArgumentCounts);
    }
}