using System;

public class LongRunningFakeCommand : ICommand
{
    private readonly IScheduler _scheduler;
    private readonly ServerThread _server;
    private int _stepsRemaining;

    public int StepsExecuted { get; private set; }

    public LongRunningFakeCommand(IScheduler scheduler, ServerThread server, int steps)
    {
        _scheduler = scheduler;
        _server = server;
        _stepsRemaining = steps;
    }

    public void Execute()
    {
        StepsExecuted++;
        _stepsRemaining--;

        if (_stepsRemaining > 0)
        {
            _scheduler.Add(this);
            _server.Pulse();
        }
    }
}