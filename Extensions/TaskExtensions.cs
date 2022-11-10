using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vacation_Portal.Extensions
{
    public static class TaskExtensions
    {
        public async static void Await(this Task task)
        {
            await task;
        }
    }
}
