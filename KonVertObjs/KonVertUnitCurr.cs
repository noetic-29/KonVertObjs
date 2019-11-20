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

        public string DynamicResults { get; set; }

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

    }
}