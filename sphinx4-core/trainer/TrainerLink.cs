using java.lang;

namespace edu.cmu.sphinx.trainer
{
	public class TrainerLink : Object
	{
		public TrainerLink(Edge transition, TrainerToken source, TrainerToken destination)
		{
			this.source = source;
			this.transition = transition;
			this.destination = destination;
		}

		public virtual TrainerToken getSource()
		{
			return this.source;
		}

		public virtual TrainerToken getDestination()
		{
			return this.destination;
		}

		public virtual Edge getTransition()
		{
			return this.transition;
		}

		private TrainerToken source;

		private TrainerToken destination;

		private Edge transition;
	}
}
