using System;
using System.Threading.Tasks;

namespace FunctionalMonads.Monads.TaskMonad
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Applies the mapFunction to the task.
        /// </summary>
        /// <typeparam name="TIn">The type of the input task.</typeparam>
        /// <typeparam name="TOut">The type of the output task.</typeparam>
        /// <param name="task">The task.</param>
        /// <param name="mapFunc">The map function.</param>
        /// <param name="continueOnCapturedContext"><c>true</c> to attempt to marshal the continuation back to the original context captured; otherwise, <c>false</c>.</param>
        /// <returns>A new task which contains the result of the applied mapFunction.</returns>
        /// <exception cref="ArgumentNullException">
        /// mapFunc
        /// or
        /// task.
        /// </exception>
        public static Task<TOut> Map<TIn, TOut>(this Task<TIn> task, Func<TIn, TOut> mapFunc, bool continueOnCapturedContext = false)
        {
            if (mapFunc == null)
            {
                throw new ArgumentNullException(nameof(mapFunc));
            }

            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            return MapInternal(task, mapFunc, continueOnCapturedContext);
        }

        /// <summary>
        /// Flatten a nested Task of Task to Task.
        /// </summary>
        /// <typeparam name="T">The inner Type.</typeparam>
        /// <param name="task">The nested </param>
        /// <returns>A flattend Task.</returns>
        /// <exception cref="ArgumentNullException">If taks is null.</exception>
        public static async Task<T> Flatten<T>(this Task<Task<T>> task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            return await await task;
        }

        /// <summary>
        /// Replaces one contained object with another contained object.
        /// It can be viewed as a combination of <see cref="Map{TIn,TOut}" /> and
        /// <see cref="Task{TResult}.Result" />. Useful for chaining many Task operations.
        /// </summary>
        /// <typeparam name="TIn">The type of the input task.</typeparam>
        /// <typeparam name="TOut">The type of the output task.</typeparam>
        /// <param name="task">The task.</param>
        /// <param name="bindFunc">The bind function.</param>
        /// <param name="continueOnCapturedContext"><c>true</c> to attempt to marshal the continuation back to the original context captured; otherwise, <c>false</c>.</param>
        /// <returns>A new task which contains the result of the applied bindFunction.</returns>
        /// <exception cref="ArgumentNullException">
        /// bindFunc
        /// or
        /// task.
        /// </exception>
        public static Task<TOut> Bind<TIn, TOut>(this Task<TIn> task, Func<TIn, Task<TOut>> bindFunc, bool continueOnCapturedContext = false)
        {
            if (bindFunc == null)
            {
                throw new ArgumentNullException(nameof(bindFunc));
            }

            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            return BindInternal(task, bindFunc, continueOnCapturedContext);
        }

        private static async Task<TOut> MapInternal<TIn, TOut>(Task<TIn> task, Func<TIn, TOut> mapFunc, bool continueOnCapturedContext) =>
            mapFunc(await task.ConfigureAwait(continueOnCapturedContext));

        private static Task<TOut> BindInternal<TIn, TOut>(Task<TIn> task, Func<TIn, Task<TOut>> bindFunc, bool continueOnCapturedContext) =>
            task.Map(bindFunc, continueOnCapturedContext).Flatten();
    }
}