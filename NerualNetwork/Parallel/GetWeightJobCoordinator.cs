using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerualNetwork.Parallel
{
    internal class GetWeightJobCoordinator : JobCoordinator<GetWeightJob>
    {
        public GetWeightJobCoordinator(Queue<GetWeightJob> jobs) : base(jobs)
        {

        }

        protected override void WorkAtJob()
        {
            GetWeightJob job;

            lock (_jobs)
            {
                job = _jobs.Dequeue();
            }

            job.neuron.GetError(job.weightCoord, job.layer);

            notEndedJobsCount--;
        }
    }
}
