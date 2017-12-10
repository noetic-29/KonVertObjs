using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft;
using Newtonsoft.Json;

using Xamarin.Forms;

namespace KonVertObjs
{
    // 2017-12-07 EIO all KonVertObjs inherit from this to always have anyError and fixReference properites/methods
    public class KonObj
    {
        public KonVertSet theSet { get; set; }
        public bool ShouldSerializetheSet() { return false; }

        public Exception anyError { get; set; }
        public bool ShouldSerializeanyError() { return false; }

        public KonObj(KonVertSet aSet)
        {
            theSet = aSet;
        }

        [JsonConstructor]
        public KonObj()
        {
            theSet = null;
        }

        public virtual void fixReferences()
        {
            return;        // if unimplemented in a KonObj then nothing to fix, so return true
        }
    }

    public class KonFuncs : Object
    {
        public const string NOTIFICATION = "com.noetic_29.konvert.RetrieveDynamic";
        public const string GROUPID = "com.noetic_29.konvert.GroupID";
        public const string DYNTYPE = "com.noetic_29.konvert.DynamicType";
        public const string RESULT = "com.noetic_29.konvert.RetrieveDynamicResult";
        public const string DEBUG1 = "com.noetic_29.konvert.Debug1";
        public const string DEBUG2 = "com.noetic_29.konvert.Debug2";

        // SEE checkString function below
        // an array of strings to convert
        private static string[] orig_strings = null;
        // an array of the corresponding conversion strings
        private static string[] conv_strings = null;

        public static string[] OrigStrings {
            set {
                orig_strings = value;
            }
        }
        public static string[] ConvStrings {
            set {
                conv_strings = value;
            }
        }

        // seems to be Android specific, will have to check where used
        /*
        private Context theContext = null;
        public Context getmyContext()
        {
            return theContext;
        }
        public void setmyContext(Context value)
        {
            theContext = value;
        }
        */

        // maxDigits for this current conversion group
        public static int konMaxDigits { get; set; }
        /*
         private static int privatekonMaxDigits;
         public static int getkonMaxDigits()
         {
             return privatekonMaxDigits;
         }
         public static void setkonMaxDigits(int value)
         {
             privatekonMaxDigits = value;
         }
         */

        // macDecimals for this current conversion group
        public static int konMaxDecimals { get; set; }
        /*
        private static int privatekonMaxDecimals;
        public static int getkonMaxDecimals()
        {
            return privatekonMaxDecimals;
        }
        public static void setkonMaxDecimals(int value)
        {
            privatekonMaxDecimals = value;
        }
        */

        // culture to use to display values - read from Windows Phone environment
        public static string konKulture { get; set; }
        /*
        private static string privatekonKulture;
        public static string getkonKulture()
        {
            return privatekonKulture;
        }
        public static void setkonKulture(string value)
        {
            privatekonKulture = value;
        }
        */

        public static bool isGroupTime(KonVersionGroup aGroup)
        {
            if (aGroup == null) return false;
            if (aGroup.GroupDynamics != null)
            {
                return aGroup.GroupDynamics.DynamicType.Equals("TIME", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public static bool isGroupCurrency(KonVersionGroup aGroup)
        {
            if (aGroup.GroupDynamics != null)
            {
                return aGroup.GroupDynamics.DynamicType.Equals("Currency", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        // appears to be an array  (only look at first and second digit)
        // 2017-11-17 EIO made more generic for use by KonVertUnitTime
        //    pass third element of array as number of days to add/subtract
        //    since array is passed in, and is probably only 2 deep, instead
        //    return number of days to modify as int (should by -1, 0 or 1)
        // check to see if second digit (must be minutes) is greater that 60 (next hour)
        // or less than 0 (previous hour) and adjust hour appropriately
        // NOTE: does not seem to take days into account
        // QUESTION: Why are these big integers when can't be over 60??????
        public static BigInteger[] fixTime(BigInteger[] aTime)
        {
            BigInteger[] retBigInt = new BigInteger[3];
            // 2017-11-17 EIO simplify using code from KonVertUnitTime
            // 2017-11-18 EIO return a new array of BigIntegers that has as it's third elemen
            //     the number of days to adjust if next or previous day
            //     do NOT change the input aTime - so must make changes to used code
            retBigInt[0] = aTime[0];
            retBigInt[1] = aTime[1];
            retBigInt[2] = 0;

            if (retBigInt[1] > 60)
            {
                retBigInt[1] = retBigInt[1] - 60;
                retBigInt[0] = retBigInt[0] + 1;
            }
            else if (retBigInt[1] < 0)
            {
                retBigInt[1] = retBigInt[1] + 60;
                retBigInt[0] = retBigInt[0] - 1;
            }
            if (retBigInt[0] > 24)
            {
                retBigInt[0] = retBigInt[0] - 24;
                retBigInt[2] = 1;
            }
            else if (retBigInt[0] <= 0)
            {
                retBigInt[0] = retBigInt[0] + 24;
                retBigInt[2] = -1;
            }
            // check for 2400
            if (retBigInt[0] == 0)
            {
                if (retBigInt[1] == 0)
                {
                    retBigInt[0] = 24;
                }
            }
            return retBigInt;
            /*
                        BigInteger bi60 = new BigInteger(60);
                        BigInteger bi24 = new BigInteger(24);

                        if (aTime[1].CompareTo(bi60) > 0)
                        {       // greater than 60, subtract to get remaining minutes and add 1 to hour
                            aTime[1] = aTime[1] - bi60;
                            aTime[0] = aTime[0] + BigInteger.One;
                        }
                        else if (aTime[1].CompareTo(BigInteger.Zero) < 0)
                        {       // less than 0, add 60 to minutes to get valid and subtract 1 from hour
                            aTime[1] = aTime[1] + bi60;
                            aTime[0] = aTime[0] - BigInteger.One;
                        }
                        if (aTime[0].CompareTo(bi24) >= 0)
                        {       // if went over midnite (or is midnite) adjust hour by 24
                            aTime[0] = aTime[0] - bi24;
                        }
                        else if (aTime[0].CompareTo(BigInteger.Zero) < 0)
                        {       // went back beyond midnite, adjust forward 
                            aTime[0] = aTime[0] + bi24;
                        }
                        // check for 2400 - it seems midnite is 2400, 12:01am is 0001
                        if (aTime[0].CompareTo(BigInteger.Zero) == 0)
                        {
                            if (aTime[1].CompareTo(BigInteger.Zero) == 0)
                            {
                                aTime[0] = bi24;
                            }
                        }
            */
        }

        // given the maxdigits and maxdecimals, format the number
        public static string konFormat(double aNum)
        {
            //String myFormat;
            return ("N" + Convert.ToString(aNum));
            //return string.Format(myFormat, aNum);
        }

        // parse a string to find 'E' for powers of 10 (only for double)
        // This function does NOT error check for illogical numbers such as 12-34 or 1.222E+04
        public static double getNum(string aString)
        {
            string mantisa = "";
            string exponent = "";
            int state = 0;

            foreach (char c in aString.ToCharArray())
            {
                if (state == 0) // get the leading number into the mantissa
                {
                    if (char.IsDigit(c) || c == '.' || c == '-') // handle decimal
                    {
                        mantisa += c;
                    }
                    else
                    {
                        state = 1;
                    }
                }
                if (state == 1)
                {
                    if (c == 'E') // found the E so now get power of 10
                    {
                        state = 2;
                        continue; // start with next character
                    }
                }
                if (state == 2)
                {
                    if (char.IsDigit(c) || c == '-' || c == '+')
                    {
                        exponent += c; // build up power of 10
                    }
                    else
                    {
                        break; // must be all done
                    }
                }

            }

            if (mantisa.Equals(""))
            {
                mantisa = "0";
            }
            if (exponent.Equals(""))
            {
                exponent = "0";
            }

            double mant = double.Parse(mantisa);
            double power = int.Parse(exponent);
            double tens = Math.Pow(10.0, power);

            double result = mant * tens;

            return result;
            // must have wanted to debug the conversion since the result is what is returned below 
            //return mant * (double)Math.Pow(10.0, power);
        }

        public static string checkString(string aStr)
        {
            if (orig_strings == null || conv_strings == null || orig_strings.Length != conv_strings.Length)
            {
                return aStr;
            }

            string retStr = "";
            for (int i = 0; i < orig_strings.Length; i++)
            {
                int fPtr = aStr.IndexOf(orig_strings[i], StringComparison.Ordinal);
                if (fPtr > -1)
                {
                    if (fPtr > 0)
                    {
                        retStr = retStr + aStr.Substring(0, fPtr); // start and length (as it appears, not in docs)
                    }
                    retStr = retStr + conv_strings[i];
                    if (aStr.Length > fPtr + orig_strings[i].Length)
                    {
                        retStr = retStr + aStr.Substring(fPtr + orig_strings[i].Length);
                    }
                    return retStr;
                }
            }
            return aStr;
        }

        public static decimal setPrecision(decimal aNum, int aPrecision)
        {
            return Math.Truncate(aNum * (10 ^ aPrecision)) / (10 ^ aPrecision);
        }

        private static string[] fractions = new string[] { "", "1/16", "1/8", "1/6", "3/16", "1/4", "5/16", "1/3", "3/8", "7/16", "1/2", "9/16", "5/8", "2/3", "11/16", "3/4", "13/16", "5/6", "7/8", "15/16", "ONE" };
        private static double?[] range = new double?[] { 0.03125, 0.09375, 0.145833333, 0.177083333, 0.21875, 0.28125, 0.322916667, 0.354166667, 0.40625, 0.46875, 0.53125, 0.59375, 0.645833333, 0.677083333, 0.71875, 0.78125, 0.822916667, 0.854166667, 0.90625, 0.96875, 1.0 };
        public static string toFraction(decimal aNum)
        {
            for (int i = 0; i < fractions.Length; i++)
            {
                if (aNum < (decimal)range[i])
                //if (aNum.compareTo(new decimal(range[i])) < 0)
                {
                    return fractions[i];
                }
            }
            return "ONE";
        }

    }

    public class Base
    {
        private string myDirectory = @"C:\Hold\HoldJSON\";
        public string Directory {
            get {
                return myDirectory;
            }
            set {
                myDirectory = value;
            }
        }
    }

    /*
        public class WriteLastDynamic : AsyncTask<string, Void, string>
        {
            private readonly KonFuncs outerInstance;

            internal Context _Context = null;
            internal Exception _Exception = null;

            public WriteLastDynamic(KonFuncs outerInstance, Context aContext)
            {
                this.outerInstance = outerInstance;
                _Context = aContext;
            }

            protected internal override string doInBackground(params string[] arg0)
            {
                // invoke with string for file name of lastdynamic file and string of contents
                try
                {
                    System.IO.FileStream fOut = _Context.openFileOutput(arg0[0], Context.MODE_PRIVATE);
                    System.IO.StreamWriter osw = new System.IO.StreamWriter(fOut);

                    // Write the string to the file
                    osw.Write(arg0[1]);
                    osw.Flush();
                    osw.Close();
                }
                catch (IOException e)
                {
                    // TOODO Auto-generated catch block
                    _Exception = e;
                    Console.WriteLine(_Exception.ToString());
                    Console.Write(_Exception.StackTrace);
                }
                // TOODO Auto-generated method stub
                return null;
            }
        }
    */

    /*
        //public class RetrieveFeedTask extends AsyncTask<String, Void, RSSFeed> {
        public class RetrieveDynamicTask : AsyncTask<string, Void, string>
        {
            private readonly KonFuncs outerInstance;


            internal Exception exception = null;
            internal string JsonString = null;
            //private JSONObject jsonObject = null;
            internal string _GroupID = null;
            internal string _DynType = null;
            //private final WeakReference<String> _ObjectRef;
            internal Context _Context = null;
            internal int _db1 = 0;
            internal int _db2 = 0;

            public RetrieveDynamicTask(KonFuncs outerInstance, string aGroupID, string aDynType, string results, Context aContext)
            {
                this.outerInstance = outerInstance;
                _GroupID = aGroupID;
                _DynType = aDynType;
                //_ObjectRef = new WeakReference<String>(results);
                _Context = aContext;
            }

            protected internal override string doInBackground(params string[] dynSourceStrs)
            {
                _db1 = -2;
                try
                {
                    URL srce = null;
                    //BufferedReader in = null;
                    //Object obj = null;
                    bool goon = false;
                    _db1 = -1;
                    try
                    {
                        srce = new URL(dynSourceStrs[0]);
                        goon = true;
                        _db1 = 1;
                    }
                    catch (MalformedURLException e)
                    {
                        // TOODO Auto-generated catch block
                        goon = false;
                        Console.WriteLine(e.ToString());
                        Console.Write(e.StackTrace);
                        exception = (Exception)e;
                    }
                    if (goon)
                    {
                        try
                        {
                            System.IO.Stream x = srce.openStream();
                            System.IO.MemoryStream myURLText = new System.IO.MemoryStream(10240);
                            _db2 = 1;
                            int byteCount = 0;
                            sbyte[] buffer = new sbyte[1025];
                            _db2 = 2;
                            while (byteCount > -1)
                            {
                                byteCount = x.Read(buffer, 0, buffer.Length);
                                if (byteCount > -1)
                                {
                                    myURLText.Write(buffer, 0, byteCount);
                                }
                                _db1 = _db1 + 1;
                            }
                            JsonString = myURLText.ToString();
                            exception = null;
                            return JsonString;
                            //goon = true;
                        }
                        catch (IOException e)
                        {
                            // TOODO Auto-generated catch block
                            goon = false;
                            Console.WriteLine(e.ToString());
                            Console.Write(e.StackTrace);
                            exception = (Exception)e;
                        }
                        catch (Exception e)
                        {
                            goon = false;
                            Console.WriteLine(e.ToString());
                            Console.Write(e.StackTrace);
                            exception = (Exception)e;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                    exception = e;
                }
                return (string)null; // null from above
            }

            protected internal virtual void onPostExecute(string results)
            {
                // TOODO: check this.exception 
                bool isOK = false;
                if (exception == null)
                {
                    //String aResult = (String) _ObjectRef.get();
                    //aResult = results;
                    isOK = true;
                }
                // TOODO: do something with the feed
                if (isOK)
                {
                    sendBroadcast(results);
                }
            }

            internal virtual void sendBroadcast(string results)
            {
                Intent intent = new Intent(NOTIFICATION); //put the same message as in the filter you used in the activity when registering the receiver
                intent.putExtra(DYNTYPE, _DynType);
                intent.putExtra(GROUPID, _GroupID);
                intent.putExtra(RESULT, results);
                intent.putExtra(DEBUG1, _db1);
                intent.putExtra(DEBUG2, _db2);
                _Context.sendBroadcast(intent);
            }

            public virtual bool haveNetworkConnection(Context aContext)
            {
                bool haveConnectedWifi = false;
                bool haveConnectedMobile = false;

                ConnectivityManager cm = (ConnectivityManager)aContext.getSystemService(Context.CONNECTIVITY_SERVICE);
                NetworkInfo[] netInfo = cm.AllNetworkInfo;
                foreach (NetworkInfo ni in netInfo)
                {
                    if (ni.TypeName.equalsIgnoreCase("WIFI"))
                    {
                        if (ni.Connected)
                        {
                            haveConnectedWifi = true;
                        }
                    }
                    if (ni.TypeName.equalsIgnoreCase("MOBILE"))
                    {
                        if (ni.Connected)
                        {
                            haveConnectedMobile = true;
                        }
                    }
                }
                return haveConnectedWifi || haveConnectedMobile;
            }
        }
    */

}
