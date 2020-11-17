using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace OCHPlanner3.Controllers
{
    public class SignMessageController : Controller
    {
        private static X509KeyStorageFlags STORAGE_FLAGS = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable;
        private IHostingEnvironment Environment;

        public SignMessageController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        public ActionResult SignMessage()
        {
            string request = HttpContext.Request.Query["request"].ToString();

            var KEY = this.Environment.WebRootPath + "/private/private-key.pfx";
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            var PASS = "";

            try
            {
                var cert = new X509Certificate2(KEY, PASS, STORAGE_FLAGS);

                RSA csp = (RSA)cert.PrivateKey;

                byte[] data = new ASCIIEncoding().GetBytes(request);
                RSACryptoServiceProvider cspStrong = new RSACryptoServiceProvider(); // 2.1 and higher: Make RSACryptoServiceProvider that can handle SHA256, SHA512
                cspStrong.ImportParameters(csp.ExportParameters(true)); // Copy to stronger RSACryptoServiceProvider
                byte[] hash = new SHA512CryptoServiceProvider().ComputeHash(data);  // Use SHA1CryptoServiceProvider for QZ Tray 2.0 and older
                string base64 = Convert.ToBase64String(cspStrong.SignHash(hash, CryptoConfig.MapNameToOID("SHA512"))); // Use "SHA1" for QZ Tray 2.0 and older
                return Content(base64, "text/plain");
            }
            catch (Exception ex)
            {
                if ((STORAGE_FLAGS & X509KeyStorageFlags.MachineKeySet) == X509KeyStorageFlags.MachineKeySet)
                {
                    // IISExpress may fail with "Invalid provider type specified"; remove MachineKeySet flag, try again
                    STORAGE_FLAGS = STORAGE_FLAGS & ~X509KeyStorageFlags.MachineKeySet;
                    return SignMessage();
                }
                throw ex;
            }
        }
    }
}