// Copyright Noetic-29 LLC 2014 - 2018
// All rights reserved

// www.noetic-29.com//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
using System.Collections.Generic;
/*
//import java.io.InputStream;
//import java.util.ArrayList;

using JSONObject = org.json.simple.JSONObject;
using JSONArray = org.json.simple.JSONArray;
using JSONParser = org.json.simple.parser.JSONParser;
using ParseException = org.json.simple.parser.ParseException;

//import com.google.gson.JsonParseException;
//import java.math.BigDecimal;


//import java.net.*;
//import java.io.BufferedReader;
//import java.io.InputStreamReader;
//import java.util.Calendar;
//import java.util.GregorianCalendar;

//import android.content.BroadcastReceiver;
using Context = android.content.Context;
//import android.content.Intent;
//import android.content.IntentFilter;
//import android.net.ConnectivityManager;
//import android.net.NetworkInfo;
//import android.os.Bundle;

//import java.math.RoundingMode;
*/

namespace KonVertObjs
{
    public class KonVertUnitCurr : KonObj
    {
        public KonVertUnitCurr(KonVertSet aSet) : base(aSet) { }

        //"query":{"count":45,"created":"2015-02-11T01:39:37Z","lang":"en-US","results":{"rate":[{"id":"EURUSD","Name":"EUR to USD","Rate":"1.133","Date":"2/11/2015","Time":"8:39pm","Ask":"1.1332","Bid":"1.1328"},{"id":"JPYUSD","Name":"JPY to USD","Rate":"0.0084","Date":"2/11/2015","Time":"8:39pm","Ask":"0.0084","Bid":"0.0084"}
        public const string NOTIFICATION = "com.noetic_29.konvert.RetrieveDynamic";
        public const string RESULT = "com.noetic_29.konvert.RetrieveDynamicResult";

        private KonFuncs myKonFuncs = new KonFuncs();

        public KonVersionGroup MyGroup {
            get 
            {
                return theSet.getKonVersionGroup(MyGroupID);
            }
        }

        public bool ShouldSerializeMyGroup() { return false; }

        public string MyGroupID { get; set; }

        /*        
                private KonVersionGroup _myGroup;
                public KonVersionGroup MyGroup
                {
                    get
                    {
                        return _myGroup;
                    }
                    set
                    {
                        _myGroup = value;
                    }
                }
        */

        public string DynamicResults { get; set; }
/*
		private string _DynamicResults = "";
		public string DynamicResults
		{
			get
			{
				return _DynamicResults;
			}
			set
			{
				_DynamicResults = value;
			}
		}
*/

		private int _count = 1;
		public int Count
		{
			get
			{
				return _count;
			}
			set
			{
				_count = value;
			}
		}

        public DateTime Created { get; set; }
/*
		private string _created = "";
		public string Created
		{
			get
			{
				return _created;
			}
			set
			{
				_created = value;
			}
		}
*/

		private string _language = "";
		public string Language
		{
			get
			{
				return _language;
			}
			set
			{
				_language = value;
			}
		}

        // 2017-11-16 EIO handle JSON automatically
/*
		private JSONObject _results = null;
		public JSONObject Results
		{
			get
			{
				return _results;
			}
			set
			{
				_results = value;
			}
		}

		private JSONArray _rate = null;
		public JSONArray Rate
		{
			get
			{
				return _rate;
			}
			set
			{
				_rate = value;
			}
		}
*/
/*
		// exceptions from read/Write/etc.
        public Exception anyError { get; set; }
*/
/*
		private Exception privateanyError;
		public Exception getanyError()
		{
			return privateanyError;
		}
		public void setanyError(Exception e)
		{
			privateanyError = e;
		}
*/

		private List<KonVertUnitCurrItem> _rateItems = new List<KonVertUnitCurrItem>();
		public List<KonVertUnitCurrItem> RateItems
		{
			get
			{
				return _rateItems;
			}
			set
			{
					_rateItems = value;
			}
		}

        public KonVertUnitCurrItem getRateItem(string anItemID)
		{
			if (_rateItems.Count > 0)
			{
				foreach (KonVertUnitCurrItem aKVUCI in _rateItems)
				{
                    if (String.Equals(aKVUCI.ID, anItemID, StringComparison.OrdinalIgnoreCase))
					//if (aKVUCI.ID.equalsIgnoreCase(anItemID))
					{
						return aKVUCI;
					}
				}
			}
			return null;
		}

		public virtual void updateCurrUnits()
		{
            bool mustAdd = false;
            foreach  (KonVertUnitCurrItem aKVUCI in _rateItems)
			{
                mustAdd = false;
                KonVertUnit aKVU = MyGroup.getUnit(aKVUCI.ID);
                if (aKVU == null)
                {
                    aKVU = new KonVertUnit(theSet);
                    mustAdd = true;
                    aKVU.GroupID = MyGroupID;
                    aKVU.minNumber = 0;
                    aKVU.myBaseUnitSystem = "USDUSD";
                    aKVU.myCrossSystemParams = null;
                    string aStr = aKVUCI.ID.Substring(0, 3);
                    //aStr = aStr.concat = "$";
                    aKVU.myDisplayTextShort = aStr;
                    aKVU.myDisplayTextShortFront = false;
                    aKVU.myDoDecimal = true;
                    aKVU.myInSystemParams = new KonVertParams(theSet);
                    aKVU.myInSystemParams.myAdder = 0m;
                    aKVU.myInSystemParams.myAddFirst = false;
                    aKVU.myInSystemParams.myPrecision = 2;
                    aKVU.myMinusAllowed = false;
                    aKVU.myNextSmaller = "";
                    aKVU.mySystem = "CURR";
                    aKVU.myToolTip = "";
                    aKVU.myUnitListOrder = 0;
                    aKVU.myUnitPrecision = 2;
                    aKVU.myVersionUnitID = aKVUCI.ID;
                    string myStr = aKVUCI.Name;
                    aKVU.myDisplayTextLong = myStr.Substring(0, 3);
                    aKVU.myDisplayTextLongPlural = myStr.Substring(0, 3);
                }
                aKVU.myInSystemParams.myMultiplier = aKVUCI.Multiplier;
                aKVU.myInSystemParams.myDivider = aKVUCI.Divider;

                if (mustAdd)
				{
                    List<KonVertUnit> myKonVertUnits = MyGroup.konVertUnits;
					myKonVertUnits.Add(aKVU);
				}
			}

			// now update data in Group Dynamic info (last update date)
			MyGroup.DynamicUpdateDate = Created;
			MyGroup.GroupDynamics.UpdateDate = Created;
		}

		public virtual bool startLoadDynamicFromSource(string aGroupID, string aDynType, string aDynSource)//, Context aContext)
		{
            // TODO - handle all IO
            /*
                        try
                        {
                            KonFuncs.RetrieveDynamicTask myTask = new KonFuncs.RetrieveDynamicTask(myKonFuncs, aGroupID, aDynType, _DynamicResults, aContext);
                            myTask.execute(aDynSource);
                            return true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                            Console.Write(e.StackTrace);
                            return false;
                        }
            */
            return false;
		}

        // 2017-11-16 EIO handle all JSON parsing automatically
        /*
                public virtual JSONObject loadDynamicFromLast(string aDynLastFile, Context aContext)
                {
                    try
                    {
                        setanyError(null);
                        JSONParser parser = new JSONParser();

                         System.IO.FileStream fIn = aContext.openFileInput(aDynLastFile);
                        System.IO.StreamReader isr = new System.IO.StreamReader(fIn.FD);

                        //Object obj = parser.parse(new FileReader(aFileName));
                        object obj = parser.parse(isr);

                        JSONObject jsonObject = (JSONObject) obj;
                        return jsonObject;
                    }
                    catch (FileNotFoundException e)
                    {
                        Console.WriteLine(e.ToString());
                        Console.Write(e.StackTrace);
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.ToString());
                        Console.Write(e.StackTrace);
                    }
                    catch (ParseException e)
                    {
                        Console.WriteLine(e.ToString());
                        Console.Write(e.StackTrace);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        Console.Write(e.StackTrace);
                    }
                    return null;
                }

                public virtual bool loadFromJSONObject(JSONObject aJSONObject)
                {
                    long myInt = 0;
                    JSONObject queryResult = null;
                    string myStr = "";
                    if (aJSONObject != null)
                    {
                        if (!aJSONObject.containsKey("query"))
                        {
                            return false;
                        }

                        queryResult = (JSONObject) aJSONObject.get("query");
                        if (queryResult.containsKey("count"))
                        {
                            myInt = (long?) queryResult.get("count").Value;
                            Count = (int) myInt;
                        }
                        if (queryResult.containsKey("created"))
                        {
                            myStr = (string)queryResult.get("created");
                            Created = myStr;
                        }
                        if (queryResult.containsKey("lang"))
                        {
                            myStr = (string)queryResult.get("lang");
                            Language = myStr;
                        }
                        if (queryResult.containsKey("results"))
                        {
                            Results = (JSONObject) queryResult.get("results");
                        }
                        else
                        { // if not results then can't do anything
                            return false;
                        }
                        if (Results.containsKey("rate"))
                        {
                            Rate = (JSONArray) Results.get("rate");
                            JSONArray myJA = Rate;
                            for (int i = 0; i < myJA.size(); i++)
                            {
                                JSONObject aUnit = (JSONObject) myJA.get(i);
                                KonVertUnitCurrItem myKVUCI = new KonVertUnitCurrItem();
                                myKVUCI.loadFromJSONObject(aUnit);
                                _rateItems.Add(myKVUCI);
                            }
                            return true;
                        }
                        else
                        { // can't do anything if don't have "rate"
                            return false;
                        }
                    }
                    return false;
                }

        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") public org.json.simple.JSONObject buildJSONObject()
                public virtual JSONObject buildJSONObject()
                {
                    JSONArray myRates = new JSONArray();
                    JSONObject myResults = new JSONObject();
                    JSONObject myJO = new JSONObject();
                    JSONObject myQuery = new JSONObject(); // return from YAHOO is a Query object

                    foreach (KonVertUnit aKVU in MyGroup.getkonVertUnits())
                    {
                        KonVertUnitCurrItem myKVUCI = new KonVertUnitCurrItem();
                        myKVUCI.buildFromCur(aKVU);
                        myRates.add(myKVUCI);
                    }

                    myResults.put("rate", myRates);

                    myJO.put("count", (long) myRates.size());
                    myJO.put("created", Created);
                    myJO.put("lang", Language);
                    myJO.put("results", myResults);

                    myQuery.put("query", myJO);
                    return myQuery;
                }

                public bool writeJSONFile(KonVersionGroup aKVG, string aDynLastFile, Context aContext)
                {
                    JSONObject myJSONObject;

                    myJSONObject = buildJSONObject();
                    try
                    { // catches IOException below
                        // ##### Write a file to the disk #####
                        // * We have to use the openFileOutput()-method 
                        // * the ActivityContext provides, to
                        // * protect your file from others and 
                        // * This is done for security-reasons. 
                        // * We chose MODE_WORLD_READABLE, because
                        // *  we have nothing to hide in our file		
                        //FileOutputStream fOut = new FileOutputStream(aFileName);
                        System.IO.FileStream fOut = aContext.openFileOutput(aDynLastFile, Context.MODE_PRIVATE);
                        System.IO.StreamWriter osw = new System.IO.StreamWriter(fOut);

                        // Write the string to the file
                        osw.BaseStream.WriteByte(myJSONObject.toJSONString());
                        //* ensure that everything is 
                        // * really written out and close 
                        osw.Flush();
                        osw.Close();
                        return true;
                    }
                    catch (Exception e)
                    {
                        setanyError(e);
                        //throw;
                        return false;
                    }
                    catch (IOException ei)
                    {
                        setanyError(ei);
                        //throw;
                        return false;
                    }
                }
        */

    }
}