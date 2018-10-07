using System;
using System.Collections;

namespace Util
{
	public class FillBitElementHelper
	{
		/**
		 * 
		 * @param arrayCheck 
		 * @param elementId 
		 * @return
		 */
		public static bool checkElement(byte[] arrayCheck, int elementId)
	    {	
	        byte x = (byte)((int)elementId / 8);
	        byte y = (byte)((int)elementId % 8);
	        if (x >= arrayCheck.Length)
	        {
	            return false;
	        }
	        
	        return (arrayCheck[x] & (1 << y)) != 0;
	    }
	    
	    /**
	     * 
	     * @param arrayCheck 
	     * @param elementId 
	     * @param Passed 
	     */
	    public static void SetElement(byte[] arrayCheck, int elementId, bool Passed)
	    {
	        byte x = (byte)((int)elementId / 8);
	        byte y = (byte)((int)elementId % 8);
	        if (Passed)
	        {
	            arrayCheck[x] = (byte)(arrayCheck[x]| (byte)(1 << y));
	        }
	        else
	        {
	            arrayCheck[x] = (byte)(arrayCheck[x]& ~(byte)(1 << y));
	        }
	    }
	}
}

