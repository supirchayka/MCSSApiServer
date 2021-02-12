using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSSApiServer
{
    public class AuthOptions
    {
        public const string ISSUER = "MCSSApiServer"; // издатель токена
        public const string AUDIENCE = "AuthClient"; // потребитель токена
        const string KEY = "My$up1R_uSEC*RET-KeYy";   // ключ для шифрации
        public const int LIFETIME = 1; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
