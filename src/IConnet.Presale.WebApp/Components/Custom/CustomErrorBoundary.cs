using Microsoft.AspNetCore.Components.Web;

namespace IConnet.Presale.WebApp.Components.Custom;

public class CustomErrorBoundary : ErrorBoundary
{
    public new Exception? CurrentException => base.CurrentException;
}
