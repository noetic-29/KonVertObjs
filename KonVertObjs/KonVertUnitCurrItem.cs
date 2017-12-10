// Copyright Noetic-29 LLC 2014 - 2018
// All rights reserved

// www.noetic-29.com//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
/*
	//import java.io.FileNotFoundException;
	//import java.io.FileReader;
	//import java.io.IOException;
	//import java.util.ArrayList;

	using JSONObject = org.json.simple.JSONObject;
	//import org.json.simple.JSONArray;
	//import org.json.simple.parser.JSONParser;
	//import org.json.simple.parser.ParseException;

	//import com.google.gson.JsonParseException;
	//import java.math.BigDecimal;

*/

namespace KonVertObjs
{
	// This OBJECT is used by KonVersionGroupDynamic to get the dynamic information about
	// currency conversion.  IT DOES NOT REPLACE KonVertUnit and all work with the Units and
	// KonVersions are done in the NORMAL fashion (as opposed to KonVertUnitTime).

	public class KonVertUnitCurrItem : Object
	{
		// {"rate":[{"id":"EURUSD","Name":"EUR to USD","Rate":"1.1343","Date":"2/3/2015","Time":"9:48pm","Ask":"1.1344","Bid":"1.1343"}

        public string ID { get; set; }
/*
		private string _id;
		public virtual string ID
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}
*/

        public string Name { get; set; }
/*
		private string _Name;
		public virtual string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name = value;
			}
		}
*/

        public double Rate { get; set; }
/*
		private double? _Rate;
		public virtual double? Rate
		{
			get
			{
				return _Rate;
			}
			set
			{
				_Rate = value;
			}
		}
*/

		private long _Multiplier = 1;
		public long Multiplier
		{
			get
			{
				return _Multiplier;
			}
		}

		private long _Divider = 1;
		public long Divider
		{
			get
			{
				return _Divider;
			}
		}

		public virtual void buildFromCur(KonVertUnit aKVU)
		{
			// 2015-11-24 EIO assuming that inversion of request has already been made
			// from a prior read of the JSON object so don't need to invert ID or name
			ID = aKVU.myVersionUnitID;
			Name = aKVU.myDisplayTextLong;
			long myMulti = aKVU.myInSystemParams.myMultiplier;
			long myDivid = aKVU.myInSystemParams.myDivider;

			double myDMulti = (double) myMulti;
			double myDDivid = (double) myDivid;
			Rate = myDMulti / myDDivid;
			// 2015-11-24 EIO now allowing for inverted requests but converting them to normal (with bigger values)
			// but stored value may have already been converted so just get rate and check for value in calcMultDiv
			calcMultDiv(Rate);
		}

        // 2017-11-16 EIO do JSON automatically
/*
		public virtual bool loadFromJSONObject(JSONObject aUnit)
		{
			// 2015-11-24 EIO Since VND is worth less than 0.0001 dollars, we are getting zero back
			//		so must invert request (in URL to USDVND) and check ID to see which came first
			//		NOTE: must have some way to delete previous versions that had zero
			//setID((String) aUnit.get("id"));
			bool? invertedRequest = false;
			string myStr = (string) aUnit.get("id");
			string first = myStr.Substring(0, 3);
			string second = myStr.Substring(3);
			// 2015-11-26 EIO - switch ALL conversion requests to USD first to get better conversion rate
			if (first.compareToIgnoreCase("USD") == 0)
			{
				// this is an inverted request, change it around but get the values right
				myStr = second + first;
				ID = myStr;

				myStr = second + " to ";
				myStr = myStr + first;
				Name = myStr;

				invertedRequest = true;
			}
			else
			{ // this is a normal request, keep old code
				// 2015-11-26 EIO shouldn't ever get here with new URL request
				ID = myStr;
				Name = (string) aUnit.get("Name");
			}

			//String myStr = (String) aUnit.get("Rate");
			myStr = (string) aUnit.get("Rate");
			double? mydbl = Convert.ToDouble(myStr);
			if (invertedRequest.Value)
			{
				mydbl = 1 / mydbl;
			}
			calcMultDiv(mydbl);
			return true;
		}
*/

		private void calcMultDiv(double aDbl)
		{
			// take the double of the rate and create longs that when divided get the same value
			// from YAHOO, the conversion rate never has more than 4 decimal places, so just multiply 
			// by 10000 for Multi and use 10000 as Divisor
			// 2015-11-24 EIO but since can have to have an invertedrequest that will result in more than 4 decimal places
			// need to calculate a reasonable multiplier/divider
			// NOTE: could possibly always use 100,000,000
			// 2015-11-26 EIO will always use 100,000,000
			//if (aDbl < 0.0001) {
				// just assume can never require more than 8 decimal places sooooo
				_Divider = 100000000;
			//} else {
			//	_Divider = 10000;
			//}
			_Multiplier = (long)(aDbl * _Divider);
		}
	}

}
