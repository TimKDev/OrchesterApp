namespace TvJahnOrchesterApp.Application.Common.ExtensionMethods;

public static class TaskExtensions
{
    /// <summary>
    /// Runs two tasks in parallel and returns their results as a tuple
    /// </summary>
    /// <typeparam name="T1">Type of the first task result</typeparam>
    /// <typeparam name="T2">Type of the second task result</typeparam>
    /// <param name="task1">The first task</param>
    /// <param name="task2">The second task to run in parallel</param>
    /// <returns>A tuple containing both task results</returns>
    public static async Task<(T1, T2)> Parallelize<T1, T2>(this Task<T1> task1, Task<T2> task2)
    {
        await Task.WhenAll(task1, task2);
        return (await task1, await task2);
    }

    /// <summary>
    /// Runs 3 tasks in parallel using fluent syntax
    /// </summary>
    /// <typeparam name="T1">Type of the first task result</typeparam>
    /// <typeparam name="T2">Type of the second task result</typeparam>
    /// <typeparam name="T3">Type of the third task result</typeparam>
    /// <param name="tuple">Tuple from previous ParallelWith call</param>
    /// <param name="task3">The third task to add</param>
    /// <returns>A tuple containing all three task results</returns>
    public static async Task<(T1, T2, T3)> And<T1, T2, T3>(this Task<(T1, T2)> tuple, Task<T3> task3)
    {
        await Task.WhenAll(tuple, task3);
        var (result1, result2) = await tuple;

        return (result1, result2, await task3);
    }
}