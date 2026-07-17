using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Xunit;

public class LongRunningTaskTests
{
    [Fact]
    public void TestCommand_ShouldExecuteExactlyThreeTimesForFiveInstances_AndThenHardStop()
    {
        var queue = new BlockingCollection<ICommand>();
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(queue, scheduler);

        var commands = new List<TestCommand>();
        for (int i = 1; i <= 5; i++)
        {
            var cmd = new TestCommand(i, scheduler, server);
            commands.Add(cmd);
            scheduler.Add(cmd);
        }

        var hardStop = new HardStopCommand(server);
        
        var timer = new Timer(_ =>
        {
            queue.Add(hardStop);
            server.Pulse();
        }, null, 300, Timeout.Infinite);

        server.Start();
        server.Join();
        timer.Dispose();

        foreach (var cmd in commands)
        {
            Assert.Equal(3, cmd.Counter);
        }
    }
}