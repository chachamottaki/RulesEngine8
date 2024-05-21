using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class ConfigFileParsingService
{
    public async Task<Dictionary<string, string>> ParseSettingsFromFileAsync(IFormFile file)
    {
        List<string> temp = new List<string>();
        List<string> tempRelements = new List<string>();
        List<string> tempSelements = new List<string>();
        Dictionary<string, string> settings = new Dictionary<string, string>();

        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            for (string line = await reader.ReadLineAsync(); line != null; line = await reader.ReadLineAsync())
            {
                switch (line)
                {
                    case string s when s.Contains("PROJECTSETTINGS.sLuxHostname"): //needs to be changed to actual param containing deviceID
                        temp.AddRange(line.Split("'"));
                        settings["DeviceID"] = temp[1];
                        temp.Clear();
                        break;
                    /*case string s when s.Contains("PROJECTSETTINGS.typMailSettings.sASMTP"):
                        // Add logic for handling this case
                        break;
                    case string s when s.Contains("PROJECTSETTINGS.typMailSettings.wSMTPPort"):
                        // Add logic for handling this case
                        break;*/
                    case string s when s.Contains("PROJECTSETTINGS.typMailSettings.sMailFrom"):
                        tempRelements.AddRange(line.Split("'"));
                        settings["sender"] = tempRelements[1];
                        break;
                    case string s when s.Contains("PROJECTSETTINGS.typMailSettings.asReceipients"):
                        tempSelements.AddRange(line.Split("'"));
                        settings["recipients"] = tempSelements[1];
                        break;
                    default:
                        break;
                }
            }
        }
        return settings;
    }

    public async Task<(List<Dictionary<string, string>>, List<Dictionary<string, string>>)> ParseDIFromFileAsync(IFormFile file)
    {
        List<string> temp = new List<string>();
        List<Dictionary<string, string>> DI = new List<Dictionary<string, string>>();
        List<Dictionary<string, string>> subDI = new List<Dictionary<string, string>>();
        Dictionary<string, string> currentDict = null;

        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            for (string line = await reader.ReadLineAsync(); line != null; line = await reader.ReadLineAsync())
            {
                if (line.Split('.').Length == 3)
                {
                    string raw_id = string.Format(line.Split(".")[1]);
                    string id = string.Format("DI{0}", ExtractId(raw_id));
                    bool idExists = DI.Any(dict => dict.ContainsKey("id") && dict["id"] == id);

                    if (!idExists)
                    {
                        currentDict = new Dictionary<string, string>();
                        currentDict["id"] = id;
                        DI.Add(currentDict);
                    }
                    else
                    {
                        // Find the dictionary with the corresponding ID
                        currentDict = DI.FirstOrDefault(dict => dict.ContainsKey("id") && dict["id"] == id);
                    }
                    switch (line)
                    {
                        case string s when s.Contains("xExists"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["topIsActive"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("usiIndex"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["TopIndex"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("usiDIIndex"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["DIIndex"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("uiNumChannels"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["NumChannels"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        default:
                            break;
                    }
                }
                else if (line.Split('.').Length >= 4)
                {
                    string raw_id = string.Format("{0}.{1}", line.Split(".")[1], line.Split(".")[2]);
                    string id = string.Format("DI{0}", ExtractId(raw_id));
                    bool idExists = subDI.Any(dict => dict.ContainsKey("id") && dict["id"] == id);

                    if (!idExists)
                    {
                        currentDict = new Dictionary<string, string>();
                        currentDict["id"] = id;
                        subDI.Add(currentDict);
                    }
                    else
                    {
                        currentDict = subDI.FirstOrDefault(dict => dict.ContainsKey("id") && dict["id"] == id);
                    }
                    switch (line)
                    {
                        case string s when s.Contains("xExists"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["isActive"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("sShortDescription"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["shortDescription"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("sLongDescription"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["longDescription"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("xTPST"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["isTPST"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("xMail"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["sendEMail"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("xInvert"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["Invert"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.xIsAlarm"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["IsAlarm"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.eInstallationType"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["InstallationType"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.sInstallationKey"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["InstallationKey"] = temp[1].Trim('\'');
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.sSensorKey"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["SensorKey"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.sSensorType"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["SensorType"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.xSendOnChange"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["SendOnChange"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.xSend"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["Send"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.xError"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["Error"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.sErrorMsg"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["ErrorMsg"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.dtLastTime"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["LastTime"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        return (DI, subDI);
    }


    private string ExtractId(string line)
    {
        Regex regex = new Regex(@"\[(\d+)\]");

        MatchCollection matches = regex.Matches(line);

        if (matches.Count == 2)
        {
            string firstNumber = matches[0].Groups[1].Value;
            string secondNumber = matches[1].Groups[1].Value;
            return $"{firstNumber}.{secondNumber}";
        }
        if (matches.Count == 1)
        {
            string number = matches[0].Groups[1].Value;
            return number;
        }
        else
        {
            return string.Empty;
        }
    }
}


