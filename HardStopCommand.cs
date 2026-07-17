using System;
using System.Threading;

public class HardStopCommand : ICommand
{
    private readonly ServerThread _serverThread;

    public HardStopCommand(ServerThread serverThread)
    {
        _serverThread = serverThread ?? throw new ArgumentNullException(nameof(serverThread));
    }

    public void Execute()
    {
        if (Thread.CurrentThread != _serverThread.UnderlyingThread)
        {
            throw new InvalidOperationException();
        }

        _serverThread.StopLoop();
    }
}