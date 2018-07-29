using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Google.GData.Spreadsheets;
// for "InsecureSecurityPolicy" class at the end

public class GDocService {

    // grab your spreadsheet's ID / "key" from the URL to access your doc...
    // e.g. everything after "key=" in the URL: https://docs.google.com/spreadsheet/ccc?key=0Ak-N8rbAmu7WdGRFdllybTBIaU1Ic0FxYklIbk1vYlE
    // make sure stop as soon as you hit an ampersand, those are additional URL parameters we don't need
    public static ListFeed GetSpreadsheet(string spreadsheetID) {
        // We need this fake certificate to trick Mono's security to use HTTPS... doesn't work in webplayer's security sandbox
        InsecureSecurityCertificatePolicy.Instate();

        var service = new SpreadsheetsService("UnityConnect");

        var listQuery =
            new ListQuery("https://spreadsheets.google.com/feeds/list/" + spreadsheetID + "/default/public/values");

        return service.Query(listQuery);
    }

}

// from http://answers.unity3d.com/questions/249052/accessing-google-apis-in-unity.html
public class InsecureSecurityCertificatePolicy {

    public static bool Validator(object sender,
                                 X509Certificate certificate,
                                 X509Chain chain,
                                 SslPolicyErrors policyErrors) {
        // Just accept and move on...
        return true;
    }

    public static void Instate() { ServicePointManager.ServerCertificateValidationCallback = Validator; }

}