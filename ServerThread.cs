using System;
using System.Collections.Concurrent;
using System.Threading;

public class ServerThread
{
    private readonly BlockingCollection<ICommand> _queue;
    private readonly Thread _thread;
    private Action _action;
    private bool _isStopped;

    public ServerThread(BlockingCollection<ICommand> queue)
    {
        _queue = queue ?? throw new ArgumentNullException(nameof(queue));
        _thread = new Thread(Run);
        
        _action = () =>
        {
            try
            {
                foreach (var command in _queue.GetConsumingEnumerable())
                {
                    ExecuteCommand(command);
                    
                    if (_isStopped)
                        break;
                }
            }
            catch (ObjectDisposedException) { }
            catch (Exception) { }
        };
    }

    public void Start() => _thread.Start();
    public void Join() => _thread.Join();
    public Thread UnderlyingThread => _thread;

    private void Run()
    {
        while (!_isStopped)
        {
            _action();
        }
    }

    public void ExecuteCommand(ICommand command)
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

    public void UpdateAction(Action newAction)
    {
        _action = newAction;
    }

    public void StopLoop()
    {
        _isStopped = true;
    }
}