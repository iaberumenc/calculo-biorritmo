using Calculo_Biorritmo.Models;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Calculo_Biorritmo.Api
{
    class urls
    {

        public static string base_url()
        {
            string url="";
            var originalURL = "https://bioritmoapi.herokuapp.com/api/";
            var originalPath = "API_URL.txt";

            try
            {
                FileStream fileStream;
                fileStream = File.Open(originalPath, FileMode.OpenOrCreate);
                using (var sr = new StreamReader(fileStream))
                {
                    url = sr.ReadLine();
                }
                fileStream.Close();

                if (url == null) {
                    FileStream fileStreamWrite;
                    fileStreamWrite = File.Open(originalPath, FileMode.Append);
                    using (var sw = new StreamWriter(fileStreamWrite))
                    {
                        sw.WriteLine(originalURL);
                    }
                    url = originalURL;
                    fileStreamWrite.Close();
                }

            }
            catch (Exception)
            {
                url = originalURL;
            }
            return url;
        }
    }

    
}
