namespace URLTester.Domain.Helpers;

public static class MethodHelper
{
    public static string GenerateShortened(bool generateShortened)
    {
        string result;

        do
        {
            result = GenerateRandomString(5);
        } while (!generateShortened);

        return result;
    }

    private static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var stringChars = new char[length];

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        return new string(stringChars);
    }
}
