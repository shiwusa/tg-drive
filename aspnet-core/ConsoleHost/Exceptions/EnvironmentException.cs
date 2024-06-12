namespace TgDrive.BotClient.Host;

public class EnvironmentException : Exception
{
    public EnvironmentException(string variablePurpose, string variableName)
        : base(
            $"You should provide {variablePurpose} in {variableName} environment variable")
    {
    }
}
