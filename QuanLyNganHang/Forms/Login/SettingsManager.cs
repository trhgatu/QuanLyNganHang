using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QuanLyNganHang.Forms.Login
{
    public class SettingsManager
    {
        public void SaveLoginCredentials(string username)
        {
            try
            {
                File.WriteAllText(LoginConstants.UserSettingsFile, username);
            }
            catch { /* Handle silently */ }
        }

        public string LoadSavedUsername()
        {
            try
            {
                if (File.Exists(LoginConstants.UserSettingsFile))
                {
                    var userSettings = File.ReadAllLines(LoginConstants.UserSettingsFile);
                    return userSettings.Length > 0 ? userSettings[0] : string.Empty;
                }
            }
            catch { /* Handle silently */ }

            return string.Empty;
        }
    }
}
