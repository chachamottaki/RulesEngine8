using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
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
                    case string s when s.Contains("PROJECTSETTINGS.sLuxHostname"):
                        temp.AddRange(line.Split("'"));
                        settings["DeviceID"] = temp[1];
                        temp.Clear();
                        break;
                    case string s when s.Contains("PROJECTSETTINGS.typMailSettings.sASMTP"):
                        // Add logic for handling this case
                        break;
                    case string s when s.Contains("PROJECTSETTINGS.typMailSettings.wSMTPPort"):
                        // Add logic for handling this case
                        break;
                    case string s when s.Contains("PROJECTSETTINGS.typMailSettings.sMailFrom"):
                        tempRelements.AddRange(line.Split("'"));
                        settings["recipients"] = tempRelements[1];
                        break;
                    case string s when s.Contains("PROJECTSETTINGS.typMailSettings.asReceipients"):
                        tempSelements.AddRange(line.Split("'"));
                        settings["sender"] = tempSelements[1];
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
                    string id = line.Split(".")[1];
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
                                currentDict["xExists"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("usiIndex"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["usiIndex"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("usiDIIndex"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["usiDIIndex"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("uiNumChannels"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["uiNumChannels"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        default:
                            break;
                    }
                }
                else if (line.Split('.').Length >= 4)
                {
                    string id = string.Format("{0}.{1}", line.Split(".")[1], line.Split(".")[2]);
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
                                currentDict["xExists"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("sShortDescription"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["sShortDescription"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("sLongDescription"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["sLongDescription"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("xTPST"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["xTPST"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("xMail"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["xMail"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("xInvert"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["xInvert"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.xIsAlarm"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["typTPST.xIsAlarm"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.eInstallationType"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["typTPST.eInstallationType"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.sInstallationKey"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["typTPST.sInstallationKey"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.sSensorKey"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["typTPST.sSensorKey"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.sSensorType"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["typTPST.sSensorType"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.xSendOnChange"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["typTPST.xSendOnChange"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.xSend"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["typTPST.xSend"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.xError"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["typTPST.xError"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.sErrorMsg"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["typTPST.sErrorMsg"] = temp[1];
                            }
                            temp.Clear();
                            break;
                        case string s when s.Contains("typTPST.dtLastTime"):
                            temp.AddRange(line.Split(":="));
                            if (currentDict != null)
                            {
                                currentDict["typTPST.dtLastTime"] = temp[1];
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

}


