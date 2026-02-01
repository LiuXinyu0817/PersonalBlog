using System.Text;

namespace PersonalBlog.WebApi.utils.MD5
{
    public class MD5Helper
    {
        public static string MD5Encrypt32(string str)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes =Encoding.UTF8.GetBytes(str);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X"));
                }
                return sb.ToString();
            }
        }
    }
}
