namespace KiddooPlaySchool.Shared.Utilities;

public static class StringHelper
{
    public static bool IsValidEmail(string email)
    {
        return email.Contains('@');
    }
}
