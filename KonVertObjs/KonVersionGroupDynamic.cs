// Copyright Noetic-29 LLC 2014 - 2019
// All rights reserved

// www.noetic-29.com
//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
/*
//import java.io.FileInputStream;
//import java.io.FileNotFoundException;
//import java.io.FileOutputStream;
//import java.io.FileReader;
//import java.io.IOException;
//import java.io.OutputStreamWriter;

//import org.json.simple.JSONArray;
using JSONObject = org.json.simple.JSONObject;
using JSONParser = org.json.simple.parser.JSONParser;
using ParseException = org.json.simple.parser.ParseException;

//import java.net.*;
//import java.io.BufferedReader;
//import java.io.InputStreamReader;
//import java.util.Calendar;
//import java.util.GregorianCalendar;

using Context = android.content.Context;
//import android.net.ConnectivityManager;
//import android.net.NetworkInfo;

using JsonParseException = com.google.gson.JsonParseException;

//import java.util.Locale;
using DateTime = org.joda.time.DateTime;
//import org.joda.time.Duration;
//import org.joda.time.Instant;
*/

namespace KonVertObjs
{

    //public class KonVersionGroupDynamic extends KonVersionGroup {
    public class KonVersionGroupDynamic : KonObj
    {
        // 2019-11-20 only creator
        public KonVersionGroupDynamic(KonVertSet aSet) : base(aSet) { }
        // actual group that this dynamic data belongs to

        public KonVersionGroup Group {
            get {
                if (theSet != null && GroupID != null && GroupID != "") 
                    return theSet.getKonVersionGroup(GroupID);      // could still return null
                return null;
            }
        }

        // 2017-12-08 EIO no KonObj to fix, so return
        //    but have a GroupID that must be set
        public void fixReferences(string aGroupID)
        {
            GroupID = aGroupID;
            fixReferences();        // 2019-11-20 calls the base (KonObj) fixReferences
        }

        // 2017-12-07 EIO Tell JSON not to serialize Group so doen's recurse
        public bool ShouldSerializeGroup() { return false; }

        public void setEqualTo(KonVersionGroupDynamic aKVGDyn)
        {
            this.GroupID = aKVGDyn.GroupID;
            this.DynamicType = aKVGDyn.DynamicType;
            this.DynamicSourceLoc = aKVGDyn.DynamicSourceLoc;
            this.DynamicLastFile = aKVGDyn.DynamicLastFile;
            this.SourceType = aKVGDyn.SourceType;
            this.UpdateDate = aKVGDyn.UpdateDate;

        }

        public virtual string GroupID { get; set; }

        private string _DynamicType = ""; // e.g. CURRENCY, TIME
        public virtual string DynamicType {
            get {
                return _DynamicType;
            }
            set {
                _DynamicType = value;
            }
        }

        private string _DynamicSourceLoc = ""; // path to data desired on "SourceType"
        public virtual string DynamicSourceLoc {
            get {
                return _DynamicSourceLoc;
            }
            set {
                _DynamicSourceLoc = value;
            }
        }

        private string _DynamicLastFile = ""; // file containing last read in dynamic data
        public string DynamicLastFile {
            get {
                return _DynamicLastFile;
            }
            set {
                _DynamicLastFile = value;
            }
        }

        private string _sourceType = ""; // where to get the dynamic data, most often INTERNET
        public virtual string SourceType {
            get {
                return _sourceType;
            }
            set {
                _sourceType = value;
            }
        }

        private DateTime _updateDate = new DateTime(1999,1,1,1,0,0); // should be DATE object - date of last read from dynamic data
        public virtual DateTime UpdateDate {
            get {
                return _updateDate;
            }
            set {
                _updateDate = value;
            }
        }

        // 2019-11-20 changed from Time to isItTime to be more clear
        public virtual bool isItTime {
            get {
                // compare time of last update to time between updates and determine if time to try again
                //DateTime current = new DateTime();
                if (DynamicGroupHold)
                { // if true then user chose to HOLD dynamic update
                    return false;
                }
                DateTime last = UpdateDate;

                //int numDays = Group.DynamicReadIntervalDays;
                DateTime next;
                if (Group != null)
                    next = last.AddDays(Group.DynamicReadIntervalDays);
                else
                    next = last.AddDays(3);

                return (next.CompareTo(DateTime.Now) < 0);
                //return last.plusDays(numDays).BeforeNow;
            }
        }

        private bool DynamicGroupHold {
            get {
                // TOODO Auto-generated method stub
                if (Group != null) return Group.DynamicReadHold;
                return false;
            }
        }

#if EDREM
        // UNUSED
        private object _DynamicObject = null;
        public object DynamicObject {
            get {
                return _DynamicObject;
            }
            set {
                _DynamicObject = value;
            }
        }
#endif
        // 2017-11-16 EIO do JSON automatically
        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") public final org.json.simple.JSONObject buildJSONObject()
        /*
                public JSONObject buildJSONObject()
                {
                    try
                    {
                        JSONObject myJSONObject = new JSONObject();

                        myJSONObject.put("DynamicType", DynamicType);
                        myJSONObject.put("DynamicSourceLoc", DynamicSourceLoc);
                        myJSONObject.put("DynamicLastFile", DynamicLastFile);
                        myJSONObject.put("SourceType", SourceType);
                        myJSONObject.put("UpdateDate", UpdateDate);
                        return myJSONObject;
                    }
                    catch (JsonParseException)
                    {
                        return null;
                    }
                }

                public virtual bool loadFromJSONObject(JSONObject aJSONObject, KonVersionGroup aGroup)
                {
                    try
                    {
                        Group = aGroup;
                        DynamicType = (string) aJSONObject.get("DynamicType");
                        DynamicSourceLoc = (string) aJSONObject.get("DynamicSourceLoc");
                        DynamicLastFile = (string) aJSONObject.get("DynamicLastFile");
                        SourceType = (string) aJSONObject.get("SourceType");
                        UpdateDate = (string) aJSONObject.get("UpdateDate");
                        aGroup.DynamicUpdateDate = UpdateDate;
                        return true;
                    }
                    catch (JsonParseException)
                    {
                        return false;
                    }
                }
        */

        public virtual bool loadDynamicDataFromLast(Object aContext)
		{
            // TODO - handle currency load
            /*
                        if (DynamicType.Equals("CURRENCY", StringComparison.CurrentCultureIgnoreCase))
                        {  
                            KonVertUnitCurr myKVUC = new KonVertUnitCurr();
                            myKVUC.MyGroup = Group;
                            JSONObject myJO = myKVUC.loadDynamicFromLast(DynamicLastFile, aContext);
                            if (myJO != null)
                            {
                                if (myKVUC.loadFromJSONObject(myJO))
                                {
                                    myKVUC.updateCurrUnits(); // adds information into Group
                                    return true;
                                }
                            }
                        }
                        else if (DynamicType.Equals("SOMETHINGELSE", StringComparison.CurrentCultureIgnoreCase))
                        {
                            // do what's right for some other dynamic data
                        }
            */
            return false;
		}

		public virtual bool downloadDynamicData()//Context aContext)
		{
            // TODO - must handle loading from Internet
/*
			// first check that can generically get to source
			if (SourceType.Equals("INTERNET", StringComparison.CurrentCultureIgnoreCase))
			{
				KonFuncs mKF = new KonFuncs();
				if (!mKF.haveNetworkConnection(aContext))
				{
					return false;
				}
			}
			else if (SourceType.Equals("SOMETHINGELSE", StringComparison.CurrentCultureIgnoreCase))
			{
				// check to see that the required underlying source is available
				// return false if NOT
			}

			// now handle specific types of dynamic data
			if (DynamicType.Equals("CURRENCY", StringComparison.CurrentCultureIgnoreCase))
			{
				KonVertUnitCurr myKVUC = new KonVertUnitCurr();
				bool result = myKVUC.startLoadDynamicFromSource(GroupID, DynamicType, DynamicSourceLoc, aContext);
				return result;
			}
			else if (DynamicType.Equals("SOMETHINGELSE", StringComparison.CurrentCultureIgnoreCase))
			{
				// do what's right for other dynamic data
			}
*/
			return false;
		}

		public virtual bool applyDynamicData(string result)//, Context aContext)
		{
            // TODO - must figure this out for writing dynamic data (e.g. Currency)
/*
			if (DynamicType.Equals("CURRENCY", StringComparison.CurrentCultureIgnoreCase))
			{
				JSONParser parser = new JSONParser();
				object obj;
				if (!string.ReferenceEquals(result, null))
				{
					try
					{
						obj = parser.parse(result);
						JSONObject jsonObject = (JSONObject) obj;

						KonVertUnitCurr myKVUC = new KonVertUnitCurr();
						if (myKVUC.loadFromJSONObject(jsonObject))
						{
							myKVUC.MyGroup = Group;
							myKVUC.updateCurrUnits();
						}
						return writeLastUpdate(result, aContext);
					}
					catch (ParseException e)
					{
						Console.WriteLine(e.ToString());
						Console.Write(e.StackTrace);
					}
				}
			}
*/
			return false;
		}

#if SysIO
        public bool writeLastUpdate(string aJsonString)//, Context aContext)
		{
            // TODO - handle IO to hold dynamic data (e.g. Currency rates from Internet)
            /*
			try
			{
				System.IO.FileStream fOut = aContext.openFileOutput(DynamicLastFile, Context.MODE_PRIVATE);
				System.IO.StreamWriter osw = new System.IO.StreamWriter(fOut);

				// Write the string to the file
				osw.Write(aJsonString);
				// ensure that everything is 
				// really written out and close 
				osw.Flush();
				osw.Close();
				return true;
			}
			catch (IOException e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
				setanyError(e);
			}
    */
			return false;
		}
#endif
	}
}