// Copyright Noetic-29 LLC 2014 - 2018
// All rights reserved

// www.noetic-29.com//========================================================================
// This conversion was produced by the Free Edition of
// Java to C# Converter courtesy of Tangible Software Solutions.
// Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
//========================================================================

using System;
using System.Collections.Generic;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;

/*
using Context = android.content.Context;

using JSONArray = org.json.simple.JSONArray;
using JSONObject = org.json.simple.JSONObject;
using JSONParser = org.json.simple.parser.JSONParser;
using ParseException = org.json.simple.parser.ParseException;

using JsonParseException = com.google.gson.JsonParseException;
*/

namespace KonVertObjs
{
	public class KonVertUserParams : KonObj
	{
		private List<KonVersion> inPreviousKonversions = new List<KonVersion>();
		private List<KonVersion> inUserGroupKonversions = new List<KonVersion>();
		private List<KonUserGroupSetting> inUserGroupSettings = new List<KonUserGroupSetting>();

        public KonVertUserParams(KonVertSet aset) : base(aset) { }

        public virtual bool isPaid { get; set; }

        public bool noBuy { get; set; }

        public virtual long ConVersionCount { get; set; }

        public virtual void incrConVersionCount()
		{
			ConVersionCount += 1;
		}

		// Last (5) conversions performed by this user - preset to 5 standard conversions
        public List<KonVersion> previousKonversions { get; set; }

        public List<KonVersion> userGroupKonversions { get; set; }

        // User settable (paid) settings for each KonversionGroup
        public List<KonUserGroupSetting> userGroupSettings {
            get 
            {
                if (inUserGroupSettings == null)
                {
                    inUserGroupSettings = new List<KonUserGroupSetting>();
                }
                return inUserGroupSettings;
            }
            set 
            {
                inUserGroupSettings = value;
            }
        }

        public override void fixReferences()
        {
            if (previousKonversions != null)
            {
                foreach (KonVersion aKV in previousKonversions)
                {
                    aKV.theSet = theSet;
                    aKV.fixReferences();
                }
            }

            if (userGroupKonversions != null)
            {
                foreach (KonVersion aKV in userGroupKonversions)
                {
                    aKV.theSet = theSet;
                    aKV.fixReferences();
                }
            }

            if (userGroupSettings != null)
            {
                foreach (KonUserGroupSetting aKUGS in userGroupSettings)
                {
                    aKUGS.theSet = theSet;
                    aKUGS.fixReferences();
                }
            }
        }

        // 2017-11-17 EIO JSON automatic
#if REM
        public bool? writeJsonFile(string aFileName, Context aContext)
		{
			JSONObject myJSONObject;

			myJSONObject = buildJSONObject();
			try
			{ // catches IOException below
				// ##### Write a file to the disk #####
				/* We have to use the openFileOutput()-method 
				 * the ActivityContext provides, to
				 * protect your file from others and 
				 * This is done for security-reasons. 
				 * We chose MODE_WORLD_READABLE, because
				 *  we have nothing to hide in our file */		
				//FileOutputStream fOut = new FileOutputStream(aFileName);
				System.IO.FileStream fOut = aContext.openFileOutput(aFileName, Context.MODE_PRIVATE);
				System.IO.StreamWriter osw = new System.IO.StreamWriter(fOut);

				// Write the string to the file
				osw.BaseStream.WriteByte(myJSONObject.toJSONString());
				/* ensure that everything is 
				 * really written out and close */
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

		public bool readJsonFile(string aFileName, Context aContext)
		{
			try
			{
				setanyError(null);
				JSONParser parser = new JSONParser();

				 System.IO.FileStream fIn = aContext.openFileInput(aFileName);
				System.IO.StreamReader isr = new System.IO.StreamReader(fIn.FD);

				//Object obj = parser.parse(new FileReader(aFileName));
				object obj = parser.parse(isr);

				JSONObject jsonObject = (JSONObject) obj;
				return loadFromJSONObject(jsonObject);
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

	/*
	 		FileInputStream fIn = aContext.openFileInput(aFileName);
	        InputStreamReader isr = new InputStreamReader(fIn);
	        ///* Prepare a char-Array that will
	        // * hold the chars we read back in.
	        char[] inputBuffer = new char[TESTSTRING.length()];
	        // Fill the Buffer with data from the file
	        isr.read(inputBuffer);
	        // Transform the chars to a String
	        String readString = new String(inputBuffer);
	       
	        // Check if we read back the same chars that we had written out
	        boolean isTheSame = TESTSTRING.equals(readString);
	
	        // WOHOO lets Celebrate =)
	        Log.i("File Reading stuff", "success = " + isTheSame);
	*/

			return false;
		}

		public bool loadFromJSONObject(JSONObject aJSONObject)
		{
			try
			{
				/// <summary>
				/// try {
				/// boolean myFlag = (boolean) aJSONObject.get("myisPaid");
				/// setisPaid(myFlag);
				/// } catch (JsonParseException e){
				/// setisPaid(false);
				/// }
				/// </summary>

				inPreviousKonversions = new List<KonVersion>();
				JSONArray ja = new JSONArray();
				ja = (JSONArray) aJSONObject.get("previousKonversions");
				for (int i = 0; i < ja.size(); i++)
				{
					JSONObject aKonv = (JSONObject) ja.get(i);
					KonVersion myKV = new KonVersion();
					myKV.loadFromJSONObject(aKonv);
					inPreviousKonversions.Add(myKV);
				}

				inUserGroupKonversions = new List<KonVersion>();
				JSONArray jag = new JSONArray();
				jag = (JSONArray) aJSONObject.get("userGroupKonversions");
				for (int i = 0; i < jag.size(); i++)
				{
					JSONObject aKonv = (JSONObject) jag.get(i);
					KonVersion myKV = new KonVersion();
					myKV.loadFromJSONObject(aKonv);
					inUserGroupKonversions.Add(myKV);
				}

				if (aJSONObject.containsKey("myisPaid"))
				{
					bool myFlag = (bool?) aJSONObject.get("myisPaid").Value;
					setisPaid(myFlag);

					if (aJSONObject.containsKey("myNoBuy"))
					{
						bool myNoBuy = (bool?) aJSONObject.get("myNoBuy").Value;
						setnoBuy(myNoBuy);
					}
					else
					{
						setnoBuy(false);
					}

					// 2014-10-07 EIO add conversion counter so can space interstitial ads
					if (aJSONObject.containsKey("myConVersionCount"))
					{
						long myCount = (long?) aJSONObject.get("myConVersionCount").Value;
						ConVersionCount = (int) myCount;
					}
					else
					{
						ConVersionCount = 0;
					}

					inUserGroupSettings = new List<KonUserGroupSetting>();
					JSONArray jug = new JSONArray();
					try
					{
						jug = (JSONArray) aJSONObject.get("userGroupSettings");
						for (int i = 0; i < jug.size(); i++)
						{
							JSONObject aKUGS = (JSONObject) jug.get(i);
							KonUserGroupSetting myKUGS = new KonUserGroupSetting();
							myKUGS.loadFromJSONObject(aKUGS);
							inUserGroupSettings.Add(myKUGS);
						}
					}
					catch (JsonParseException)
					{
						//int i = 1;
						// do nothing, should be empty array
					}
				}
				else
				{
					setisPaid(false);
					setnoBuy(false);
					inUserGroupSettings = new List<KonUserGroupSetting>(); // empty array
				}

				return true;
			}
			catch (JsonParseException)
			{
				return false;
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public final org.json.simple.JSONObject buildJSONObject()
		public JSONObject buildJSONObject()
		{
			try
			{
				JSONObject myJSONObject = new JSONObject();

				myJSONObject.put("myisPaid", getisPaid());
				// 2014-10=07 EIO - save counts of conversions
				myJSONObject.put("myConVersionCount", ConVersionCount);
				myJSONObject.put("myNoBuy", getnoBuy());

				JSONArray ja = new JSONArray();
				for (int i = 0; i < inPreviousKonversions.Count; i++)
				{
					KonVersion aKVU = inPreviousKonversions[i];
					JSONObject aUnit = aKVU.buildJSONObject();
					ja.add(aUnit);
				}
				myJSONObject.put("previousKonversions", ja);

				JSONArray jag = new JSONArray();
				for (int i = 0; i < inUserGroupKonversions.Count; i++)
				{
					KonVersion aKVU = inUserGroupKonversions[i];
					JSONObject aUnit = aKVU.buildJSONObject();
					jag.add(aUnit);
				}
				myJSONObject.put("userGroupKonversions", jag);

				JSONArray jug = new JSONArray();
				for (int i = 0; i < inUserGroupSettings.Count; i++)
				{
					KonUserGroupSetting aKUG = inUserGroupSettings[i];
					JSONObject aUnit = aKUG.buildJSONObject();
					jug.add(aUnit);
				}
				myJSONObject.put("userGroupSettings", jug);

				return myJSONObject;
			}
			catch (JsonParseException)
			{
				return null;
			}
		}
#endif

        public bool setEqual(KonVertUserParams aKonVertUserParams)
        {
            anyError = null;
            try
            {
                this.isPaid = aKonVertUserParams.isPaid;
                this.noBuy = aKonVertUserParams.noBuy;
                this.ConVersionCount = aKonVertUserParams.ConVersionCount;

                this.previousKonversions = aKonVertUserParams.previousKonversions;
                this.userGroupKonversions = aKonVertUserParams.userGroupKonversions;
                this.userGroupSettings = aKonVertUserParams.userGroupSettings;

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

        public void addKonVersion(KonVersion aKonVersion)
		{
			bool gotIt = false;
			int prevCount = previousKonversions.Count;
			// clean up just in case somehow we got more than 5
			if (prevCount > 5)
			{
				for (int i = prevCount - 1; i > 4; i--) // count down should be quicker since internal table just removes from end
				{
					previousKonversions.RemoveAt(i);
				}
			}

			// now check to see if there already is a KonVersion in the previous list that has this GroupID, LeftID and RIghtID
			foreach (KonVersion aKonv in previousKonversions)
			{
				if (aKonv.myVersionGroupID.Equals(aKonVersion.myVersionGroupID) 
                    && aKonv.myVertUnitLeftID.Equals(aKonVersion.myVertUnitLeftID) 
                        && aKonv.myVertUnitRightID.Equals(aKonVersion.myVertUnitRightID))
				{
					previousKonversions.Remove(aKonv);
					gotIt = true;
				}
			}

			if (gotIt == false)
			{
				previousKonversions.RemoveAt(4);
			}
			previousKonversions.Insert(0, aKonVersion);


			// Stand to reason that if this is the most recent previous conversion, then it's also the last Konversion for it's group
			foreach (KonVersion aKonv in userGroupKonversions)
			{
				if (aKonv.myVersionGroupID.Equals(aKonVersion.myVersionGroupID))
				{
					aKonVersion.makeLastKonversion();
					userGroupKonversions.Remove(aKonv);
					userGroupKonversions.Add(aKonVersion);
					gotIt = true;
					break;
				}
			}
			if (gotIt == false)
			{
				aKonVersion.makeLastKonversion();
				userGroupKonversions.Add(aKonVersion);
			}
			return;
		}

		public void addUserGroupSetting(KonUserGroupSetting aUserGroupSetting)
		{
			foreach (KonUserGroupSetting aKUGS in userGroupSettings)
			{
				if (aKUGS.myVersionGroupID.Equals(aUserGroupSetting.myVersionGroupID))
				{
					userGroupSettings.Remove(aKUGS);
					break;
				}
			}
			userGroupSettings.Add(aUserGroupSetting);
		}

		public virtual KonUserGroupSetting findUserGroupSetting(string aGroupID)
		{
			foreach (KonUserGroupSetting aKUGS in userGroupSettings)
			{
				if (aKUGS.myVersionGroupID.Equals(aGroupID))
				{
					return aKUGS;
				}
			}
			return null;
		}

        public bool readJsonFile(string aFileName)
        {
            anyError = null;
            try
            {
                KonVertUserParams myUserParams = (KonVertUserParams)JsonConvert.DeserializeObject<KonVertUserParams>(aFileName);
                setEqual(myUserParams);
                return true;
            }
            catch (JsonException je)
            {
                anyError = je;
                return false;
            }
        }
        /*
                public bool writeJsonFile(string aFileName)
                {
                    anyError = null;
                    try
                    {

                        var text = File.ReadAllText("TestData/ReadMe.txt");
                        Console.WriteLine(text);


                        TextWriter xx = new TextWriter();


                        TextWriter(@"c:\movie.json", JsonConvert.SerializeObject(movie));

                    }
                    catch (JsonException je)
                    {
                        anyError = je;
                        return false;
                    }

                }
        */
    }
}