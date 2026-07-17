using System;

public class TestCommand : ICommand
{
    private readonly int _id;
    private readonly IScheduler _scheduler;
    private readonly ServerThread _server;
    private int _counter;

    public int Counter => _counter;

    public TestCommand(int id, IScheduler scheduler, ServerThread server)
    {
        _id = id;
        _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        _server = server ?? throw new ArgumentNullException(nameof(server));
    }

    public void Execute()
    {
        _counter++;
        Console.WriteLine($"Поток {_id} вызов {_counter}");

        if (_counter < 3)
        {
            _scheduler.Add(this);
            _server.Pulse();
        }
    }
}