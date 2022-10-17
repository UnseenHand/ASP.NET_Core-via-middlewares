public class RoutingMiddleware
{
    readonly RequestDelegate next;
    public RoutingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request.Path;
        if (path == "/index" || path == @"^/index/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$")
        {
            await next.Invoke(context);
        }
        else if (path == "/about")
        {
            await next.Invoke(context);
        }
        else
        {
            context.Response.StatusCode = 404;
        }
    }
}
