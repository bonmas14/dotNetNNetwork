using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerualNetwork.Parallel
{
    internal abstract class JobCoordinator<T>
    {
        protected Queue<T> _jobs;

        protected int notEndedJobsCount;


        public JobCoordinator(Queue<T> jobs)
        {
            _jobs = jobs;
            notEndedJobsCount = _jobs.Count;
        }

        public void Coordinate()
        {
            List<Task> tasks = new List<Task>(Environment.ProcessorCount);

            for (int i = 0; i < notEndedJobsCount; i++)
            {
                tasks.Clear();

                int startedJobs = Math.Min(_jobs.Count, Environment.ProcessorCount);
                
                while (startedJobs > 0)
                {
                    startedJobs -= 1;

                    tasks.Add(Task.Run(WorkAtJob));
                }

                Task.WaitAll(tasks.ToArray());
            }
        }

        protected abstract void WorkAtJob();
    }
}
