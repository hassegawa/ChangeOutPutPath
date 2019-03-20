using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace ChangeOutPutPath.Core
{
    public class Comun
    {
        public static Dictionary<string, string> XmlToDictionary
                                   (string key, string value, XElement baseElm)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (XElement elm in baseElm.Elements())
            {
                string dictKey = elm.Attribute(key).Value;
                string dictVal = elm.Attribute(value).Value;

                dict.Add(dictKey, dictVal);

            }

            return dict;
        }

        public XElement DictToXml(Dictionary<string, string> inputDict, string elmName, string valuesName)
        {

            XElement outElm = new XElement(elmName);

            Dictionary<string, string>.KeyCollection keys = inputDict.Keys;

            XElement inner = new XElement(valuesName);

            foreach (string key in keys)
            {
                inner.Add(new XAttribute("key", key));
                inner.Add(new XAttribute("value", inputDict[key]));
            }

            outElm.Add(inner);

            return outElm;
        }

        public string GetCurrentBrach(string workingDirectory)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("git.exe");

            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = workingDirectory;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.Arguments = " branch "; // | awk '/\\*/ { print $2; }'";
            
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            List<string> output = new List<string>();
            string lineVal = process.StandardOutput.ReadLine();

            while (lineVal != null)
            {

                output.Add(lineVal);
                lineVal = process.StandardOutput.ReadLine();

            }

            int val = output.Count();
            process.WaitForExit();

            foreach (var branch in output)
            {
                if (branch.StartsWith("*"))
                {
                    return branch;
                }
            }

            return String.Empty;
        }

    }
}
