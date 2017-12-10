//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
/*
using JSONObject = org.json.simple.JSONObject;
using JSONArray = org.json.simple.JSONArray;
using JSONParser = org.json.simple.parser.JSONParser;
using ParseException = org.json.simple.parser.ParseException;


using JsonParseException = com.google.gson.JsonParseException;
//import java.math.MathContext;
//import java.math.RoundingMode;
*/

namespace KonVertObjs
{
    // All the Units that can be converted from/to for this type of conversion
    public class KonVersionGroup : KonObj
    {
        public KonVersionGroup(KonVertSet aSet) : base(aSet) { }

        //INTERNAL VARIABLES
        private List<KonVertUnit> myVertUnits;
        private List<string> myAppApplys;

        //PROPERTIES
        // Unique language independent ID of this KonVersionGroup
        public string myVersionGroupID { get; set; }
        
        // Display text for type of conversion, e.g. distance, weight, etc.
        public string myDisplayText { get; set; }
        
        // Short (2 Char?) string to display type of conversion on buttons/tiles
        public string myDisplayTextShort { get; set; }
        
        // Descriptive text for help/tooltip
        public string myToolTip { get; set; }
        
        // order for this group to appear in the group list
        public int myGroupListOrder { get; set; }
        
        // 2015-03-03 EIO add prompt for entry
        public string ValueEntryPrompt { get; set; }

	    // 2015-04-06 EIO add unit entry prompt
        public string UnitEntryPrompt { get; set; }

		// default units to display on left selection
        public string myDefaultUnitLeftID { get; set; }

		// default units to displsy on right selection
        public string myDefaultUnitRightID { get; set; }

		// base unit to convert all others to and from
        public string myBaseUnitID { get; set; }

		// maximum number of significant digits
        public int myMaxDigits { get; set; }

		// number of decimal places
        public int myDecimals { get; set; }
		
        // print with sub-units
        public bool myDoPrettyPrint { get; set; }

        // 2017-11-16 EIO To have a default of 3 must expand the get/set
		private int _DynamicReadIntervalDays = 3;
		public int DynamicReadIntervalDays
		{
			get
			{
				return _DynamicReadIntervalDays;
			}
			set
			{
				_DynamicReadIntervalDays = value;
			}
		}

		//private bool _DynamicReadHold = false;
		public bool DynamicReadHold { get; set; }
        
        // last conversion of this Set
        // 2017-11-16 EIO to return the empty string instead of null must expand get/set
        // changes to DateTime
        public DateTime DynamicUpdateDate { get; set; }

       public KonVersion myLastKonVersion { get; set; }

        public KonVersionGroupDynamic GroupDynamics { get; set; }

		// list of units to display on the left of selections
        // 2017-11-16 EIO to build new list if null, must expand get/set
		public List<KonVertUnit> konVertUnits
		{
            get 
            {
                if (myVertUnits == null)
                {
                    myVertUnits = new List<KonVertUnit>();
                }
                return myVertUnits;
            }
            set
            {
                if (myVertUnits != null)
                {
                    myVertUnits = null;
                }
                myVertUnits = value;
            }
        }

        public override void fixReferences()
        {
            if (myLastKonVersion != null)
            {
                myLastKonVersion.theSet = theSet;
                myLastKonVersion.fixReferences();
            }

            if (GroupDynamics != null)
            {
                GroupDynamics.theSet = theSet;
                GroupDynamics.fixReferences(myVersionGroupID);
            }

            if (konVertUnits != null)
            {
                foreach(KonVertUnit aKVU in konVertUnits)
                {
                    aKVU.theSet = theSet;
                    aKVU.fixReferences(myVersionGroupID);
                }
            }
        }

        public List<string> AppApplys
        {
            get {
                if (myAppApplys == null)
                {
                    myAppApplys = new List<string>();
                    //myAppApplys.Add("free");      // 2017-12-06 EIO Don't want a group to always apply to free
                }
                return myAppApplys;
            }
            set
            {
                myAppApplys = value;
            }
        }

        // METHODS
        // 2017-12-06 EIO add remove a unit from list of units to handle initial build of files
        public bool removeUnit(KonVertUnit aKonVertUnit)
        {
            if (myVertUnits != null)
            {
                return myVertUnits.Remove(aKonVertUnit);
            }
            return false;
        }

        public bool containsUnit(KonVertUnit aKonVertUnit)
        {
            if (myVertUnits != null)
            {
                return myVertUnits.Contains(aKonVertUnit);
            }
            return false;
        }

        // check list of available KonVersions (free vs paid)
        public bool doesAppApply(string anAppType)
        {
            if (myAppApplys == null)
            {
                return ((anAppType.ToLower()).CompareTo("free") == 0);
                //return (anAppType.compareToIgnoreCase("free") == 0);
            }
            else
            {
                foreach (string aStr in myAppApplys)
                {
                    if ((aStr.ToLower()).CompareTo(anAppType) == 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        // Display the results page/window based on to from and value
        // Not sure this belongs here, should be part of View
        public bool doDisplayResults(double aNum, KonVertUnit aVertUnitLeft, KonVertUnit aVertUnitRight, bool aDirection)
		{
			return true;
		}

		// convert a value given the KonVertUnit Objects
		public decimal doConversion(decimal aNum, KonVertUnit aVertUnitLeft, KonVertUnit aVertUnitRight, bool aDirection)
		{
		// aDirection True means aNum is in Left units, convert to right units, if false, aNum is in Right units, convert to Left units
			decimal result = 0m;

			if (aVertUnitLeft == null || aVertUnitRight == null)
			{
				return 0m;
			}

			if (aVertUnitLeft == aVertUnitRight)
			{
				return aNum;
			}

			if (aDirection)
			{
				result = aVertUnitLeft.doKonvertFromSelf(aNum, aVertUnitRight.mySystem);
				return aVertUnitRight.doKonvertToSelf(result, aVertUnitLeft.mySystem);
			}
			else
			{
				result = aVertUnitRight.doKonvertFromSelf(aNum, aVertUnitLeft.mySystem);
				return aVertUnitLeft.doKonvertToSelf(result, aVertUnitRight.mySystem);
			}
		}

		// convert a value given the KonVertUnit IDs
		public decimal doConversion(decimal aNum, string aVertUnitLeftID, string aVertUnitRightID, bool aDirection)
		{
		// aDirection True means aNum is in Left units, convert to right units, if false, aNum is in Right units, convert to Left units

			KonVertUnit myVertUnitLeft = getUnit(aVertUnitLeftID);
			KonVertUnit myVertUnitRight = getUnit(aVertUnitRightID);
			return doConversion(aNum, myVertUnitLeft, myVertUnitRight, aDirection);
		}

		// find a konVersionUnit in the list of conversion units
		public KonVertUnit getUnit(string anID)
		{
			if (myVertUnits != null)
			{
				foreach (KonVertUnit aKonVertUnit in myVertUnits)
				{
					if (aKonVertUnit.myVersionUnitID.Equals(anID))
					{
						aKonVertUnit.GroupID = this.myVersionGroupID;
						return aKonVertUnit;
					}
				}
			}
			return null;
		}

		public virtual void checkStrings()
		{
			myDisplayText = KonFuncs.checkString(myDisplayText);
			myDisplayTextShort = KonFuncs.checkString(myDisplayTextShort);
			myToolTip = KonFuncs.checkString(myToolTip);
			foreach (KonVertUnit aKVU in myVertUnits)
			{
				aKVU.checkStrings();
			}
		}

        // 2017-11-16 EIO JSON seems to create json from existing object without explicit functions
        
        public bool setEqual(KonVersionGroup aKonVersionGroup)
        {
            anyError = null;
            try
            {
                this.konVertUnits = aKonVersionGroup.konVertUnits;
                this.myBaseUnitID = aKonVersionGroup.myBaseUnitID;
                this.myDecimals = aKonVersionGroup.myDecimals;
                this.myDoPrettyPrint = aKonVersionGroup.myDoPrettyPrint;
                this.myDefaultUnitLeftID = aKonVersionGroup.myDefaultUnitLeftID;
                this.myDefaultUnitRightID = aKonVersionGroup.myDefaultUnitRightID;
                this.myDisplayText = aKonVersionGroup.myDisplayText;
                this.myDisplayTextShort = aKonVersionGroup.myDisplayTextShort;
                this.myLastKonVersion = aKonVersionGroup.myLastKonVersion;
                this.myMaxDigits = aKonVersionGroup.myMaxDigits;
                this.myToolTip = aKonVersionGroup.myToolTip;
                this.myGroupListOrder = aKonVersionGroup.myGroupListOrder;
                this.myVersionGroupID = aKonVersionGroup.myVersionGroupID;
                this.myVertUnits = aKonVersionGroup.myVertUnits;

                //fixReferences(this);
                return true;
            }
            catch (Exception e)
            {
                anyError = e;
                //throw;
                return false;
            }
        }

        public override string ToString()
		{
			//return myVersionGroupID;
			return myDisplayText;
		}

		public virtual void doSortUnit(List<KonVertUnit> aWrkList, bool ascending)
		{
			bool didSwap = true;
			bool isCurrency = KonFuncs.isGroupCurrency(this);
			bool doSwap = false;

			while (didSwap)
			{
				didSwap = false;
				for (int i = 0; i < aWrkList.Count; ++i)
				{
					if (i + 1 >= aWrkList.Count)
					{
						break;
					}
					if (ascending)
					{
						if (isCurrency)
						{
							// compareTo, negative int if string 1 is 'less than' string 2, 0 if equal, positive int if greater
							doSwap = (aWrkList[i + 1].myDisplayTextLong.CompareTo(aWrkList[i].myDisplayTextLong) < 0);
						}
						else
						{
							doSwap = aWrkList[i + 1].myUnitListOrder < aWrkList[i].myUnitListOrder;
						}
						if (doSwap)
						{
							KonVertUnit tmp = aWrkList[i + 1];
							aWrkList.Remove(tmp);
							aWrkList.Insert(i, tmp);
							didSwap = true;
						}
					}
					else
					{
						if (isCurrency)
						{
							// compareTo, negative int if string 1 is 'less than' string 2, 0 if equal, positive int if greater
							doSwap = (aWrkList[i + 1].myDisplayTextLong.CompareTo(aWrkList[i].myDisplayTextLong) > 0);
						}
						else
						{
							doSwap = aWrkList[i + 1].myUnitListOrder > aWrkList[i].myUnitListOrder;
						}
						if (doSwap)
						{
							KonVertUnit tmp = aWrkList[i + 1];
							aWrkList.Remove(tmp);
							aWrkList.Insert(i, tmp);
							didSwap = true;
						}
					}
				}
			}
		}
	}
}