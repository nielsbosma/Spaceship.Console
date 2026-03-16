namespace Spaceship.Console.Infrastructure;

public class SpaceshipException(string message) : Exception(message);

public class SpaceshipApiException(string message, int statusCode) : SpaceshipException(message)
{
    public int StatusCode { get; } = statusCode;
}
