// Copyright Noetic-29 LLC 2014 - 2019
// All rights reserved
// www.noetic-29.com
#define NewtonSoft

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KonVertObjs
{
    // Base class for all SwypConvert conversion allowed for this version of the product
    public class KonVertSet : KonObj
    {
        // INTERNAL VARIABLES
        private List<KonVersionGroup> myKonVersionGroups;

        // CONSTRUCTORS
        public KonVertSet(KonVertSet aSet) : base(aSet)
        {
            theSet = this;
        }

        // PROPERTIES
        // Name of this version of SwypConvert (e.g. SwypConvertTraveler???)
        public string myZwypConvertName { get; set; }
        // Release in form 1.0.0.0
        public string myZwypConvertRelease { get; set; }
        // User parameters read from a separate file in the InstanceFolder on a Phone
        // The default/static version of SwypConvert.txt includes a default set of previous and group level Konversions to start the program off
        // after installation.
        public KonVertUserParams myKonVertUserParams { get; set; }

        // Collection of KonVersionGroups supported by this version of SwypConvert
        public List<KonVersionGroup> KonVersionGroups {
            get {
                if (myKonVersionGroups == null)
                {
                    myKonVersionGroups = new List<KonVersionGroup>();
                }
                return myKonVersionGroups;
            }
            set {
                if (myKonVersionGroups != null)
                {
                    myKonVersionGroups = null;
                }
                myKonVersionGroups = value;
            }
        }

        public override void fixReferences()
        {
            if (myKonVertUserParams != null)
            {
                myKonVertUserParams.theSet = this;
                myKonVertUserParams.fixReferences();
            }
            if (KonVersionGroups != null)
            {
                foreach (KonVersionGroup aKVG in KonVersionGroups)
                {
                    aKVG.theSet = this;
                    aKVG.fixReferences();
                }
            }
            return;
        }

        // boolean set if user buys out of ads
        public Boolean isPaid {
            get {
                if (myKonVertUserParams != null)
                {
                    return myKonVertUserParams.isPaid;
                }
                else
                    return false;
            }
            set {
                if (myKonVertUserParams != null) myKonVertUserParams.isPaid = value;
            }
        }

        // boolean true if user pressed don't remind me on buy screen
        public Boolean noBuy {
            get {
                if (myKonVertUserParams != null)
                    return myKonVertUserParams.noBuy;
                else
                    return false;
            }
            set {
                if (myKonVertUserParams != null) myKonVertUserParams.noBuy = value;
            }
        }

        // number of conversions performed, used for interstitial ads
        public long ConVersionCount {
            get {
                if (myKonVertUserParams != null)
                    return myKonVertUserParams.ConVersionCount;
                else
                    return 0;
            }
            set {
                if (myKonVertUserParams != null) myKonVertUserParams.ConVersionCount = value;
            }
        }

        public void incrConVersionCount()
        {
            if (myKonVertUserParams != null) myKonVertUserParams.incrConVersionCount();
        }

        // private array to hold original settings from Groups.  Must be loaded after
        // first reading swypconvert_txt
        private List<KonUserGroupSetting> hldDefaultSettings;
        public List<KonUserGroupSetting> DefaultSettings
        {
            get {
                return hldDefaultSettings;
            }
            set {
                hldDefaultSettings = value;
            }
        }

        // METHODS
        public bool contains(KonVersionGroup aGroup)
        {
            return KonVersionGroups.Contains(aGroup);
        }

        public void remove(KonVersionGroup aGroup)
        {
            bool gotOne = true;
            while (gotOne)
            {
                gotOne = KonVersionGroups.Remove(aGroup);
            }
        }

        public KonVersionGroup getKonVersionGroup(string aKonVersionGroupID)
        {
            if (myKonVersionGroups != null && !String.IsNullOrEmpty(aKonVersionGroupID))
            {
                foreach (KonVersionGroup aKonVersionGroup in myKonVersionGroups)
                {
                    if (String.Compare(aKonVersionGroup.myVersionGroupID, aKonVersionGroupID) == 0)
                    {
                        return aKonVersionGroup;
                    }
                }
            }
            return null;
        }
#if EDREM

#if WINDOWS
        // Routine to read KonVertParams and load in objects where only strings are saved
        public bool loadDynamicUserParams()
        {	// routine to use standard KonVertUserParams location in the Instance Folder
            Base aBase = new Base();
            string aFile = aBase.Directory + "KonVertUserParams.txt";
            return loadDynamicUserParams(aFile);
        }

        public bool loadDynamicUserParams(string aUserParamsFile)
        {
            KonVertUserParams aKonVertUserParams = new KonVertUserParams();
            if (aKonVertUserParams.readJsonFile(aUserParamsFile) == true) {
                // able to read KonVertUserParams from Local data so use it
                myKonVertUserParams = aKonVertUserParams;
            } else {
                // if no KonVertUserParams file, will use the ones embedded in SwyConvert.txt read in from app data
                // but must load the Group/unit object - so leave myKonVertUserParams as is
                // Be Ware - must be sure that SwypConvert.txt includes a KonVertUserParams
            }

            KonVertUserParams myUParams = myKonVertUserParams;
            if (myUParams != null) {
                foreach (KonVersion aKonVersion in myUParams.previousKonversions) {
				    // since favorite Konversions are not saved with their groups, must insert group and unit
				    // objects into Konversions
                    if (aKonVersion.myVersionGroupID != "") {
                        KonVersionGroup aKonVersionGroup = getKonVersionGroup(aKonVersion.myVersionGroupID);
                        if (aKonVersionGroup != null) aKonVersion.setGroup(getKonVersionGroup(aKonVersion.myVersionGroupID));
                    }
                }

                foreach (KonVersion aKonVersion in myUParams.userGroupKonversions) {
    				// must also update Group most recent conversions (although not used as of 2014-10-01)
                    if (aKonVersion.myVersionGroupID != "") {
                        KonVersionGroup aKonVersionGroup = getKonVersionGroup(aKonVersion.myVersionGroupID);
                        if (aKonVersionGroup != null) {
                            // put group into the Konversion
                            aKonVersion.setGroup(getKonVersionGroup(aKonVersion.myVersionGroupID));
                            // and put conversion into group
                            aKonVersionGroup.myLastKonVersion = aKonVersion;
                        }
                    }
                }
            }
            return true;
        }
#elif WINDOWSPHONE
        // Routine to read KonVertParams and load in objects where only strings are saved
        public async Task<bool> loadDynamicUserParams()
        {	// routine to use standard KonVertUserParams location in the Instance Folder
            string aFile = "KonVertUserParams.txt";
            return await loadDynamicUserParams(aFile);
        }

        public async Task<bool> loadDynamicUserParams(string aUserParamsFile)
        {
            KonVertUserParams aKonVertUserParams = new KonVertUserParams();
            if (await aKonVertUserParams.readJsonFile(aUserParamsFile) == true)
            {
                // able to read KonVertUserParams from Local data so use it
                myKonVertUserParams = aKonVertUserParams;
            }
            else
            {
                // if no KonVertUserParams file, will use the ones embedded in SwyConvert.txt read in from app data
                // but must load the Group/unit object - so leave myKonVertUserParams as is
                // Be Ware - must be sure that SwypConvert.txt includes a KonVertUserParams
            }

            KonVertUserParams myUParams = myKonVertUserParams;
            if (myUParams != null)
            {
                foreach (KonVersion aKonVersion in myUParams.previousKonversions)
                {
                    if (aKonVersion.myVersionGroupID != "")
                    {
                        KonVersionGroup aKonVersionGroup = getKonVersionGroup(aKonVersion.myVersionGroupID);
                        if (aKonVersionGroup != null) aKonVersion.setGroup(getKonVersionGroup(aKonVersion.myVersionGroupID));
                    }
                }

                foreach (KonVersion aKonVersion in myUParams.userGroupKonversions)
                {
                    if (aKonVersion.myVersionGroupID != "")
                    {
                        KonVersionGroup aKonVersionGroup = getKonVersionGroup(aKonVersion.myVersionGroupID);
                        if (aKonVersionGroup != null)
                        {
                            // put group into the Konversion
                            aKonVersion.setGroup(getKonVersionGroup(aKonVersion.myVersionGroupID));
                            // and put conversion into group
                            aKonVersionGroup.myLastKonVersion = aKonVersion;
                        }
                    }
                }
            }
            return true;
        }
#endif

        // Routine to write internal KonVertUserParams to disk when user pauses, app looses focus or closes
#if WINDOWS
        public bool SaveUserParams() {
            Base aBase = new Base();
            string aFile = aBase.Directory + "KonVertUserParams.txt";
            return SaveUserParams(aFile);
        }

        public bool SaveUserParams(string aUserParamsFile) {
            if (myKonVertUserParams != null)
            {
                return myKonVertUserParams.writeJsonFile(aUserParamsFile);
            }
            return false;
        }
#elif WINDOWSPHONE
        public async Task<bool> SaveUserParams()
        {
            //Base aBase = new Base();
            string aFile = "KonVertUserParams.txt";
            return await SaveUserParams(aFile);
        }

        public async Task<bool> SaveUserParams(string aUserParamsFile)
        {
            if (myKonVertUserParams != null)
            {
                return await myKonVertUserParams.writeJsonFile(aUserParamsFile);

            }
            return false;
        }
#endif
#if WINDOWS
        public bool writeJsonFile(string aFileName) {
            try {
                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                anyError = null;

                using (StreamWriter fileWriter = new StreamWriter(aFileName))
                using (JsonWriter JsonWriter = new JsonTextWriter(fileWriter)) {
                    serializer.Serialize(JsonWriter, this);
                }
                return true;
            } catch (Exception e) {
                anyError = e;
                //throw;
                return false;
            }
        }
#elif WINDOWSPHONE
        public async void writeJsonFile(string aFileName)               // KonVertSet
        {
            try
            {
                anyError = null;
                StorageFolder installedFolder = Package.Current.InstalledLocation;
                //StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile jsonFile = await installedFolder.CreateFileAsync(aFileName, CreationCollisionOption.OpenIfExists);
                Stream jsonOut = await jsonFile.OpenStreamForWriteAsync();

                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;

                using (StreamWriter fileWriter = new StreamWriter(jsonOut))
                using (JsonWriter JsonWriter = new JsonTextWriter(fileWriter))
                {
                    serializer.Serialize(JsonWriter, this);
                }
                return;
            }
            catch (Exception e)
            {
                anyError = e;
                return;
            }
        }
#endif
#if WINDOWS
        public bool readJsonFile(string aFileName) {
            try {
                KonVertSet myKonVertSet;
                anyError = null;

                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;

                using (StreamReader fileReader = new StreamReader(aFileName)) {
                    myKonVertSet = (KonVertSet)serializer.Deserialize(fileReader, typeof(KonVertSet));
                }

                doSortGroup(myKonVertSet.KonVersionGroups, true);

                foreach (KonVersionGroup aGroup in myKonVertSet.KonVersionGroups) {
                    KonVersionGroup.doSortUnit(aGroup.konVertUnits, true);
                }

                return setEqual(myKonVertSet);
            } catch (Exception e) {
                anyError = e;
                //throw;
                return false;
            }
        }
#elif WINDOWSPHONE
        public async Task<bool> readJsonFile(string aFileName)      // KonVertSet
        {
            try
            {
                anyError = null;
                KonVertSet myKonVertSet;
                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;

                StorageFolder installedFolder = Package.Current.InstalledLocation;
                Stream jsonIn = await installedFolder.OpenStreamForReadAsync(aFileName);
                using (StreamReader fileReader = new StreamReader(jsonIn))
                {
                    myKonVertSet = (KonVertSet)serializer.Deserialize(fileReader, typeof(KonVertSet));
                }
                doSortGroup(myKonVertSet.KonVersionGroups, true);

                setEqual(myKonVertSet);
                return true;
            }
            catch (Exception e)
            {
                anyError = e;
                return false;
            }
        }
#endif
#endif

        public static void doSortGroup(List<KonVersionGroup> aWrkList, bool ascending)
        {
            bool didSwap = true;

            while (didSwap)
            {
                didSwap = false;
                for (int i = 0; i < aWrkList.Count(); ++i)
                {
                    if (i + 1 >= aWrkList.Count()) break;
                    if (ascending)
                    {
                        if (aWrkList.ElementAt<KonVersionGroup>(i + 1).myGroupListOrder < aWrkList.ElementAt<KonVersionGroup>(i).myGroupListOrder)
                        {
                            KonVersionGroup tmp = aWrkList.ElementAt<KonVersionGroup>(i + 1);
                            aWrkList.RemoveAt(i + 1);
                            aWrkList.Insert(i, tmp);
                            didSwap = true;
                        }
                    }
                    else
                    {
                        if (aWrkList.ElementAt<KonVersionGroup>(i + 1).myGroupListOrder > aWrkList.ElementAt<KonVersionGroup>(i).myGroupListOrder)
                        {
                            KonVersionGroup tmp = aWrkList.ElementAt<KonVersionGroup>(i + 1);
                            aWrkList.RemoveAt(i + 1);
                            aWrkList.Insert(i, tmp);
                            didSwap = true;
                        }
                    }
                }
            }
        }

        public bool setEqualTo(KonVertSet aKonVertSet)
        {
            anyError = null;
            try
            {
                this.myZwypConvertName = aKonVertSet.myZwypConvertName;
                this.myZwypConvertRelease = aKonVertSet.myZwypConvertRelease;
                this.myKonVersionGroups = aKonVertSet.myKonVersionGroups;
                this.myKonVertUserParams = aKonVertSet.myKonVertUserParams;

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
            return myZwypConvertName;
        }

        public void setUpUserSettings()
        {
            // these don't make sense since KonVerSet doesn't keep a copy but references KonVerUserParams
            isPaid = myKonVertUserParams.isPaid;
            noBuy = myKonVertUserParams.noBuy;
            ConVersionCount = myKonVertUserParams.ConVersionCount;

            foreach (KonUserGroupSetting aKUGS in myKonVertUserParams.userGroupSettings)
            {
                KonVersionGroup myKVG = getKonVersionGroup(aKUGS.myVersionGroupID);
                if (myKVG != null)
                {
                    myKVG.myDoPrettyPrint = aKUGS.myDoPrettyPrint;
                    foreach (KonUserVertUnit aKUVU in aKUGS.myUserVertUnits)
                    {
                        KonVertUnit myKVU = myKVG.getUnit(aKUVU.myVersionUnitID);
                        if (myKVU != null)
                        {
                            myKVU.myUnitPrecision = aKUVU.myUnitPrecision;
                            myKVU.myDoDecimal = aKUVU.myDoDecimal;
                        }
                    }
                }
            }
        }

        public void defaultUserSettings()
        {
            // tricky so hold the previous KonUserGroupSettings and then pass back in the favorites
            if (hldDefaultSettings != null)
            {
                foreach (KonUserGroupSetting aKUGS in hldDefaultSettings)
                {
                    KonVersionGroup myKVG = getKonVersionGroup(aKUGS.myVersionGroupID);
                    if (myKVG != null)
                    {
                        myKVG.myDoPrettyPrint = aKUGS.myDoPrettyPrint;
                        foreach (KonUserVertUnit aKUVU in aKUGS.myUserVertUnits)
                        {
                            KonVertUnit aKVU = myKVG.getUnit(aKUVU.myVersionUnitID);
                            if (aKVU != null)
                            {
                                aKVU.myDoDecimal = aKUVU.myDoDecimal;
                                aKVU.myUnitPrecision = aKUVU.myUnitPrecision;
                            }
                        }
                    }
                }
            }
        }

#if NewtonSoft
        // Routine to read KonVertParams and load in objects where only strings are saved
        public bool loadDynamicUserParams()
        {	// routine to use standard KonVertUserParams location in the Instance Folder
            Base aBase = new Base();
            string aFile = aBase.Directory + "KonVertUserParams.txt";
            return loadDynamicUserParams(aFile);
            //return false;  //NJ
        }

        public bool loadDynamicUserParams(string aUserParamsFile)
        {
#if NeedsWork
            KonVertUserParams aKonVertUserParams = new KonVertUserParams(theSet);
            if (aKonVertUserParams.readJsonFile(aUserParamsFile) == true)
            {
                // able to read KonVertUserParams from Local data so use it
                myKonVertUserParams = aKonVertUserParams;
            }
            else
            {
                // if no KonVertUserParams file, will use the ones embedded in SwyConvert.txt read in from app data
                // but must load the Group/unit object - so leave myKonVertUserParams as is
                // Be Ware - must be sure that SwypConvert.txt includes a KonVertUserParams
            }

            KonVertUserParams myUParams = myKonVertUserParams;
            if (myUParams != null)
            {
                //  2017-12-07 EIO now that root object has KonVertSet, no longer need Groups in sub-objects
                //    just need GroupID (myVersionGroupID)
                       
                foreach (KonVersion aKonVersion in myUParams.previousKonversions)
                {
                    // since favorite Konversions are not saved with their groups, must insert group and unit
                    // objects into Konversions
                    if (aKonVersion.myVersionGroupID != "")
                    {
                        KonVersionGroup aKonVersionGroup = getKonVersionGroup(aKonVersion.myVersionGroupID);
                        if (aKonVersionGroup != null) aKonVersion.myVersionGroupID = getKonVersionGroup(aKonVersion.myVersionGroupID).myVersionGroupID;
                    }
                }

                foreach (KonVersion aKonVersion in myUParams.userGroupKonversions)
                {
                    // must also update Group most recent conversions (although not used as of 2014-10-01)
                    if (aKonVersion.myVersionGroupID != "")
                    {
                        KonVersionGroup aKonVersionGroup = getKonVersionGroup(aKonVersion.myVersionGroupID);
                        if (aKonVersionGroup != null)
                        {
                            // put group into the Konversion
                            //aKonVersion.myVersionGroupID = getKonVersionGroup(aKonVersion.myVersionGroupID);
                            // and put conversion into group
                            aKonVersionGroup.myLastKonVersion = aKonVersion;
                        }
                    }
                }
            }
#endif
            return true;
        }


        public bool SaveUserParams()
        {
            Base aBase = new Base();
            string aFile = aBase.Directory + "KonVertUserParams.txt";
            return SaveUserParams(aFile);
        }

        public bool SaveUserParams(string aUserParamsFile)
        {
            return false;
/*
            if (myKonVertUserParams != null)
            {
                return myKonVertUserParams.writeJsonFile(aUserParamsFile);
            }
            return false;
*/
        }
#endif
        }
}
