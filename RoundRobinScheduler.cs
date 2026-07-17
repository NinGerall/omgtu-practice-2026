using System;
using System.Collections.Concurrent;

public class RoundRobinScheduler : IScheduler
{
    private readonly ConcurrentQueue<ICommand> _longRunningCommands = new ConcurrentQueue<ICommand>();

    public bool HasCommand()
    {
        return !_longRunningCommands.IsEmpty;
    }

    public ICommand Select()
    {
        if (_longRunningCommands.TryDequeue(out var cmd))
        {
            return cmd;
        }
        return null;
    }

    public void Add(ICommand cmd)
    {
        if (cmd == null) throw new ArgumentNullException(nameof(cmd));
        _longRunningCommands.Enqueue(cmd);
    }
}