// GetDigits and NumberOfDigits from:
// https://stackoverflow.com/questions/829174/is-there-an-easy-way-to-turn-an-int-into-an-array-of-ints-of-each-digit

using System.Text;

namespace ship_battle.Data
{
    public class RandStringNum
    {
        public int RandomStringLength => RandomString.Length;
        private string RandomString { get; set; }
        public RandStringNum(int length) 
        {
            RandomString = GetRandomStringNum(length);
        }
        // returns a pseudo random string with given length containing 0-9 digits
        private static string GetRandomStringNum(int length)
        {
            StringBuilder generatedString = new();
            Random random = new();

            const string chars = "0123456789";

            while(true)
            {
                generatedString.Append(chars.Select(c => chars[random.Next(chars.Length)]).Take(8).ToArray());
                if (generatedString.ToString().Length > length) { break; }
            }
            return (generatedString.ToString()[..length]);
        }
        public int GetIntByPos(int pos)
        {
            char character = RandomString[pos];
            if (character is '1') { return 1; }
            if (character is '2') { return 2; }
            if (character is '3') { return 3; }
            if (character is '4') { return 4; }
            if (character is '5') { return 5; }
            if (character is '6') { return 6; }
            if (character is '7') { return 7; }
            if (character is '8') { return 8; }
            if (character is '9') { return 9; }
            return 0;
        }
        public override string ToString()
        {
            return RandomString;
        }
    }
}
