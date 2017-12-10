//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
using System.Globalization;
/*
using JSONObject = org.json.simple.JSONObject;
using JSONParser = org.json.simple.parser.JSONParser;
using ParseException = org.json.simple.parser.ParseException;

//import java.math.MathContext;
//import java.math.RoundingMode;

using JsonParseException = com.google.gson.JsonParseException;
*/

namespace KonVertObjs
{
	// Base object that contains information about one of the conversion units
	public class KonVertUnit : KonObj
	{
        public KonVertUnit(KonVertSet aSet) : base(aSet) { }

        public override void fixReferences()
        {
            if (myInSystemParams != null)
            {
                myInSystemParams.theSet = theSet;
                myInSystemParams.fixReferences();
            }

            if (myCrossSystemParams != null)
            {
                myCrossSystemParams.theSet = theSet;
                myCrossSystemParams.fixReferences();
            }
        }

        public void fixReferences(string aGroupID)
        {
            GroupID = aGroupID;
            fixReferences();
        }

        //PROPERTIES
        // Unique language independent ID of the conversion Unit
        public string myVersionUnitID { get; set; }

		// Units to display on selection list
        public string myDisplayTextLong { get; set; }

		// Units to display on selection list when more than 1
        public string myDisplayTextLongPlural { get; set; }

		// Units to display on results window (may be the same)
        public string myDisplayTextShort { get; set; }

		// Are units displayed before (True) or after (False) TexstShort on results window
        public bool myDisplayTextShortFront { get; set; }

		// Descriptive text for help/tooltip
        public string myToolTip { get; set; }

		// whether or not it makes sense to allow a value less than 0
        public bool myMinusAllowed { get; set; }

		// temperature is only current conversion that allows negative but it, too, has a limit at 0 K
		// so include a number that is the (negative but necessarily) limit for this unit
		// for negative not allowed, it's zero
        public double minNumber { get; set; }

        // System to which this unit belongs - US for ft/in/Tbsp, Metric for metric.  Any others??
        public string mySystem { get; set; }

        // next smaller unit for doing pretty display
        public string myNextSmaller { get; set; }

        // Order to display in Unit lists
        public int myUnitListOrder { get; set; }

        // The system that the base unit lives in, if it's my system then only need InSystemParams
        public string myBaseUnitSystem { get; set; }

		// for BigDecimal version, Units must know (have) a precision
        public long myUnitPrecision { get; set; }

		// for pretty print, decide whether to use decimal or fractions
        public bool myDoDecimal { get; set; }

		// KonVertParams to us when converting between two units in the same system
        public KonVertParams myInSystemParams { get; set; }

		// KonVertParams to use when converting between systems via a base Unit
        public KonVertParams myCrossSystemParams { get; set; }

/*        // 2017-12-07 EIO implement fixReferences to get Group right
        public override bool fixReferences(KonObj obj)
        {
            Group = (KonVersionGroup) obj;
            return true;
        }
*/

        // 2017-11-16 EIO original JAVA did not have a 'get'
        //private KonVersionGroup _Group;
        public KonVersionGroup Group {
            get {
                // 2017-12-06 EIO Would love to fetch the group in _GroupID is not null
                // but don't know where to get it from at this point
                // on the other hand, when a Group reads in it's list of KonVertUnits, it 
                // should be able to place itself into Group, so maybe _GroupID is not necessary
                // keep it for now

                if (GroupID != null)
                {
                    return theSet.getKonVersionGroup(GroupID);
                }
                return null;
                //return _Group;
            }
/*
            set {
                _Group = value;
                if (_Group != null) _GroupID = _Group.myVersionGroupID;
                else _GroupID = "";
            }
*/
        }

        public bool ShouldSerializeGroup() {
            return false;
        }

        // 2017-12-06 EIO ShouldSerialize tells NewtonSoft JSON not to create group
        //    but need to remember/know GroupID to load in group later??
        //
        //private string _GroupID;
        public string GroupID { get; set; }

		//METHODS
		// Function that calculates conversion to base units where base units are different between in system and cross system conversions
		public decimal doKonvertFromSelf(decimal aNumThisUnits, string aToSystem)
		{ // 2014-12-05 EIO as of now, all numbers are kept (in the KonVersion) in precision 7.  DO NOT CONVERT PRECISION until creating string
			if (mySystem.Equals(aToSystem, StringComparison.CurrentCultureIgnoreCase) || mySystem.Equals(myBaseUnitSystem, StringComparison.CurrentCultureIgnoreCase))
			{
				return myInSystemParams.doKonvertFromSelf(aNumThisUnits, Group);
			}
			else
			{
				return myCrossSystemParams.doKonvertFromSelf(aNumThisUnits, Group);
			}
		}

		// Function that calculates conversion from base units where base units are different between in system and cross system conversions
		public decimal doKonvertToSelf(decimal aNumBaseUnits, string aFromSystem)
		{
			if (mySystem.Equals(aFromSystem, StringComparison.CurrentCultureIgnoreCase) || mySystem.Equals(myBaseUnitSystem, StringComparison.CurrentCultureIgnoreCase))
			{
				return myInSystemParams.doKonvertToSelf(aNumBaseUnits, Group);
			}
			else
			{
				return myCrossSystemParams.doKonvertToSelf(aNumBaseUnits, Group);
			}
		}

		// display values as sub units of top unit (pretty)
		public string prettyVal2Str(decimal aNum, bool isFinal)
		{
			KonVertUnit mySmaller;
			decimal myDecInt = 0m;
			decimal myRemain = 0m;
			string myIntStr;
			string myRemainStr;
			long hldUnitPrecision;

			if (aNum == 0m)
			{
				return val2Str(aNum);
			}

			if (Group == null)
			{
				return val2Str(aNum);
			}
			if (Group.myDoPrettyPrint == false)
			{
				return val2Str(aNum);
			}

			// if nothing smaller (or not defined) just return normal val2Str
			if (myNextSmaller == null || myNextSmaller == "")
			{
				return val2Str(aNum);
			}

			mySmaller = Group.getUnit(myNextSmaller);
			if (mySmaller == null)
			{
				return val2Str(aNum);
			}

            // Calculate part values before checking for isFinal
            myDecInt = Math.Truncate(aNum);
            myRemain = aNum - myDecInt;
/*
			myDecInt = new decimal(aNum.toBigInteger());
			myRemain = aNum.subtract(myDecInt);
*/
			// to avoid long strings of irrelevant measures, specify that this is final so return val2Str
			if (isFinal)
			{
				// in the case where myDecInt is ZERO, actually go to next lower unit (to display myRemain as something more meaningful)
				if (myDecInt != 0m)
				{
					return val2Str(aNum);
				}
			}

			// OK, finally have a smaller unit so lets get the integer part of this number and then pass the remainder on to the smaller unit
			//if (myDecInt.compareTo(BigDecimal.ZERO) != 0) {
				hldUnitPrecision = myUnitPrecision;
				myUnitPrecision = 0;
				myIntStr = val2Str(myDecInt) + " ";
				myUnitPrecision = hldUnitPrecision;
			//} else {
				//myIntStr = "";
			//}

			if (myRemain != 0m)
			{
				KonVersion remainKonVersion = new KonVersion(theSet);
				remainKonVersion.myVertUnitLeftID = myVersionUnitID;
				remainKonVersion.myVertUnitRightID = myNextSmaller;
				remainKonVersion.myVersionGroupID = GroupID;
				remainKonVersion.myLeftNumber = myRemain;
				remainKonVersion.performKonversion(true);

				myRemainStr = mySmaller.prettyVal2Str(remainKonVersion.myRightNumber, true);
			}
			else
			{
				myRemainStr = "";
			}

			return myIntStr + myRemainStr;
		}

		public string val2Str(decimal aNum)
		{
			decimal myIntDec;
			decimal myRemain;
            //decimal myPower10;
			string myFract;
			string myFull;

            // 2017-11-17 EIO need to establish unit precision for Decimal
            // first attempt, multiply aNum by 10 to the myUnitPrecision power
            // truncate and then divide by same power of 10
            // NOTE: truncate does NOT do rounding, is this a problem????
            //myPower10 = 10 ^ myUnitPrecision;
            //decimal useNum = Math.Truncate(aNum * myPower10) / myPower10;
            decimal useNum = KonFuncs.setPrecision(aNum, (int)myUnitPrecision);
			//decimal useNum = aNum.setScale((int)myUnitPrecision, RoundingMode.HALF_EVEN);
			if (useNum == 0m)
			{
				return displayAValue("0");
			}

			if (myDoDecimal == true)
			{
				return displayAValue(useNum.ToString());
			}
			else
			{
				myIntDec = Math.Truncate(useNum);
				myRemain = useNum - myIntDec;
				myFract = KonFuncs.toFraction(myRemain);
				if (myFract.Equals("ONE"))
				{
					myIntDec = myIntDec + 1m;
					myFract = "";
				}

				if (myIntDec == 0m)
				{ // so don't have leading 0
					if (myFract.CompareTo("") == 0) // but if both are null then display 0
					{
						myFull = "0";
					}
					else
					{
						myFull = myFract;
					}
				}
				else
				{
					myFull = myIntDec.ToString();
					if (myFract != "")
					{
						myFull = myFull + " " + myFract;
					}
				}
				return displayAValue(myFull);
			}
		}

		public string displayAValue(string aNumStr)
		{
			// 2014-11-07 EIO strip off trailing 00s in decimal
			if (myDoDecimal)
			{
                NumberFormatInfo currentCultureNumbers = CultureInfo.CurrentCulture.NumberFormat;

                string curSepChar = currentCultureNumbers.CurrencyDecimalSeparator;
                string numSepChar = currentCultureNumbers.NumberDecimalSeparator;

/*
				char[] sep = new char[] {(char)0};
				string sepChar = ".";
				NumberFormat format = DecimalFormat.Instance;

				if (format is DecimalFormat)
				{
					DecimalFormatSymbols symbols = ((DecimalFormat)format).DecimalFormatSymbols;
					sep[0] = symbols.DecimalSeparator;
					sepChar = new string(sep);
				}
*/
				if (KonFuncs.isGroupCurrency(Group))
				{
					if (aNumStr.Contains(curSepChar))
					{
						for (int j = aNumStr.Length - 1; j >= 0; j--)
						{
							if (aNumStr[j] == curSepChar[0])
							{
								// found decimal character, do I have sufficient length left
								int decLen = aNumStr.Length - j - 1; // don't count sep
								if (decLen < (int) myUnitPrecision)
								{
									aNumStr = aNumStr + "0000000".Substring(0, (int) myUnitPrecision - decLen);
								}
								break;
							}
						}
					}
					else
					{
						aNumStr = aNumStr + curSepChar;
						aNumStr = aNumStr + "0000000".Substring(0, (int) myUnitPrecision);
					}
				}
				else
				{
					if (aNumStr.Contains(numSepChar))
					{
						int len = aNumStr.Length;
						int zeroCtr = 0;
						for (int j = len - 1; j >= 0; j--)
						{
							if (aNumStr[j] == "0"[0])
							{
								zeroCtr++; // found a trailing 0, continue looking
							}
							else if (aNumStr[j] == numSepChar[0])
							{
								zeroCtr++; // found the decimal separator, must be all trailing zeros
								break;
							}
							else
							{ // found a non-zero decimal number before decimal separator
								break;
							}
						}
						if (zeroCtr > 0)
						{
							string newStr = aNumStr.Substring(0, len - zeroCtr);
							aNumStr = newStr;
						}
					}
				}
			}

			// 2015-03-03 EIO make sure time has 4 digits (leading zeros)
			if (KonFuncs.isGroupTime(Group))
			{
				if (aNumStr.Length < 4)
				{
					aNumStr = "0000" + aNumStr;
					aNumStr = aNumStr.Substring(aNumStr.Length - 4);
				}
			}

			if (myDisplayTextShortFront)
			{
				return myDisplayTextShort + " " + aNumStr;
			}
			else
			{
				return aNumStr + " " +myDisplayTextShort;
			}
		}

		public override string ToString()
		{
			return myDisplayTextLong;
		}

        // 2017-11-16 EIO will handle JSON using NewSoftJSON tools, not hand written

		public virtual void checkStrings()
		{
			myDisplayTextLong = KonFuncs.checkString(myDisplayTextLong);
			myDisplayTextLongPlural = KonFuncs.checkString(myDisplayTextLongPlural);
			myDisplayTextShort = KonFuncs.checkString(myDisplayTextShort);
			myToolTip = KonFuncs.checkString(myToolTip);
		}

        public bool setEqual(KonVertUnit aKonVertUnit)
        {
            anyError = null;
            try
            {
                this.myBaseUnitSystem = aKonVertUnit.myBaseUnitSystem;
                this.myCrossSystemParams = aKonVertUnit.myCrossSystemParams;
                this.myDisplayTextLong = aKonVertUnit.myDisplayTextLong;
                this.myDisplayTextLongPlural = aKonVertUnit.myDisplayTextLongPlural;
                this.myDisplayTextShort = aKonVertUnit.myDisplayTextShort;
                this.myDisplayTextShortFront = aKonVertUnit.myDisplayTextShortFront;
                this.myMinusAllowed = aKonVertUnit.myMinusAllowed;
                this.myInSystemParams = aKonVertUnit.myInSystemParams;
                this.mySystem = aKonVertUnit.mySystem;
                this.myNextSmaller = aKonVertUnit.myNextSmaller;
                this.myToolTip = aKonVertUnit.myToolTip;
                this.myUnitListOrder = aKonVertUnit.myUnitListOrder;
                this.myVersionUnitID = aKonVertUnit.myVersionUnitID;
                this.myUnitPrecision = aKonVertUnit.myUnitPrecision;
                this.myDoDecimal = aKonVertUnit.myDoDecimal;
                this.minNumber = aKonVertUnit.minNumber;

                this.GroupID = aKonVertUnit.GroupID;
                return true;
            }
            catch (Exception e)
            {
                anyError = e;
                //throw;
                return false;
            }
        }
    }
}