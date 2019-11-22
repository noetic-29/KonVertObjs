// Copyright Noetic-29 LLC 2014 - 2019
// All rights reserved
// www.noetic-29.com

using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//NJusing Newtonsoft;
//NJusing Newtonsoft.Json;

//using Xamarin.Forms;

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

        public string addl { get; set; }

        //NJ[JsonConstructor]
        public KonObj()
        {
            theSet = null;
        }

        public virtual void fixReferences()
        {
            return;        // if unimplemented in a KonObj then nothing to fix, so return true
        }
    
        // 2019-11-20 added setEqualTo as can implement base function
        public virtual bool setEqualTo(KonObj aKO)
        {
            this.theSet = aKO.theSet;
            this.anyError = aKO.anyError;
            return true;
        }

        public virtual KonObj makeStableCopy(KonObj aKO)
        {
            KonObj retKO = new KonObj();
            retKO.setEqualTo(aKO);
            return retKO;
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

        // maxDigits for this current conversion group
        public static int konMaxDigits { get; set; }

        // macDecimals for this current conversion group
        public static int konMaxDecimals { get; set; }

        // culture to use to display values - read from Windows Phone environment
        public static string konKulture { get; set; }

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
            // multiply by number of decimals required, then truncate.  Finally divide bace to get decimal
            decimal bigNum = aNum * (decimal)(Math.Pow(10, aPrecision));
            decimal neededNum = Math.Truncate(bigNum);
            decimal retNum = neededNum / (decimal)Math.Pow(10, aPrecision);
            return retNum;
            //return Math.Truncate((aNum * (decimal)(Math.Pow(10,aPrecision))) / ((decimal)(Math.Pow(10, aPrecision))));
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
}
