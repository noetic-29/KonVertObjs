// Copyright Noetic-29 LLC 2014 - 2018
// All rights reserved
// www.noetic-29.com
//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
/*
using JSONObject = org.json.simple.JSONObject;
using JsonParseException = com.google.gson.JsonParseException;
//import java.math.BigInteger;
//import java.math.MathContext;
*/

namespace KonVertObjs
{
	/// <summary>
	/// REDESIGN TO USE BIGDECIMAL
	/// 
	/// @author EOwens
	/// @date 2014-05-22
	/// 
	/// In order to avoid the inherent build up of computational errors using double
	/// convert to using BigDecimal for values and calculations.  However, there is 
	/// still the issue of 0.33333etc which may stilL bite us.  Therefore, first,
	/// adopt only a single conversion factor (so far, the from multiplier is always
	/// the inverse of the to multiplier).  And assume that if from add first is true 
	/// then to add first is false.  So adopt for that single multiplier two long
	/// integers that are the multiplier and the divider.  e.g. for inches to mm,
	/// which is 2.543 inches per millimeter represent as 254 / 100 and do multiply
	/// first, only finally with division.
	/// </summary>

	// parameters required for conversion to/from a base unit
	public class KonVertParams : KonObj
	{
        // 2017-12-07 EIO nothing to fix so don't override fixReferences

        public KonVertParams(KonVertSet aSet) : base(aSet) { }

        // 2017-12-08 EIO nothing to fix

        //public static MathContext theMathContext = new MathContext(1, RoundingMode.HALF_EVEN);

        //PROPERTIES
        // value to multiply when converting from self to base unit
        public long myMultiplier { get; set; }

		// value to divide when converting from self to base unit
        public long myDivider { get; set; }

		// value to add when converting from self from base unit
        public decimal myAdder { get; set; }

		// WHEN CONVERTING FROM SELF TO BASE UNIT
		// if True add Adder to value before multiplying/dividing
		// if False multiply/divide and then add Adder
		// WHEN CONVERTING FROM BASE UNIT TO SELF INVERSE IS TRUE
		// if True multiply/divide first and then add
		// if false, add first then multiply/divide
        public bool myAddFirst { get; set; }

        public int myPrecision { get; set; }

		// Convert From a number of this units to a number of base units for whatever set of base units this params object represents
		public virtual decimal doKonvertFromSelf(decimal aNumThisUnits, KonVersionGroup aGroup)
		{
			decimal myCalc = 0m;
			//myCalc.setScale(privatemyPrecision + 1, RoundingMode.HALF_EVEN);
			decimal myRet = 0m;
			//myRet.setScale(privatemyPrecision + 1, RoundingMode.HALF_EVEN);
			decimal myMulti = (decimal)myMultiplier;
            //myMulti.setScale(privatemyPrecision + 1, RoundingMode.HALF_EVEN);
            decimal myDivide = (decimal)myDivider;
            //myDivide.setScale(privatemyPrecision + 1, RoundingMode.HALF_EVEN);
            //decimal myPower10 = (decimal)(10 ^ 7);

            // 2014-12-05 EIO - try doing all calculations at 6 precision
            //decimal useNum = Math.Truncate(aNumThisUnits * myPower10) / (myPower10);
            decimal useNum = KonFuncs.setPrecision(aNumThisUnits, 7);
			//decimal useNum = aNumThisUnits.setScale(7, RoundingMode.HALF_EVEN); // should already be 7 but just in case

			// won't get here if doing time - handled inline code in KonVertUnitTime
			//if (KonFuncs.isGroupTime(aGroup)) {
			//	return doKonvertFromSelfTime(aNumThisUnits);
			//}

			// do calculations to return value in base units for aNumThisUnits
			if (myAddFirst)
			{
				try
				{
					//myCalc = aNumThisUnits.add(getmyAdder());
					myCalc = useNum + myAdder;
					myRet = myCalc * myMulti;
					myCalc = myRet / myDivide;
                    //myCalc = Math.Truncate(myCalc * myPower10) / myPower10;
                    myCalc = KonFuncs.setPrecision(myCalc, 7);
				}
				catch (Exception)
				{
					//Exception newErr = err;
					return -42m;
				}
			}
			else
			{
				try
				{
					//myCalc = aNumThisUnits.multiply(myMulti);
					myCalc = useNum * myMulti;
                    //myRet = myCalc.divide(myDivide, 7, RoundingMode.HALF_EVEN);
                    myRet = myCalc / myDivide;
                    myCalc = myRet + myAdder;
                    myCalc = KonFuncs.setPrecision(myCalc, 7);
				}
				catch (Exception)
				{
					//Exception newErr = err;
					return -42m;
				}
			}
			//return myCalc.setScale(privatemyPrecision, RoundingMode.HALF_EVEN);
			return myCalc;
		}

		// Convert To a number of this units from a number of base units for whatever set of base units this params object represents
		public virtual decimal doKonvertToSelf(decimal aNumBaseUnits, KonVersionGroup aGroup)
		{
			decimal myCalc = 0m;
			//myCalc.setScale(privatemyPrecision + 1, RoundingMode.HALF_EVEN);
			decimal myRet = 0m;
			//myRet.setScale(privatemyPrecision + 1, RoundingMode.HALF_EVEN);
			decimal myMulti = (decimal)myMultiplier;
			//myMulti.setScale(privatemyPrecision + 1, RoundingMode.HALF_EVEN);
			decimal myDivide = (decimal)myDivider;
			//myDivide.setScale(privatemyPrecision + 1, RoundingMode.HALF_EVEN);

			// won't get here if TIME, handled in inline code in KonVertUnitTime
			//if (KonFuncs.isGroupTime(aGroup)) {
			//	return doKonvertToSelfTime(aNumBaseUnits);
			//}

			// 2014-12-05 EIO - try doing all calculations at 6 precision
			//decimal? useNum = aNumBaseUnits.setScale(7, RoundingMode.HALF_EVEN); // should already be 7 but just in case
            decimal useNum = KonFuncs.setPrecision(aNumBaseUnits, 7);
			// do calculations to return value in this units for aNumBaseUnits
			// if AddFirst is true for FROM then it implies not to add first for TO
			if (myAddFirst)
			{
				try
				{
					myCalc = useNum * myDivide; // for TO, invert Divider and Multiplier
					//myRet = myCalc.divide(myMulti, 7, RoundingMode.HALF_EVEN);
                    myRet = myCalc / myMulti;
					myCalc = myRet - myAdder;
                    myCalc = KonFuncs.setPrecision(myCalc, 7);
				}
				catch (Exception)
				{
					//Exception newErr = err;
					return -42m;
				}
			}
			else
			{
				try
				{
					//myCalc = aNumBaseUnits.subtract(getmyAdder());
					myCalc = useNum - myAdder;
					myRet = myCalc * myDivide;
					myCalc = myRet / myMulti;
                    myCalc = KonFuncs.setPrecision(myCalc, 7);
				}
				catch (Exception)
				{
					//Exception newErr = err;
					return -42m;
				}
			}
			//return myCalc.setScale(privatemyPrecision, RoundingMode.HALF_EVEN);
			return myCalc;
		}

        // 2017-11-16 EIO handle JSON automatically
        
	}
}