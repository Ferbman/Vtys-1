using ProjeYonetimi.Models;
using System.Linq;
using System.Web.Mvc;

namespace ProjeYonetimi.Controllers
{
    public class HomeController : Controller
    {
        public static string PasswordEncrypt(string password)
        {
            int minAsciiVal = 32;
            int maxAsciiVal = 126;
            int key = 13;
            int mirrorKey = -17;

            string crypto = "";
            string cryptoMirror = "";

            string mirror = new string(password.Reverse().ToArray());

            for (int i = 0; i < password.Length; i++)
            {
                int charPassword = (int)password[i] + key;
                int charMirror = (int)mirror[i] + mirrorKey;

                if (charPassword > maxAsciiVal)
                    charPassword = charPassword - maxAsciiVal + minAsciiVal - 1;

                if (charMirror < minAsciiVal)
                    charMirror = charMirror + maxAsciiVal - minAsciiVal + 1;

                // Escape single quotes in the password
                if (charPassword == 39)
                {
                    charPassword = int.Parse("39" + charPassword);
                }

                if (charMirror == 39)
                {
                    charMirror = int.Parse("39" + charMirror);
                }

                // Escape double quotes in the password
                if (charPassword == 34)
                {
                    charPassword = int.Parse("34" + charPassword);
                }

                if (charMirror == 34)
                {
                    charMirror = int.Parse("34" + charMirror);
                }

                crypto += (char)charPassword;
                cryptoMirror += (char)charMirror;
            }

            string newPassword = crypto + cryptoMirror;

            return newPassword;
        }

    }
}