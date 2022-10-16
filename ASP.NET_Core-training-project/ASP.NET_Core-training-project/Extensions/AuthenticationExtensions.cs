public static class AuthenticationExtensions
{
    public static IApplicationBuilder UseDefinedAuthentication(this IApplicationBuilder builder, string pattern)
    {
        return builder.UseMiddleware<AuthenticationMiddleware>(pattern);
    }
}