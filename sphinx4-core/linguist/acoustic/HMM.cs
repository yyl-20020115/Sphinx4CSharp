namespace edu.cmu.sphinx.linguist.acoustic
{
	public interface HMM
	{
		Unit getBaseUnit();

		HMMPosition getPosition();

		Unit getUnit();

		HMMState getState(int i);

		int getOrder();

		HMMState getInitialState();
	}
}
