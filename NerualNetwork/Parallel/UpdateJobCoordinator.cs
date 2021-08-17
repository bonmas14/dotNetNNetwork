using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerualNetwork.Parallel
{
    internal class UpdateJobCoordinator : JobCoordinator<UpdateJob>
    {
        public UpdateJobCoordinator(Queue<UpdateJob> jobs) : base(jobs)
        {

        }

        protected override void WorkAtJob()
        {
            UpdateJob job;

            lock (_jobs)
            {
                job = _jobs.Dequeue();
            }

            job.neuron.UpdateData(job.layer);

            notEndedJobsCount--;
        }
    }
}
