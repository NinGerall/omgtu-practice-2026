using System;
using System.Collections.Concurrent;
using System.Threading;
using Xunit;

public class ServerThreadTests
{
    [Fact]
    public void HardStop_ShouldStopImmediately_EvenIfCommandsRemainInQueue()
    {
        var queue = new BlockingCollection<ICommand>();
        var server = new ServerThread(queue);
        var indicator = false;
        var dummyCommand = new ActionCommand(() => indicator = true);
        var hardStop = new HardStopCommand(server);

        queue.Add(hardStop);
        queue.Add(dummyCommand);

        server.Start();
        server.Join();

        Assert.False(indicator);
    }

    [Fact]
    public void SoftStop_ShouldExecuteAllRemainingCommands_BeforeStopping()
    {
        var queue = new BlockingCollection<ICommand>();
        var server = new ServerThread(queue);
        var indicator = false;
        var dummyCommand = new ActionCommand(() => indicator = true);
        var softStop = new SoftStopCommand(server);

        queue.Add(softStop);
        queue.Add(dummyCommand);

        server.Start();
        server.Join();

        Assert.True(indicator);
    }

    [Fact]
    public void StopCommands_ExecutedInWrongThread_ShouldThrowException()
    {
        var queue = new BlockingCollection<ICommand>();
        var server = new ServerThread(queue);
        var hardStop = new HardStopCommand(server);

        Assert.Throws<InvalidOperationException>(() => hardStop.Execute());
    }
}

public class ActionCommand : ICommand
{
    private readonly Action _action;
    public ActionCommand(Action action) => _action = action;
    public void Execute() => _action();
}