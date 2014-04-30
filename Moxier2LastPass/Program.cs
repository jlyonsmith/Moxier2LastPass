using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Moxier2LastPass
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var reader = new StreamReader(args[0], Encoding.Unicode);
            var writer = new StreamWriter(args[1]);

            try 
            {
                writer.WriteLine(CreateLastPassCsvLine(new string[]
                { "url", "type", "username", "password", "hostname", "extra", "name", "grouping"}));

                string line;
                IList<string> outFields;

                while ((line = reader.ReadLine()) != null)
                {
                    IList<string> inFields = ParseMoxierCsvLine(line);

                    switch (inFields[1])
                    {
                    case "Web Logins":
                        outFields = CreateWebLogin(inFields);
                        break;
                    case "Social Security":
                        outFields = CreateSocialSecurity(inFields);
                        break;
                    case "Bank Accounts":
                        outFields = CreateBankAccount(inFields);
                        break;
                    case "Credit Cards":
                        outFields = CreateCreditCard(inFields);
                        break;
                    case "Insurance":
                        outFields = CreateInsurance(inFields);
                        break;
                    case "Driver License":
                        outFields = CreateDriverLicense(inFields);
                        break;
                    case "Notes":
                        outFields = CreateNotes(inFields);
                        break;
                    default:
                        Console.WriteLine("Unhandled MoxierWallet type '{0}'", inFields[1]);
                        continue;
                    }

                    writer.WriteLine(CreateLastPassCsvLine(outFields));
                }
            } 
            finally 
            {
                reader.Close();
                writer.Close();
            }
        }

        static IList<string> ParseMoxierCsvLine(string line)
        {
            var list = new List<string>();

            if (line.Length == 0)
                return list;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ',' || line[i] == '\n')
                {
                    list.Add(sb.ToString());
                    sb.Clear();
                }
                else if (line[i] == '\\')
                {
                    i++;

                    if (i >= line.Length)
                        throw new FormatException();

                    if (line[i] == '.')
                        sb.Append(',');
                    else if (line[i] == '_')
                        sb.Append('\n');
                    else
                        throw new FormatException();
                }
                else
                {
                    sb.Append(line[i]);
                }
            }

            list.Add(sb.ToString());

            return list;
        }

        static string CreateLastPassCsvLine(IList<string> fields)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].IndexOf('\n') == -1)
                    sb.Append(fields[i]);
                else
                {
                    sb.Append('\"');
                    sb.Append(fields[i]);
                    sb.Append('\"');
                }

                if (i < fields.Count - 1)
                    sb.Append(",");
            }

            return sb.ToString();
        }

        static IList<string> CreateWebLogin(IList<string> inFields)
        {
            return CreateLastPassFields(inFields[16].Length == 0 ? "http://unknown.com" : inFields[16], 
                "", inFields[8], inFields[12], "", inFields[4], inFields[0], "");
        }

        static IList<string> CreateSocialSecurity(IList<string> inFields)
        {
            var extra = String.Format("NoteType:Social Security\nName:{0}\nNumber:{1}\nNotes:{2}",
                inFields[12],
                inFields[8],
                inFields[4]
            );

            return CreateLastPassFields("http://sn", 
                "", "", "", "", extra, inFields[0], "Secure Notes");
        }

        static IList<string> CreateBankAccount(IList<string> inFields)
        {
            var extra = String.Format("NoteType:Bank Account\nBank Name:{0}\nAccount Type:\nRouting Number:{1}\nAccount Number:{2}\nSWIFT Code:\nIBAN Number:\nPin:{3}\nBranch Address:{4}\nBranch Phone:{5}\nNotes:{6}",
                inFields[0], // Bank Name
                inFields[12], // Routing Number
                inFields[8], // Account Number
                inFields[20], // PIN
                inFields[24], // Address
                inFields[28], // Phone
                inFields[4] // Notes
            );

            return CreateLastPassFields("http://sn", 
                "", "", "", "", extra, inFields[0], "Secure Notes");
        }

        static IList<string> CreateCreditCard(IList<string> inFields)
        {
            var extra = String.Format("NoteType:Credit Card\nName on Card:{0}\nType:\nNumber:{1}\nSecurity Code:{2}\nStart Date:\nExpiration Date:{3}\nNotes:Billing Zip:{4}\nPin:{5}\nPhone:{6}",
                inFields[16], // 0 Name on Card
                inFields[8], // 1 Number
                inFields[20], // 2 Security Code
                inFields[12], // 3 Expiration
                inFields[24], // 4 Billing Zip
                inFields[28], // 5 PIN
                inFields[36] // 6 Phone
            );

            return CreateLastPassFields("http://sn", 
                "", "", "", "", extra, inFields[0], "Secure Notes");
        }

        static IList<string> CreateInsurance(IList<string> inFields)
        {
            var extra = String.Format("NoteType:Insurance\nCompany:{0}\nPolicy Type:\nPolicy Number:{1}\nExpiration:{2}\nAgent Name:\nAgent Phone:{3}\nURL:\nNotes:{4}",
                inFields[0], // 0 Company
                inFields[8], // 1 Policy Number
                inFields[20], // 2 Expiration
                inFields[24], // 3 Phone
                inFields[4] // 4 Notes
            );

            return CreateLastPassFields("http://sn", 
                "", "", "", "", extra, inFields[0], "Secure Notes");
        }

        static IList<string> CreateDriverLicense(IList<string> inFields)
        {
            var extra = String.Format("NoteType:Driver's License\nNumber:{0}\nExpiration Date:{1}\nLicense Class:{2}\nName:\nAddress:\nCity / Town:\nState:{3}\nZIP / Postal Code:\nCountry:\nDate of Birth:\nSex:\nHeight:\nNotes:",
                inFields[8], // 0 License Number
                inFields[16], // 1 Expiration Date
                inFields[20], // 2 Class
                inFields[12] // 3 State
            );

            return CreateLastPassFields("http://sn", 
                "", "", "", "", extra, inFields[0], "Secure Notes");
        }

        static IList<string> CreateNotes(IList<string> inFields)
        {
            return CreateLastPassFields("http://sn", 
                "", "", "", "", inFields[4], inFields[0], "Secure Notes");
        }

        static IList<string> CreateLastPassFields(
            string url, string type, string username, string password, string hostname,
            string extra, string name, string grouping)
        {
            return new string[]
            {
                url, type, username, password, hostname, extra, name, grouping
            };
        }
    }
}
