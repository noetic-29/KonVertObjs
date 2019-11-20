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
//NJusing Newtonsoft;
//NJusing Newtonsoft.Json;
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
        public KonVertUserParams(KonVertSet aset) : base(aset) { }

        public virtual bool isPaid { get; set; }

        public bool noBuy { get; set; }

        public virtual long ConVersionCount { get; set; }

        public virtual void incrConVersionCount()
		{
			ConVersionCount += 1;
		}

        // Last (5) conversions performed by this user - preset to 5 standard conversions
        private List<KonVersion> _previousKonversions;
        public List<KonVersion> previousKonversions {
            get 
            {
                if (_previousKonversions == null) _previousKonversions = new List<KonVersion>();
                return _previousKonversions;
            }
            set 
            {
                _previousKonversions = value;
            }
        }

        private List<KonVersion> _userGroupKonversions;
        public List<KonVersion> userGroupKonversions {
            get {
                if (_userGroupKonversions == null) _userGroupKonversions = new List<KonVersion>();
                return _userGroupKonversions;
            }
            set {
                _userGroupKonversions = value;
            }
        }

        // User settable (paid) settings for each KonversionGroup
        private List<KonUserGroupSetting> inUserGroupSettings = new List<KonUserGroupSetting>();
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

        public bool setEqualTo(KonVertUserParams aKonVertUserParams)
        {
            anyError = null;
            try
            {
                this.isPaid = aKonVertUserParams.isPaid;
                this.noBuy = aKonVertUserParams.noBuy;
                this.ConVersionCount = aKonVertUserParams.ConVersionCount;

                // this code did not create a second copy of these lists so changed to contained elements are reflected in both KonVertUserParams
#if EDREM
                this.previousKonversions = aKonVertUserParams.previousKonversions;
                this.userGroupKonversions = aKonVertUserParams.userGroupKonversions;
                this.userGroupSettings = aKonVertUserParams.userGroupSettings;
#endif
                //this.previousKonversions = aKonVertUserParams.previousKonversions;
                this.previousKonversions = new List<KonVersion>();
                foreach (KonVersion aKV in aKonVertUserParams.previousKonversions)
                {
                    KonVersion newKV = new KonVersion();
                    newKV.setEqualTo(aKV);
                    this.previousKonversions.Add(newKV);
                }

                //this.userGroupKonversions = aKonVertUserParams.userGroupKonversions;
                this.userGroupKonversions = new List<KonVersion>();
                foreach (KonVersion aKV in aKonVertUserParams.userGroupKonversions)
                {
                    KonVersion newKV = new KonVersion();
                    newKV.setEqualTo(aKV);
                    this.userGroupKonversions.Add(newKV);
                }

                //this.userGroupSettings = aKonVertUserParams.userGroupSettings;
                this.userGroupSettings = new List<KonUserGroupSetting>();
                foreach (KonUserGroupSetting aKUGS in aKonVertUserParams.userGroupSettings)
                {
                    KonUserGroupSetting newKUGS = new KonUserGroupSetting(this.theSet);
                    newKUGS.setEqualTo(aKUGS);
                    this.userGroupSettings.Add(newKUGS);
                }

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
            addGroupKonVersion(aKonVersion);

			return;
		}

        public void addGroupKonVersion(KonVersion aKonVersion)
        {
            bool gotIt = false;
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

    }
}