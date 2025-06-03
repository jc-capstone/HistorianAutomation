using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianAutomation.Framework
{
    public class WaitHelper
    {
        public static void WaitFor(int time, string reason)
        {
            Thread.Sleep(time);
        }

        public static void WaitFor(Func<bool> action, int trys = 10, TimeSpan waitInterval = default)
        {
            if (waitInterval == default) waitInterval = TimeSpan.FromMilliseconds(500);

            for (int i = 1; i < trys; i++)
            {
                try
                {
                    var result = action();
                    if (result)
                    {
                        WaitFor(500, "Allow for UI Refesh");
                        break;
                    }
                    else if (i == trys)
                    {
                        throw new TimeoutException($"Timeout occured after {trys} attempts");
                    }
                    else
                    {
                        Task.Delay(waitInterval).Wait();
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
        }

        public static async Task<T> WaitForAsync<T>(Func<Task<T>> condition, int polls = 60, int pollingInterval = 500)
        {
            ArgumentNullException.ThrowIfNull(condition);
            T result = default;
            for (var i = 0; i < polls; i++)
            {
                result = await condition();
                if (result != null)
                    break;
                else if (i == polls - 1)
                    throw new TimeoutException($"Timed out after {polls} polls with interval {pollingInterval} milliseconds.");
                else
                    await Task.Delay(pollingInterval);
            }
            return result;
        }
    }
}
