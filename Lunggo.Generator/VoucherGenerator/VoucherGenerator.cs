using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Generator.VoucherGenerator
{
    class VoucherGenerator
    {
        //  BEGIN REGION SETTING
        //  ONLY THIS PART THAT MATTERS
        //
        //  CARI PERHATIAN
        //  CARI PERHATIAN
        //  CARI PERHATIAN
        //  CARI PERHATIAN
        //  CARI PERHATIAN
        //  CARI PERHATIAN
        //  CARI PERHATIAN
        //  CARI PERHATIAN
        //  CARI PERHATIAN
        const CharactersPosition POSITION = CharactersPosition.Front;
        const string FIXED_CHARACTERS = "TRVMCL";
        const string FORMAT_VOUCHER = "ZZY";//FORMAT VOUCHER LIAT DI BAWAH
        static int sequenceStartFrom = 1;
        static int sequenceEndAt = 10;
        //  CARI PERHATIAN
        //  CARI PERHATIAN
        //  CARI PERHATIAN
        //  CARI PERHATIAN
        //  CARI PERHATIAN
        //  CARI PERHATIAN
        //  CARI PERHATIAN
        //  CARI PERHATIAN
        //  CARI PERHATIAN
        //
        //  END REGION SETTING


        //FORMATNYA NIH
        const char RANDOM_NUMBER = 'X';
        const char ALPHA_RANDOM = 'Y';
        const char SEQUENCE_NUMBER = 'Z';
        //NIH FORMATNYA

        //FILE PATHNYA NIH
        const string FilePath = @"..\..\VoucherCodes.txt";
        //NIH FILE PATHNYA

        static void Main(string[] args)
        {
            List<string> listResult = new List<string>();
            var stringFormat = GetListString();
            for (int i = sequenceStartFrom; i <= sequenceEndAt; i++)
            {
                string temp = new VoucherGenerator().Generate(stringFormat);
                listResult.Add(temp);
            }

            using (var file = new StreamWriter(FilePath))
            {
                foreach (var voucher in listResult)
                    file.WriteLine(voucher);
            }
        }

        private enum CharactersPosition
        {
            Front,
            Back
        }

        private static readonly object syncLock = new object();
        private static Random rnd = new Random();
        private string AddFixedCharacters(string param)
        {
            switch(POSITION)
            {
                case CharactersPosition.Front:
                    return FIXED_CHARACTERS + param;
                case CharactersPosition.Back:
                    return param + FIXED_CHARACTERS;
            }
            return param;
        }

        private static IList<string> GetListString()
        {
            char temp = '0';
            int index = -1; 
            List<string> listString = new List<string>();
            foreach (char c in FORMAT_VOUCHER.ToUpper())
            {
                if (temp != c)
                {
                    temp = c;
                    index++;
                    listString.Add("");
                }
                listString[index] += temp;
            }
            return listString;
        }
        private string Generate(IList<string> listStringFormat)
        {
            string finalFormat = "";
            foreach (string item in listStringFormat)
            {
                int charCount = item.Length;
                if(item[0] == RANDOM_NUMBER)
                {
                    for (int i = 0; i < charCount; i++)
                    {
                        int tempInt = GetRandom(0, 9);
                        finalFormat += rnd.Next(10);
                    }
                }
                else if(item[0] == ALPHA_RANDOM)
                {
                    for (int i = 0; i < charCount; i++)
                    {
                        int tempInt = GetRandom(0,26);
                        char tempChar = (char)('a' + tempInt);
                        finalFormat += tempChar.ToString().ToUpper();
                    }
                }
                else if(item[0] == SEQUENCE_NUMBER)
                {
                    string zeroFormat = "";
                    for (int i = 0; i < charCount; i++)
                        zeroFormat += "0";
                    finalFormat += sequenceStartFrom.ToString(zeroFormat);
                }
            }
            sequenceStartFrom++;
            finalFormat = AddFixedCharacters(finalFormat);
            return finalFormat;
        }
        private int GetRandom(int min, int max)
        {
            lock (syncLock)
            {
                return rnd.Next(min, max);
            }
        }
    }
}
