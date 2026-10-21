using java.lang;
using System;
using System.Globalization;
namespace Cooperative.Models
{
    public class NumberToWordsConverter
    {
        private static string[] ones = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
                                     "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };

        private static string[] tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        private static string[] indianUnits = { "", "Thousand", "Lakh", "Crore" };


        public static string ConvertAmountToWords(decimal amount)
        {
            if (amount == 0)
                return "Zero";

            string[] parts = amount.ToString("F2").Split('.');
            Int64 integerPart = Int64.Parse(parts[0]);
            Int64 decimalPart = Int64.Parse(parts[1]);

            string words = ConvertIntegerToWords(integerPart) + " Rupees";

            if (decimalPart > 0)
            {
                words += " and " + ConvertIntegerToWords(decimalPart) + " Paise";
            }

            return words + " Only";
        }

        private static string ConvertIntegerToWords(Int64 number)
        {
            if (number == 0)
                return "";

            if (number < 20)
                return ones[number];

            if (number < 100)
                return tens[number / 10] + (number % 10 > 0 ? " " + ones[number % 10] : "");

            if (number < 1000)
                return ones[number / 100] + " Hundred" + (number % 100 > 0 ? " and " + ConvertIntegerToWords(number % 100) : "");

            return ConvertToIndianSystem(number);
        }

        private static string ConvertToIndianSystem(Int64 number)
        {
            string result = "";

            if (number >= 10000000)
            {
                result += ConvertIntegerToWords(number / 10000000) + " Crore ";
                number %= 10000000;
            }

            if (number >= 100000)
            {
                result += ConvertIntegerToWords(number / 100000) + " Lakh ";
                number %= 100000;
            }

            if (number >= 1000)
            {
                result += ConvertIntegerToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if (number >= 100)
            {
                result += ConvertIntegerToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                result += "and " + ConvertIntegerToWords(number);
            }

            return result.Trim();
        }
        #region

        private static readonly string[] units = { "", "एक", "दो", "तीन", "चार", "पाँच", "छह", "सात", "आठ", "नौ", "दस",
        "ग्यारह", "बारह", "तेरह", "चौदह", "पंद्रह", "सोलह", "सत्रह", "अठारह", "उन्नीस" };
        private static readonly string[] tens_hi = { "", "", "बीस", "तीस", "चालीस", "पचास", "साठ", "सत्तर", "अस्सी", "नब्बे" };
        public static string ConvertToHindiWords(long number)
        {
            if (number == 0) return "शून्य रुपये मात्र";

            string result = ConvertNumberToWordsHi(number);
            return result.Trim() + " रुपये मात्र";
        }
        private static string ConvertNumberToWordsHi(long number)
        {
            if (number < 20)
                return units[number];

            if (number < 100)
                return tens_hi[number / 10] + (number % 10 != 0 ? " " + units[number % 10] : "");

            if (number < 1000)
                return units[number / 100] + " सौ " + ConvertNumberToWordsHi(number % 100);

            if (number < 100000)
                return ConvertNumberToWordsHi(number / 1000) + " हजार " + ConvertNumberToWordsHi(number % 1000);

            if (number < 10000000)
                return ConvertNumberToWordsHi(number / 100000) + " लाख " + ConvertNumberToWordsHi(number % 100000);

            return ConvertNumberToWordsHi(number / 10000000) + " करोड़ " + ConvertNumberToWordsHi(number % 10000000);
        }
        #endregion
    }


    public class NumberToHindiWordConverter
    {
        static string[] numbers = {
                "शून्य","एक","दो","तीन","चार","पाँच","छह","सात","आठ","नौ",
                "दस","ग्यारह","बारह","तेरह","चौदह","पंद्रह","सोलह","सत्रह","अठारह","उन्नीस",
                "बीस","इक्कीस","बाईस","तेइस","चौबीस","पच्चीस","छब्बीस","सत्ताईस","अट्ठाईस","उनतीस",
                "तीस","इकतीस","बत्तीस","तैंतीस","चौंतीस","पैंतीस","छत्तीस","सैंतीस","अड़तीस","उनतालीस",
                "चालीस","इकतालीस","बयालीस","तैंतालीस","चौवालीस","पैंतालीस","छियालीस","सैंतालीस","अड़तालीस","उनचास",
                "पचास","इक्यावन","बावन","तिरेपन","चौवन","पचपन","छप्पन","सत्तावन","अट्ठावन","उनसठ",
                "साठ","इकसठ","बासठ","तिरसठ","चौंसठ","पैंसठ","छियासठ","सड़सठ","अड़सठ","उनहत्तर",
                "सत्तर","इकहत्तर","बहत्तर","तिहत्तर","चौहत्तर","पचहत्तर","छिहत्तर","सतहत्तर","अठहत्तर","उन्नासी",
                "अस्सी","इक्यासी","बयासी","तिरासी","चौरासी","पचासी","छियासी","सत्तासी","अट्ठासी","नवासी",
                "नब्बे","इक्यानवे","बानवे","तिरानवे","चौरानवे","पंचानवे","छियानवे","सत्तानवे","अट्ठानवे","निन्यानवे"
                };

        public static string ConvertAmtWords(decimal amount)
        {
            if (amount == 0)
                return "शून्य रुपये मात्र";

            string[] parts = amount.ToString("F2").Split('.');
            long integerPart = long.Parse(parts[0]);
            long decimalPart = long.Parse(parts[1]);

            string words = ConvertIntegerToWords(integerPart) + " रुपये";
            if (decimalPart > 0)
            { words += " और " + ConvertIntegerToWords(decimalPart) + " पैसे"; }
            return words + " मात्र";
        }

        private static string ConvertIntegerToWords(long number)
        {
            if (number == 0)
                return "";

            if (number < 100)
                return numbers[number];

            if (number < 1000)
                return ConvertIntegerToWords(number / 100) + " सौ " + ConvertIntegerToWords(number % 100);

            if (number < 100000)
                return ConvertIntegerToWords(number / 1000) + " हज़ार " + ConvertIntegerToWords(number % 1000);

            if (number < 10000000)
                return ConvertIntegerToWords(number / 100000) + " लाख " + ConvertIntegerToWords(number % 100000);

            return ConvertIntegerToWords(number / 10000000) + " करोड़ " + ConvertIntegerToWords(number % 10000000);
        }


        public static string FormatDecimalAmount(decimal value)
        {
            return value % 1 == 0 ? value.ToString("N0", new CultureInfo("en-IN")) : value.ToString("0.##");
        }
    }


}
