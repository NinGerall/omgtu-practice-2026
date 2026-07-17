using System;
using System.Threading;

public class SoftStopCommand : ICommand
{
    private readonly ServerThread _serverThread;

    public SoftStopCommand(ServerThread serverThread)
    {
        _serverThread = serverThread ?? throw new ArgumentNullException(nameof(serverThread));
    }

    public void Execute()
    {
        if (Thread.CurrentThread != _serverThread.UnderlyingThread)
        {
            throw new InvalidOperationException();
        }

        _serverThread.UpdateAction(() =>
        {
            _serverThread.StopLoop();
        });
    }
}