// Copyright Noetic-29 LLC 2014 - 2018
// All rights reserved

// www.noetic-29.com//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================
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
using System;
using Newtonsoft;
using Newtonsoft.Json;

namespace KonVertObjs
{
	public class KonUserVertUnit : KonObj
	{
        // 2017-12-07 EIO nothing to fix so don't override fixReferences

        public KonUserVertUnit(KonVertSet aSet) : base(aSet) { }

        [JsonConstructor]
        public KonUserVertUnit() : base()
        {
            theSet = null;
        }

        public string myVersionUnitID { get; set; }
/*
		private string privatemyVersionUnitID;
		public string getmyVersionUnitID()
		{
			return privatemyVersionUnitID;
		}
		public void setmyVersionUnitID(string value)
		{
			privatemyVersionUnitID = value;
		}
*/

		// for BigDecimal version, Units must know (have) a precision
        public long myUnitPrecision { get; set; }
/*
		private long privatemyUnitPrecision;
		public long getmyUnitPrecision()
		{
			return privatemyUnitPrecision;
		}
		public void setmyUnitPrecision(long value)
		{
			privatemyUnitPrecision = value;
		}
*/

		// for pretty print, decide whether to use decimal or fractions
        public bool myDoDecimal { get; set; }
/*
		private bool privatemyDoDecimal = false;
		public bool getmyDoDecimal()
		{
			return privatemyDoDecimal;
		}
		public void setmyDoDecimal(bool value)
		{
			privatemyDoDecimal = value;
		}
*/

            // 2017-11-17 EIO JSON automatic and problematic
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public final org.json.simple.JSONObject buildJSONObject()
/*
		public JSONObject buildJSONObject()
		{
			try
			{
				JSONObject myJSONObject = new JSONObject();

				myJSONObject.put("myVersionUnitID", getmyVersionUnitID());
				myJSONObject.put("myUnitPrecision", getmyUnitPrecision());
				myJSONObject.put("myDoDecimal", getmyDoDecimal());

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
				setmyVersionUnitID((string) aJSONObject.get("myVersionUnitID"));
				long myLong = (long?) aJSONObject.get("myUnitPrecision").Value;
				setmyUnitPrecision(myLong);
				bool myFlag = (bool?) aJSONObject.get("myDoDecimal").Value;
				setmyDoDecimal(myFlag);

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