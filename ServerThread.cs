using System;
using System.Collections.Concurrent;
using System.Threading;

public class ServerThread
{
    private readonly BlockingCollection<ICommand> _queue;
    private readonly IScheduler _scheduler;
    private readonly Thread _thread;
    private readonly AutoResetEvent _signal = new AutoResetEvent(false);
    private bool _isStopped;

    public ServerThread(BlockingCollection<ICommand> queue, IScheduler scheduler)
    {
        _queue = queue ?? throw new ArgumentNullException(nameof(queue));
        _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        _thread = new Thread(Run);
    }

    public void Start() => _thread.Start();
    public void Join() => _thread.Join();
    public Thread UnderlyingThread => _thread;

    public void Pulse()
    {
        _signal.Set();
    }

    private void Run()
    {
        while (!_isStopped)
        {
            bool hasWork = false;

            if (_queue.TryTake(out var queueCmd))
            {
                hasWork = true;
                ExecuteCommand(queueCmd);
            }

            if (_scheduler.HasCommand())
            {
                var schedulerCmd = _scheduler.Select();
                if (schedulerCmd != null)
                {
                    hasWork = true;
                    ExecuteCommand(schedulerCmd);
                }
            }

            if (!hasWork && !_isStopped)
            {
                _signal.WaitOne(10);
            }
        }
    }

    private void ExecuteCommand(ICommand command)
    {
        try
        {
            command.Execute();
        }
        catch (Exception ex)
        {
            ExceptionHandler.Handle(command, ex);
        }
    }

    public void StopLoop()
    {
        _isStopped = true;
        _signal.Set();
    }
}