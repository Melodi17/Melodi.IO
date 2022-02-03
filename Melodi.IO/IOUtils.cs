using System.Collections.Generic;
using System.Linq;

namespace Box
{
    public class IOUtils
    {
        public static string[] ConnectStrings(string text, bool allowEscape)
        {
            List<string> items = new();
            bool inStr = false;
            string current = "";
            bool escaped = false;
            foreach (char ch in text)
            {
                if (ch == '\\' && !escaped && allowEscape)
                {
                    escaped = true;
                    continue;
                }
                if (ch == '"' && !escaped)
                {
                    inStr = !inStr;
                    items.Add(current);
                    current = "";
                    continue;
                }
                if (ch == ' ' && !inStr)
                {
                    items.Add(current);
                    current = "";
                    continue;
                }

                if (escaped)
                {
                    if (ch == 'n')
                    {
                        current += "\n";
                    }

                    escaped = false;
                    continue;
                }

                current += ch;
                escaped = false;
            }

            items.Add(current);

            items.RemoveAll(x => x.Trim().Length == 0);
            return items.ToArray();
        }
        public static Dictionary<string, string> AssignStrings(string text, string entryDelimiter, string keyValueDelimiter, string valueDefault, bool trimEntries = true)
        {
            Dictionary<string, string> result = new();

            foreach (string entry in text.Split(entryDelimiter))
            {
                string[] splt = entry.Split(keyValueDelimiter, 2);
                string key = splt[0];
                string value = splt.Length > 1 ? splt[1] : valueDefault;
                result[key] = value;
            }

            return result;
        }
    }
}