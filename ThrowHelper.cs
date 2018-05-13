using System;

namespace IntrinsicsPlayground
{
	public static class ThrowHelper
	{
		// separate methods to throw exception
		// in order to have more chances to inline methods

		public static void ArgumentNullException()
		{
			throw new ArgumentNullException();
		}
	}
}
