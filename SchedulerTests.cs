using System;
using System.Collections.Concurrent;
using System.Threading;
using Xunit;

public class SchedulerTests
{
    [Fact]
    public void Scheduler_ShouldExecuteLongRunningCommandInPieces_WithoutDeadlock()
    {
        var queue = new BlockingCollection<ICommand>();
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(queue, scheduler);

        var longCmd = new LongRunningFakeCommand(scheduler, server, 3);
        
        queue.Add(longCmd);
        server.Pulse();

        var stopCmd = new ActionCommand(() => server.StopLoop());
        
        var timer = new Timer(_ => 
        {
            queue.Add(stopCmd);
            server.Pulse();
        }, null, 200, Timeout.Infinite);

        server.Start();
        server.Join();
        timer.Dispose();

        Assert.Equal(3, longCmd.StepsExecuted);
    }

    [Fact]
    public void RoundRobinScheduler_ShouldAlternateCommands()
    {
        var scheduler = new RoundRobinScheduler();
        var order = new ConcurrentBag<string>();

        var cmd1 = new ActionCommand(() => order.Add("A"));
        var cmd2 = new ActionCommand(() => order.Add("B"));

        scheduler.Add(cmd1);
        scheduler.Add(cmd2);

        var executed1 = scheduler.Select();
        var executed2 = scheduler.Select();

        executed1.Execute();
        executed2.Execute();

        var resultArray = order.ToArray();
        Array.Reverse(resultArray);

        Assert.Equal("A", resultArray[0]);
        Assert.Equal("B", resultArray[1]);
    }
}