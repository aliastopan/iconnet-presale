using Microsoft.AspNetCore.Components.Server.Circuits;

namespace IConnet.Presale.WebApp.WebSockets;

public class CustomCircuitHandler : CircuitHandler
{
    public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        string state = "Connection Up";
        try
        {
            return base.OnConnectionUpAsync(circuit, cancellationToken);
        }
        catch (Exception exception)
        {
            LogSwitch.Debug("Exception has occurred {during}: {exception}", state, exception.Message);
            return base.OnConnectionUpAsync(circuit, cancellationToken);
        }
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        string state = "Connection Down";
        try
        {
            return base.OnConnectionDownAsync(circuit, cancellationToken);
        }
        catch (Exception exception)
        {
            LogSwitch.Debug("Exception has occurred {during}: {exception}", state, exception.Message);
            return base.OnConnectionDownAsync(circuit, cancellationToken);
        }
    }
}
