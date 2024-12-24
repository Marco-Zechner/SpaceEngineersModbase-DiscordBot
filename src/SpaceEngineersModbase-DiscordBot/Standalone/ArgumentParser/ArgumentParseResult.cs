namespace MarcoZechner.Standalone.ArgumentParser {
    public record ArgumentParserResult(bool Success, string ErrorMessage, Dictionary<string, string> FlagArgumentPairs, List<string> PositionalArguments);
}