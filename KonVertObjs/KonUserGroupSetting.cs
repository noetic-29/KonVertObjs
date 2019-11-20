// Copyright Noetic-29 LLC 2014 - 2019
// All rights reserved

// www.noetic-29.com
//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
using System.Collections.Generic;
/*
   	using JSONObject = org.json.simple.JSONObject;
	//import java.io.FileNotFoundException;
	//import java.io.FileReader;
	//import java.io.IOException;
	//import java.util.ArrayList;

	//import org.json.simple.JSONObject;
	//import org.json.simple.JSONArray;
	//import org.json.simple.parser.JSONParser;
	//import org.json.simple.parser.ParseException;

	using JsonParseException = com.google.gson.JsonParseException;
	//import java.math.BigDecimal;
*/

namespace KonVertObjs
{
	public class KonUserGroupSetting : KonObj
	{
        public KonUserGroupSetting(KonVertSet aSet) : base(aSet) { }
        public string myVersionGroupID { get; set; }
        public bool myDoPrettyPrint { get; set; }
        public bool myDoDecimal { get; set; }
        public long myUnitPrecision { get; set; }

		// 2015-02-04 EIO - add dynamic update interval in number and fraction of days
		//    but display to user as something better???
		private int _dynamicReadIntervalDays = 3;
		public int DynamicReadIntervalDays
		{
			get
			{
				return _dynamicReadIntervalDays;
			}
			set
			{
				_dynamicReadIntervalDays = value;
			}
		}

        public bool DynamicReadHold { get; set; }
        public DateTime DynamicUpdateDate { get; set; }

        private List<KonUserVertUnit> _myUserVertUnits;
        public List<KonUserVertUnit> myUserVertUnits {
            get {
                if (_myUserVertUnits == null) _myUserVertUnits = new List<KonUserVertUnit>();
                return _myUserVertUnits;
            }
            set {
                _myUserVertUnits = value;
            }
        }

        // 2017-12-07 EIO changed to fix myUserVertUnits
        public override void fixReferences()
        {
            if (myUserVertUnits != null)
            {
                foreach (KonUserVertUnit aKUVU in myUserVertUnits)
                {
                    aKUVU.theSet = theSet;
                    aKUVU.fixReferences();
                }
            }
        }
        public virtual void loadCurrent(KonVersionGroup aGroup)
		{
			if (aGroup != null)
			{
				myVersionGroupID = aGroup.myVersionGroupID;
				myDoPrettyPrint = aGroup.myDoPrettyPrint;
				KonVertUnit aKVU = aGroup.konVertUnits[0]; // read in first unit to get values

				myDoDecimal = aKVU.myDoDecimal;
				myUnitPrecision = aKVU.myUnitPrecision;

				// 2014-02-04 EIO default to every 3 days
				DynamicReadIntervalDays = aGroup.DynamicReadIntervalDays;
				DynamicReadHold = aGroup.DynamicReadHold;
				DynamicUpdateDate = aGroup.DynamicUpdateDate;
			}
		}
		public override string ToString()
		{
			return myVersionGroupID + "-USER";
		}
        public KonUserVertUnit findKonUserVertUnit(String aUserVertUnitID)
        {
            if (myUserVertUnits != null)
            {
                foreach (KonUserVertUnit myKUVU in myUserVertUnits)
                {
                    if (aUserVertUnitID == myKUVU.myVersionUnitID)
                    {
                        return myKUVU;
                    }
                }
            }
            return null;
        }
        public void addKonUserVertUnit(KonUserVertUnit aKUVU)
        {
            foreach (KonUserVertUnit myKUVU in myUserVertUnits)
            {
                if (String.Compare(myKUVU.myVersionUnitID, aKUVU.myVersionUnitID) == 0)     // 2019-06-02 better code
                //if (aKUVU.myVersionUnitID == myKUVU.myVersionUnitID)
                {
                    myUserVertUnits.Remove(aKUVU);
                    break;
                }
            }
            myUserVertUnits.Add(aKUVU);
        }

        // 2017-11-17 EIO JSON automatic but problematic
        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") public final org.json.simple.JSONObject buildJSONObject()
        /*
                public JSONObject buildJSONObject()
                {
                    try
                    {
                        JSONObject myJSONObject = new JSONObject();

                        myJSONObject.put("myVersionGroupID", getmyVersionGroupID());
                        myJSONObject.put("myDoPrettyPrint", getmyDoPrettyPrint());
                        myJSONObject.put("myDoDecimal", getmyDoDecimal());
                        myJSONObject.put("myUnitPrecision", getmyUnitPrecision());
                        string myStr = DynamicReadIntervalDays.ToString();
                        myJSONObject.put("dynamicReadIntervalDays", myStr);
                        myStr = DynamicReadHold.ToString();
                        myJSONObject.put("dynamicReadHold", myStr);
                        myJSONObject.put("dynamicUpdateDate", DynamicUpdateDate);
                        return myJSONObject;
                    }
                    catch (JsonParseException)
                    {
                        return null;
                    }
                }

                public bool loadFromJSONObject(JSONObject aJSONObject)
                {
                    try
                    {
                        setmyVersionGroupID((string) aJSONObject.get("myVersionGroupID"));
                        bool myFlag = (bool?) aJSONObject.get("myDoPrettyPrint").Value;
                        setmyDoPrettyPrint(myFlag);
                        if (aJSONObject.containsKey("myDoDecimal"))
                        {
                            myFlag = (bool?) aJSONObject.get("myDoDecimal").Value;
                            setmyDoDecimal(myFlag);
                        }
                        else
                        {
                            setmyDoDecimal(true);
                        }
                        if (aJSONObject.containsKey("myUnitPrecision"))
                        {
                            long myPrecision = (long?) aJSONObject.get("myUnitPrecision").Value;
                            setmyUnitPrecision(myPrecision);
                        }
                        else
                        {
                            setmyUnitPrecision(3);
                        }

                        if (aJSONObject.containsKey("dynamicReadIntervalDays"))
                        {
                            try
                            {
                                string myStr = (string) aJSONObject.get("dynamicReadIntervalDays");
                                DynamicReadIntervalDays = Convert.ToInt32(myStr);
                            }
                            catch (JsonParseException)
                            {
                                DynamicReadIntervalDays = 3;
                            }
                        }
                        else
                        {
                            DynamicReadIntervalDays = 3;
                        }

                        if (DynamicReadIntervalDays == 0)
                        {
                            DynamicReadIntervalDays = 3;
                        }

                        if (aJSONObject.containsKey("dynamicReadHold"))
                        {
                            try
                            {
                                string myBool = (string) aJSONObject.get("dynamicReadHold");
                                DynamicReadHold = myBool.Equals("true", StringComparison.CurrentCultureIgnoreCase);
                            }
                            catch (JsonParseException)
                            {
                                DynamicReadHold = false;
                            }
                        }
                        else
                        {
                            DynamicReadHold = false;
                        }

                        if (aJSONObject.containsKey("dynamicUpdateDate"))
                        {
                            try
                            {
                                string myStr = (string) aJSONObject.get("dynamicUpdateDate");
                                DynamicUpdateDate = myStr;
                            }
                            catch (JsonParseException)
                            {
                                DynamicUpdateDate = "";
                            }
                        }
                        else
                        {
                            DynamicUpdateDate = "";
                        }
                        return true;
                    }
                    catch (JsonParseException)
                    {
                        return false;
                    }
                }
*/
    }
}
