using System;
using System.Collections.Generic;

namespace ConvertToArab
{
	public class ConvertToArabic
	{

		Dictionary<char,int> romain = new Dictionary<char,int> ();

		public ConvertToArabic ()
		{
			// populate arabics
			romain.Add('I',1);
			romain.Add('V',5);
			romain.Add('X',10);
			romain.Add('L', 50);
			romain.Add('C', 100);
			romain.Add('D', 500);
			romain.Add('M', 1000);
		}

		int FindValue ( char c )
		{
			int nout;
			if (!romain.TryGetValue(c,out nout) )
				throw new Exception ("Incorrect nombre romain!!");
			return nout;
		}

		public int Convert ( string numeric )
		{
			int total = 0;

			for ( int i = 0 ; i < numeric.Length ; ++i )
			{
				int valeur = FindValue( numeric[i]);

				// Regarde si on est dans le cas IV, ou IX
				if(  numeric.Length > i + 1 && FindValue(numeric[i + 1 ]) > valeur )
				{
					total += FindValue(numeric[i + 1 ]) - valeur;
					++i;
				}
				else
					total += valeur;

			}

			return total;
		}
	}
}

