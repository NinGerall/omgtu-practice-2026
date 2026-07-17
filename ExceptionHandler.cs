using System;

public static class ExceptionHandler
{
    public static void Handle(ICommand command, Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}