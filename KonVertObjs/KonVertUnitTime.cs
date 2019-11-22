// Copyright Noetic-29 LLC 2014 - 2019
// All rights reserved

// www.noetic-29.com
//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================
using System;
using System.Numerics;

namespace KonVertObjs
{
    // 2019-11-21 NOTE: KonVertUnitTime should probably be derived from KonVerUnit but that would mean that all of KonVertUnit's
    //     methods would have to be overrideable.  Instead I chose in the past to have KonVertUnitTime only contain one element, the 
    //     KonVertUnit, and still implemented all of the functions of KonVertUnit but doing them for time.  Not sure this was a wise choice

    // 2011-11-21  It's even worse than I thought.  KonVertUnitTime should be related to KonVertUnit but is, instead, related to KonVersion
    //     Should probably be called KonVersionTime.  and should inherit from KonVersion.  So can't use this to help build window and display 
    //     DST.  OH WELL

    // 2019-11-21 But as of this date, I'm using addl (from KonObj) to get the text for the DST prompt.  Means will have to create new apk
    //     each year for changes in start/stop dates.  Not the best solution

	public class KonVertUnitTime : KonObj
	{
        public KonVersion konVersion { get; set; }
/*
		private KonVersion _KonVersion;
		public KonVersion KonVersion
		{
			get
			{
				return _KonVersion;
			}
			set
			{
				_KonVersion = value;
			}
		}
*/

		// creator
		public KonVertUnitTime(KonVersion aKonVersion)
		{
			konVersion = aKonVersion;
		}

		public KonVertUnitTime()
		{
			konVersion = null;
		}

        public KonVertUnitTime(KonVertSet aSet) : base(aSet) { }

        public KonVertUnitTime(KonVertSet aSet, KonVersion aKonv) : base(aSet)
        {
            konVersion = aKonv;
        }

		public virtual bool performKonversionTime(bool doLeftRight)
		{
			decimal result = decimal.Zero;
			decimal minNum = decimal.Zero;
			decimal adder = decimal.Zero;
			decimal inNum = decimal.Zero;
			decimal myI100 = 100m;
			//BigDecimal myI24 = BigDecimal.valueOf(2400);

			//int addDays = 0;
			//timeResults myTimeResults = new timeResults();

			if (konVersion == null)
			{
				return false;
			}

			konVersion.myLeft2Right = doLeftRight;
			konVersion.AddDays = 0;
			if (doLeftRight)
			{
				// 2014-11-25 EIO add check for number less than allowed in any case
				minNum = (decimal)konVersion.LeftUnit.minNumber;
				if (konVersion.myLeftNumber <minNum)
				{
					return false;
				}

				adder = konVersion.LeftUnit.myInSystemParams.myAdder;
				inNum = konVersion.myLeftNumber;
				// 2015-06-22 EIO to handle DST, if DST checked then adjust back to Standard time for conversion to UTC
				//			convert to standard time by subtracting 1 hr which is 100 in 24 hour time
				//   		check to be sure not negative, if so, convert to time before midnight
				if (konVersion.myLeftDST)
				{
					inNum = inNum - myI100;
					inNum = fixTime(inNum);
				}
				result = doKonvertFromSelfTime(inNum, adder);
				// result will be a 24 hour time expressed as BigDecimal with NO decimal values
				// BUT it may be less than 0000 or more than 2400 since it could be on another day
				// so need to handle day cross over.  NOTE: 2400 between Tuesday and Wednesday is Tuesday
				// 2015-03-16 EIO fixDays replaced by fixTime in doKonvertFromSelfTime
				//myTimeResults.setTimeValue(result);
				//myTimeResults.setAddDays(0);
				//fixDays(myTimeResults);
				//addDays = myTimeResults.getAddDays();
				//result = myTimeResults.getTimeValue();

				// now complete the Konversion
				adder = konVersion.RightUnit.myInSystemParams.myAdder;
				inNum = result;
				result = doKonvertToSelfTime(inNum, adder);
				// 2015-06-22 EIO to handle DST, if DST checked then adjust forward to DST time from UTC
				//			convert to DST time by adding 1 hr which is 100 in 24 hour time
				//   		check to be sure not above 2400, if so, convert to time after midnight
				if (konVersion.myRightDST)
				{
					result = result + myI100;
					result = fixTime(result);
				}
				// 2015-03-16 EIO fixDays replaced by fixTime in doKonvertFromSelfTime
				//myTimeResults.setTimeValue(result);
				//myTimeResults.setAddDays(0);
				//fixDays(myTimeResults);
				//result = myTimeResults.getTimeValue();
				//addDays = addDays + myTimeResults.getAddDays();  // will either change back to 0 or keep at 1 or -1,
																 // 'SHOULD' never result in -2 or 2
				//setAddDays(addDays);
				konVersion.myRightNumber = result;
			}
			else
			{
				// 2014-11-25 EIO add check for number less than allowed in any case
				minNum = (decimal)konVersion.RightUnit.minNumber;
				if (konVersion.myRightNumber < minNum)
				{
					return false;
				}

				adder = konVersion.RightUnit.myInSystemParams.myAdder;
				inNum = konVersion.myRightNumber;
				// 2015-06-22 EIO to handle DST, if DST checked then adjust back to Standard time for conversion to UTC
				//			convert to standard time by subtracting 1 hr which is 100 in 24 hour time
				//   		check to be sure not negative, if so, convert to time before midnight
				if (konVersion.myRightDST)
				{
					inNum = inNum - myI100;
					inNum = fixTime(inNum);
				}
				result = doKonvertFromSelfTime(inNum, adder);

				// now complete the Konversion
				adder = konVersion.LeftUnit.myInSystemParams.myAdder;
				inNum = result;
				result = doKonvertToSelfTime(inNum, adder);
				// 2015-06-22 EIO to handle DST, if DST checked then adjust forward to DST time from UTC
				//			convert to DST time by adding 1 hr which is 100 in 24 hour time
				//   		check to be sure not above 2400, if so, convert to time after midnight
				if (konVersion.myLeftDST)
				{
					result = result + myI100;
					result = fixTime(result);
				}
				konVersion.myLeftNumber = result;
			}
			return true;

		}

		// for TIME conversion, hold text for 'next day' in DYnamicLastFile,
		//                      hold text for 'prior day' in DynamicSourceLoc
		public virtual string addDayText(string myStr)
		{
			string tmpStr = "";
			if (konVersion.AddDays > 0)
			{
				tmpStr = konVersion.Group.GroupDynamics.DynamicLastFile; // next day
			}
			else if (konVersion.AddDays < 0)
			{
				tmpStr = konVersion.Group.GroupDynamics.DynamicSourceLoc; // prior day
			}
			if (konVersion.AddDays != 0)
			{
				myStr = myStr + " ";
			}
			return myStr + tmpStr;
		}

		public string val2StrTime(KonVertUnit aUnit, decimal aNum)
		{
			string myStr = aUnit.prettyVal2Str(aNum, false);
			if (aUnit.myVersionUnitID.Equals(konVersion.LeftUnit.myVersionUnitID))
			{
				// getting left unit, if left to right then just return string
				if (konVersion.myLeft2Right)
				{
					return myStr;
				}
				else
				{ // otherwise check to add/subtract days to time
					return addDayText(myStr);
				}
			}
			else
			{
				if (!konVersion.myLeft2Right)
				{
					return myStr;
				}
				else
				{
					return addDayText(myStr);
				}
			}
		}

		public virtual string toStringTime()
		{
			// 2014-12-05 EIO - avoid setting to sub-unit if sub-unit is what we're converting to
			string leftValStr = null;
			string rightValStr = null;
			bool hldPrettyPrint = konVersion.Group.myDoPrettyPrint;
			if (konVersion.myLeft2Right)
			{
                konVersion.Group.myDoPrettyPrint = false;
				leftValStr = konVersion.val2Str(konVersion.LeftUnit, konVersion.myLeftNumber);
                konVersion.Group.myDoPrettyPrint = hldPrettyPrint;
				rightValStr = konVersion.val2Str(konVersion.RightUnit, konVersion.myRightNumber);
				return konVersion.Group.myDisplayTextShort + ": " + leftValStr + " = " + rightValStr;
			}
			else
			{
                konVersion.Group.myDoPrettyPrint = false;
				rightValStr = konVersion.val2Str(konVersion.RightUnit, konVersion.myRightNumber);
                konVersion.Group.myDoPrettyPrint = hldPrettyPrint;
				leftValStr = konVersion.val2Str(konVersion.LeftUnit, konVersion.myLeftNumber);
				return konVersion.Group.myDisplayTextShort + ": " + rightValStr + " = " + leftValStr;
			}
		}

		// 	// 2015-02-28 EIO for TIME conversion, only use ADDER but also need to return whether in same DAY or not
		public virtual decimal doKonvertFromSelfTime(decimal aNumThisUnits, decimal anAdder)
		{
            // aNumThisUnits is a 24 hour clock time expressed as an integer.
            // 10:15am is 1015,  3:30am is 0330 and 9:42pm is 2142
            // Adder, however, is a decimal, usually integer but could be .5, .25 or .75
            // and cannot be 'added' arithmetically, must make part of hour
            return doKonvertToFromSelfTime(aNumThisUnits, anAdder, false);
/*
			try
			{
                BigInteger[] myInBreakDown = breakTime(aNumThisUnits);

                decimal[] myAddBreakDown = breakAddr(anAdder);

                BigInteger myAddHrs = (BigInteger)myAddBreakDown[0];
                // get number of minutes represented by this decimal remainder 
                BigInteger myAddMins = (BigInteger)(myAddBreakDown[1] * 60);

				myInBreakDown[0] = myInBreakDown[0] + myAddHrs;
				myInBreakDown[1] = myInBreakDown[1] + myAddMins;
				// fix time in internal routine so can addDays
				fixTime(myInBreakDown);

                BigInteger intReturn = myInBreakDown[0] * 100 + myInBreakDown[1];
				decimal decReturn = (decimal)intReturn;
				return decReturn;
			}
			catch (Exception)
			{
				return decimal.Zero;
			}
*/
		}

        private decimal doKonvertToFromSelfTime(decimal aNumBaseUnits, decimal anAdder, bool isTo)
        {
            // aNumThisUnits is a 24 hour clock time expressed as an integer.
            // 10:15am is 1015,  3:30am is 0330 and 9:42pm is 2142
            // Adder, however, is a decimal, usually integer but could be .5, .25 or .75
            // and cannot be 'added' arithmetically, must make part of hour
            try
            {
                BigInteger[] myInBreakDown = breakTime(aNumBaseUnits);

                // divide by 1 leaving integer hours in myAddBreakDown[0] and a fraction of hours in myBreakDown[1]
                decimal[] myAddBreakDown = breakAddr(anAdder);

                BigInteger myAddHrs = (BigInteger)myAddBreakDown[0];
                BigInteger myAddMins = (BigInteger)(myAddBreakDown[1] * 60);

                BigInteger direction = 1;
                if (isTo) direction = -1;       // To self subtracts time ??

                myInBreakDown[0] = myInBreakDown[0] + direction * myAddHrs;
                myInBreakDown[1] = myInBreakDown[1] + direction * myAddMins;
                myInBreakDown = fixTime(myInBreakDown);

                // KLAXON KLAXON - Does not take into account days
                BigInteger intReturn = myInBreakDown[0] * 100 + myInBreakDown[1];
                decimal decReturn = (decimal)intReturn;
                return decReturn;
            }
            catch (Exception)
            {
                return decimal.Zero;
            }
        }

        public virtual decimal doKonvertToSelfTime(decimal aNumBaseUnits, decimal anAdder)
		{
            // aNumThisUnits is a 24 hour clock time expressed as an integer.
            // 10:15am is 1015,  3:30am is 0330 and 9:42pm is 2142
            // Adder, however, is a decimal, usually integer but could be .5, .25 or .75
            // and cannot be 'added' arithmetically, must make part of hour
            return doKonvertToFromSelfTime(aNumBaseUnits, anAdder, true);
/*
			try
			{
                BigInteger[] myInBreakDown = breakTime(aNumBaseUnits);

                // divide by 1 leaving integer hours in myAddBreakDown[0] and a fraction of hours in myBreakDown[1]
                decimal[] myAddBreakDown = breakAddr(anAdder);

				BigInteger myAddHrs = (BigInteger)myAddBreakDown[0];
                BigInteger myAddMins = (BigInteger)(myAddBreakDown[1] * 60);

				myInBreakDown[0] = myInBreakDown[0] - myAddHrs;
				myInBreakDown[1] = myInBreakDown[1] - myAddMins;
				fixTime(myInBreakDown);

				// KLAXON KLAXON - Does not take into account days
                BigInteger intReturn = myInBreakDown[0] * 100 + myInBreakDown[1];
                decimal decReturn = (decimal)intReturn;
                return decReturn;
            }
            catch (Exception)
			{
				return decimal.Zero;
			}
*/
		}

        // 2017-11-18 EIO THere are THREE fixTime functions (2 private in KonVertUnitTime and 1 public in KonFunc)
        //   The first here takes a decimal 24hr time, converts to BigInteger Hour/Minute and calls the second
        //   The second here takes a BigInteger array[3] of hour/minute/addDay and makes sure it is a legal 24hr time
        //      This fixTime calls the public KonFuncs fixTime that actually checks and adjusts the hour/minute and
        //         in the process it indicates (in array[2]) whether to add or subtract a day
        //      Finally, before returning, it modifies the konVersion addDays by whatever was returned in array[2]
        //   All calls to fixTime in this module pass through the fixTime(BigInteger[]) function so all calls modify
        //      the addDays field
		private decimal fixTime(decimal aTime)
		{
            // divide by 100, leaving integer hours in myBreakDown[0], num of minutes in myBreakDown[1]
            BigInteger[] myInBreakDown = breakTime(aTime);
			myInBreakDown = fixTime(myInBreakDown);
			BigInteger intReturn = myInBreakDown[0] * 100 + myInBreakDown[1];
			decimal decReturn = (decimal)intReturn;
			return decReturn;
		}

		private BigInteger[] fixTime(BigInteger[] aTime)
		{
            BigInteger[] newTime;
            newTime = KonFuncs.fixTime(aTime);
            konVersion.AddDays += (int)newTime[2];
            return newTime;
		}

        private static BigInteger[] breakTime(decimal a24HrTime)
        {
            BigInteger myI100 = 100;
            BigInteger my24HrTime = (BigInteger)a24HrTime;
            BigInteger[] myInBreakDown = new BigInteger[3];
            // divide by 100, leaving integer hours in myBreakDown[0], num of minutes in myBreakDown[1]
            myInBreakDown[0] = BigInteger.Divide(my24HrTime, myI100);
            myInBreakDown[1] = BigInteger.Remainder(my24HrTime, myI100);
            return myInBreakDown;
        }

        private static decimal[] breakAddr(decimal anAdder)
        {
            decimal[] myAddBreakDown = new decimal[2];
            // divide by 1 leaving integer hours in myAddBreakDown[0] and a fraction of hours in myBreakDown[1]
            myAddBreakDown[0] = KonFuncs.setPrecision(anAdder, 0);          // get integer portion
            myAddBreakDown[1] = anAdder - myAddBreakDown[0];        // get remainder
            return myAddBreakDown;
        }

        public static decimal checkTime(decimal value)
        {
            if (value < 0)
            {
                value = decimal.Zero;
            }
            else if (value > 2400)
            {
                value = 2400;
            }
            return value;
        }

        public static string getmyNumberDisplay(string myStr)
        {
            if (myStr.Length < 4)
            {
                myStr = "0000" + myStr;
                myStr = myStr.Substring(myStr.Length - 4);
            }
            return myStr;
        }

#if EDREM
        // 2011-11-21 add DST prompt using KonObj addl
        public string DSTPrompt {
            get {
                return addl;
            }
        }
#endif
    }
}