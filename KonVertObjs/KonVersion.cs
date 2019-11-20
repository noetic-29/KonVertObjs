// Copyright Noetic-29 LLC 2014 - 2019
// All rights reserved
// www.noetic-29.com
//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

//import java.text.DecimalFormat;
//import java.text.NumberFormat;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using System.Net;

namespace KonVertObjs
{
    // attributes of a conversion performed by the user
    public class KonVersion : KonObj
    {
        // NOTE: Only the 'ID's and the InputNumber and Result are stored in the XML version of this object
        //      When the XML ID is read, the objects are filled in (implies these entries must be at end, or fill in later)
        // Actual KonVersionGroup object used for this KonVersion
        // 2017-11-16 EIO Java only had 'get', not 'set'
        //private KonVersionGroup _Group;
        /*
                private KonVersionGroup myVersionGroup; // must be set by calling/owning object so units can be loaded
                public KonVersionGroup getmyVersionGroup()
                {
                    return myVersionGroup;
                }
        */
        /*
                public override bool fixReferences(KonObj obj)
                {
                    Group = (KonVersionGroup)obj;
                    if (Group != null)
                    {
                        myVertUnitLeft = Group.getUnit(myVertUnitLeftID);
                        myVertUnitLeft.Group = Group;
                        myVertUnitRight = Group.getUnit(myVertUnitRightID);
                        myVertUnitRight.Group = Group;
                    }
                    return true;
                }
        */
        public KonVersionGroup Group {
            get {
                if (!String.IsNullOrEmpty(myVersionGroupID) && theSet != null)
                {
                    return theSet.getKonVersionGroup(myVersionGroupID);
                }
                return null;
            }
        }

        // 2017-12-06 EIO Tell JSON not to serialize Group so doen's recurse
        public bool ShouldSerializeGroup() { return false; }

        public KonVertUnit LeftUnit {
            get {
                if (!String.IsNullOrEmpty(myVertUnitLeftID) && !String.IsNullOrEmpty(myVersionGroupID) && theSet != null)
                {
                    KonVersionGroup myGroup = theSet.getKonVersionGroup(myVersionGroupID);
                    if (myGroup != null)
                    {
                        KonVertUnit retUnit = myGroup.getUnit(myVertUnitLeftID);
                        if (retUnit != null)
                            return retUnit;
                    }
                }
                return null;
            }
        }

        public bool ShouldSerializeLeftUnit() { return false; }

        public KonVertUnit RightUnit {
            get {
                if (!String.IsNullOrEmpty(myVertUnitRightID) && !String.IsNullOrEmpty(myVersionGroupID) && theSet != null)
                {
                    KonVersionGroup myGroup = theSet.getKonVersionGroup(myVersionGroupID);
                    if (myGroup != null)
                    {
                        KonVertUnit retUnit = myGroup.getUnit(myVertUnitRightID);
                        if (retUnit != null)
                            return retUnit;
                    }
                }
                return null;
            }
        }

        public bool ShouldSerializeRightUnit() { return false; }

       // Unique language independent ID of the conversion Group for this conversion
        public string myVersionGroupID { get; set; }

        // Unique language independent ID of the left side conversion units for this conversion
        // 2017-11-16 EIO expanded set to include setting myVertUnitLeft from ID
        //private string privatemyVertUnitLeftID;
        public string myVertUnitLeftID { get; set; } 

		// Unique language independent ID of the right side conversion units for this conversion
		//private string privatemyVertUnitRightID;
        public string myVertUnitRightID { get; set; }

		// number converted
		private decimal privatemyLeftNumber;
		public decimal myLeftNumber
		{
            get {
                privatemyLeftNumber = KonFuncs.setPrecision(privatemyLeftNumber, 7);
                //privatemyLeftNumber = privatemyLeftNumber.setScale(7, RoundingMode.HALF_EVEN);
                return privatemyLeftNumber;
            }
            set {
                if (Group != null)
                {
                    if (KonFuncs.isGroupTime(Group))
                    {
                        value = KonVertUnitTime.checkTime(value);
                    }
                }
                privatemyLeftNumber = KonFuncs.setPrecision(value, 7);
            }
        }
        public string myLeftNumberDisplay
		{
            get {
                if (LeftUnit != null)
                {
                    // 2015-03-03 EIO make sure time has 4 digits (leading zeros)
                    string myStr = KonFuncs.setPrecision(myLeftNumber, (int)LeftUnit.myUnitPrecision).ToString();
                    //string myStr = privatemyLeftNumber.setScale((int)myVertUnitLeft.getmyUnitPrecision(), RoundingMode.HALF_EVEN).toPlainString();
                    if (KonFuncs.isGroupTime(Group))
                    {
                        return KonVertUnitTime.getmyNumberDisplay(myStr);
                    }
                    return myStr;
                    //return privatemyLeftNumber.setScale((int)myVertUnitLeft.getmyUnitPrecision(), RoundingMode.HALF_EVEN).toPlainString();
                }
                else
                {
                    return privatemyLeftNumber.ToString();
                }
            }
        }
        public bool myLeftDST { get; set; }

        private decimal privatemyRightNumber;
		public decimal myRightNumber
		{
            get {
                privatemyRightNumber = KonFuncs.setPrecision(privatemyRightNumber, 7);
                return privatemyRightNumber;
                //privatemyRightNumber = privatemyRightNumber.setScale(7, RoundingMode.HALF_EVEN);
                //return privatemyRightNumber;
            }
            set {
                if (Group != null)
                {
                    if (KonFuncs.isGroupTime(Group))
                    {
                        value = KonVertUnitTime.checkTime(value);
                    }
                }
                privatemyRightNumber = KonFuncs.setPrecision(value, 7);
            }
        }
        public string myRightNumberDisplay
		{
            get {
                if (RightUnit != null)
                {
                    // 2015-03-03 EIO make sure time has 4 digits (leading zeros)
                    string myStr = KonFuncs.setPrecision(privatemyRightNumber, (int)RightUnit.myUnitPrecision).ToString();
                    //string myStr = privatemyRightNumber.setScale((int)myVertUnitRight.getmyUnitPrecision(), RoundingMode.HALF_EVEN).toPlainString();
                    if (KonFuncs.isGroupTime(Group))
                    {
                        return KonVertUnitTime.getmyNumberDisplay(myStr);
                    }
                    return myStr;
                    //return privatemyRightNumber.setScale((int)myVertUnitRight.getmyUnitPrecision(), RoundingMode.HALF_EVEN).toPlainString();
                }
                else
                {
                    return KonFuncs.setPrecision(myRightNumber, 7).ToString(); ;
                }
            }
		}

        public bool myRightDST { get; set; }

        public bool myLeft2Right { get; set; }

		// used for TIME conversion to account for over a day between times
        public int AddDays { get; set; }
        // sets all non-KonVertObjs values which causes other KonVertObjs values to be set
        public KonVersion Equal {
            set {
                this.myVertUnitLeftID = value.myVertUnitLeftID;
                this.myVertUnitRightID = value.myVertUnitRightID;
                //this.Group = value.Group;
                this.myLeft2Right = value.myLeft2Right;
                this.myLeftNumber = value.myLeftNumber;
                this.myRightNumber = value.myRightNumber;
                this.AddDays = value.AddDays;
                this.myRightDST = value.myRightDST;
                this.myLeftDST = value.myLeftDST;
                this.myVersionGroupID = value.myVersionGroupID;
            }
        }


        // CONSTRUCTORS
        public KonVersion(string aVersionGroupID, KonVertSet aSet) : base(aSet) 
		{
			if (aVersionGroupID != null)
			{
				myVersionGroupID = aVersionGroupID;
			}
		}

		public KonVersion(KonVertSet aSet) : base(aSet) { }

        //NJ[JsonConstructor]
        public KonVersion() : base()
        {
            theSet = null;
        }

		// METHODS
		// After Safety Check, perform conversion, placing result in appropriate left/right number
		public virtual bool performKonversion(bool doLeftRight)
		{
			if (safetyCheck() == false)
			{
				return false;
			}

			if (KonFuncs.isGroupTime(Group))
			{
				KonVertUnitTime myKVUT = new KonVertUnitTime(theSet, this);
				return myKVUT.performKonversionTime(doLeftRight);
			}
			myLeft2Right = doLeftRight;
			decimal result = 0m;
			if (doLeftRight)
			{
				// 2014-11-25 EIO add check for number less than allowed in any case
				result = (decimal)LeftUnit.minNumber;
				if (privatemyLeftNumber < result)
				{
					return false;
				}

				result = LeftUnit.doKonvertFromSelf(myLeftNumber, RightUnit.mySystem);
				myRightNumber = RightUnit.doKonvertToSelf(result, LeftUnit.mySystem);
				if (myRightNumber < (decimal)RightUnit.minNumber)
				{
					return false;
				}
			}
			else
			{
				// 2014-11-25 EIO add check for number less than allowed in any case
				//result = (decimal)myVertUnitRight.minNumber;
				if (privatemyRightNumber < (decimal)RightUnit.minNumber)
				{
					return false;
				}

				result = RightUnit.doKonvertFromSelf(myRightNumber, LeftUnit.mySystem);
				myLeftNumber = LeftUnit.doKonvertToSelf(result, RightUnit.mySystem);
				if (myLeftNumber < (decimal)LeftUnit.minNumber)
				{
						return false;
				}
			}
			return true;
		}

        // 2017-12-08 EIO - fixReferences might want ot fix VertUnitLeft/Right but should not, let the Group that owns them do it

        // displayable string for this conversion e.g. VOL:1gal = 3.8L
		public override string ToString()
		{
			if (!safetyCheck())
			{
				return "UNSAFE";
			}

			if (KonFuncs.isGroupTime(Group))
			{
				KonVertUnitTime myKVUT = new KonVertUnitTime(theSet, this);
				return myKVUT.toStringTime();
			}
			// 2014-12-05 EIO - avoid setting to sub-unit if sub-unit is what we're converting to
			string leftValStr = null;
			string rightValStr = null;
			bool hldPrettyPrint = Group.myDoPrettyPrint;
			if (myLeft2Right)
			{
                Group.myDoPrettyPrint = false;
				leftValStr = val2Str(LeftUnit, myLeftNumber);
                Group.myDoPrettyPrint = hldPrettyPrint;
				rightValStr = val2Str(RightUnit, myRightNumber);
				return Group.myDisplayTextShort + ": " + leftValStr + " = " + rightValStr;
			}
			else
			{
                Group.myDoPrettyPrint = false;
				rightValStr = val2Str(RightUnit, myRightNumber);
                Group.myDoPrettyPrint = hldPrettyPrint;
				leftValStr = val2Str(LeftUnit, myLeftNumber);
				return Group.myDisplayTextShort + ": " + rightValStr + " = " + leftValStr;
			}
		}

		public virtual string val2Str(KonVertUnit aUnit, decimal aNum)
		{
			if (!safetyCheck())
			{
				return "UNSAFE";
			}
			if (KonFuncs.isGroupTime(Group))
			{
				KonVertUnitTime myKVUT = new KonVertUnitTime(theSet, this);
				return myKVUT.val2StrTime(aUnit, aNum);
			}
			return aUnit.prettyVal2Str(aNum, false);
		}

		public string val2Str(bool doLeft, decimal aNum)
		{
			if (doLeft)
			{
				return val2Str(LeftUnit, aNum);
			}
			else
			{
				return val2Str(RightUnit, aNum);
			}
		}

		public string val2Str(bool doLeft)
		{
			if (doLeft)
			{
				return val2Str(LeftUnit, myLeftNumber);
			}
			else
			{
				return val2Str(RightUnit, myRightNumber);
			}
		}

		// invoked to place this KonVersion into it's group as the last KonVersion
		public void makeLastKonversion()
		{
			if (Group != null)
			{
                Group.myLastKonVersion = this;
			}
		}

		// check to see no null objects before creating string.  Attempt to load objects if possible
		protected internal virtual bool safetyCheck()
		{
			if (Group == null)
			{
				return false;
			}

			if (RightUnit == null)
			{
				return false;
			}

			if (LeftUnit == null)
			{
				return false;
			}
			return true;
		}

        // sets all non-KonVertObjs values which causes other KonVertObjs values to be set
        // 2019-11-19 This creates a write only property that copies values from an existing Konversion into this 
        //     in other cases I use setEqualTo which, for some reason, I prefer
        // changed to setEqualTo 2019-11-20
        public void setEqualTo(KonVersion aKonVersion)
        {
            this.myVertUnitLeftID = aKonVersion.myVertUnitLeftID;
            this.myVertUnitRightID = aKonVersion.myVertUnitRightID;
            //this.Group = value.Group;
            this.myLeft2Right = aKonVersion.myLeft2Right;
            this.myLeftNumber = aKonVersion.myLeftNumber;
            this.myRightNumber = aKonVersion.myRightNumber;
            this.AddDays = aKonVersion.AddDays;
            this.myRightDST = aKonVersion.myRightDST;
            this.myLeftDST = aKonVersion.myLeftDST;
            this.myVersionGroupID = aKonVersion.myVersionGroupID;
        }

        // call this function to clear groups out of a KonVersion before writing to the KonVertUserParams file
        public void prepareToWrite()
		{
			//this.Group = null;
			//this.myVertUnitLeft = null;
			//this.myVertUnitRight = null;
		}

        // Build a new KonVersion that is a value copy of the IDs in this KonVersion
        // 2019-11-20 this should be abrogated in favor of setEqualTo which ensures new instances (so changing source doesn't change copy)
        public KonVersion buildCopy()
        {
            KonVersion aKonversion = new KonVersion(theSet);
            aKonversion.myLeftNumber = myLeftNumber;
            aKonversion.myRightNumber = myRightNumber;
            aKonversion.myVersionGroupID = myVersionGroupID;
            aKonversion.myVertUnitLeftID = myVertUnitLeftID;
            aKonversion.myVertUnitRightID = myVertUnitRightID;
            aKonversion.myLeft2Right = myLeft2Right;
            aKonversion.AddDays = AddDays;
            aKonversion.myLeftDST = myLeftDST;
            aKonversion.myRightDST = myRightDST;

            return aKonversion;
        }

        // resulting KonVersion is new space but with same values
        public virtual KonVersion buildCopyFull()
        {
            KonVersion aKonversion = new KonVersion(theSet);

            aKonversion.myVersionGroupID = myVersionGroupID;
            //aKonversion.Group = _Group;

            aKonversion.myVertUnitLeftID = myVertUnitLeftID; // sets VertUnitLeft if VersionGroup is not null
            aKonversion.myVertUnitRightID = myVertUnitRightID; // ditto for Right
            aKonversion.myLeft2Right = myLeft2Right;

            aKonversion.myLeftNumber = myLeftNumber;
            aKonversion.myRightNumber = myRightNumber;
            aKonversion.AddDays = AddDays;
            aKonversion.myRightDST = myRightDST;
            aKonversion.myLeftDST = myLeftDST;
            aKonversion.myVersionGroupID = myVersionGroupID;
            return aKonversion;
        }

    }
}