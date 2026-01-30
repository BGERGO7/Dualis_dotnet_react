namespace React_dotnet.Server.Middlewares
{
    public class AuthorizationHeaderSetterMiddleware
    {
        private readonly RequestDelegate next;

        public AuthorizationHeaderSetterMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync (HttpContext context)
        {
            context.Request.Cookies.TryGetValue(Constants.AccessTokenCookieKey, out string? accessToken);

            if(string.Compare("/auth/login", context.Request.Path, StringComparison.OrdinalIgnoreCase) != 0 && 
                !string.IsNullOrEmpty(accessToken))
            {
                context.Request.Headers.Authorization = $"Bearer {accessToken}";
            }

            await next(context);
        }
    }
}
