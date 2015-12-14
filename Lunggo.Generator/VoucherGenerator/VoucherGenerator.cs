using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Generator.VoucherGenerator
{
    class VoucherGenerator
    {
        private enum CharactersPosition
        {
            Front,
            Back
        }

        //FORMATNYA NIH
        const char RANDOM_NUMBER = 'X';
        const char ALPHA_RANDOM = 'Y';
        const char SEQUENCE_NUMBER = 'Z';
        //NIH FORMATNYA

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
        
        //  BEGIN REGION SETTING
        //  ONLY THIS THAT MATTERS
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
        const string FIXED_CHARACTERS = "TRVM";
        const string FORMAT_VOUCHER = "YZZZ";//FORMAT VOUCHER LIAT DI ATAS
        static int sequenceStartFrom = 1;
        static int sequenceEndAt = 500;
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


        static void Main(string[] args)
        {
            List<string> listResult = new List<string>();
            var stringFormat = GetListString();
            for (int i = sequenceStartFrom; i <= sequenceEndAt; i++)
            {
                string temp = new VoucherGenerator().Generate(stringFormat);
                listResult.Add(temp);
            }

            foreach (var voucher in listResult)
                Console.WriteLine(voucher);

            Console.ReadLine();
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
