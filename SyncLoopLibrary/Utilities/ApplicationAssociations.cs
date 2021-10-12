using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.IO;

namespace SyncLoopLibrary
{
    /// <summary>
    /// General utilities class.
    /// </summary>
    public static partial class Utilities
    {
        /// <summary>
        /// External call to notify icon refresh needed.
        /// </summary>
        /// <param name="wEventId"></param>
        /// <param name="uFlags"></param>
        /// <param name="dwItem1"></param>
        /// <param name="dwItem2"></param>
        [DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);


        /// <summary>
        /// Check if file extension is not already associated.
        /// </summary>
        /// <returns>True if not associated, false otherwise.</returns>
        public static bool IsNotAssociated()
        {
            return Registry.CurrentUser.OpenSubKey("Software\\Classes\\.syncloop", false) == null;
        }


        /// <summary>
        /// Associates file extension and icon.
        /// </summary>
        /// <param name="iconPath">Path to icon file.</param>
        [Obsolete]
        public static void Associate(string iconPath)
        {
            // Get assembly location.
            var wpfAssembly = (AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(item => item.EntryPoint != null)
                .Select(item =>
                new { item, applicationType = item.GetType(item.GetName().Name + ".App", false) })
                .Where(a => a.applicationType != null && typeof(System.Windows.Application)
                .IsAssignableFrom(a.applicationType))
                .Select(a => a.item))
                .FirstOrDefault();

            

            RegistryKey fileReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\.syncloop");
            RegistryKey appReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\Applications\\SyncLoop.exe");
            RegistryKey appAssociation = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.syncloop");

            fileReg.CreateSubKey("DefaultIcon").SetValue("", iconPath);
            fileReg.CreateSubKey("PerceivedType").SetValue("", "SyncLoop Project");

            appReg.CreateSubKey("shell\\open\\command").SetValue("", "\"" + wpfAssembly.Location + "\" " + "\"%1\"");
            appReg.CreateSubKey("DefaultIcon").SetValue("", $"{wpfAssembly.Location},0");

            appAssociation.CreateSubKey("UserChoice").SetValue("Progid", "syncloopdoc");
            appAssociation.CreateSubKey("OpenWithProgids").SetValue("syncloopdoc", "0");
            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }


        /// <summary>
        /// Associates file type and icon.
        /// </summary>
        public static void Create_Sync_FileAssociation()
        {

            // Get assembly location.
            var wpfAssembly = (AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(item => item.EntryPoint != null)
                .Select(item =>
                new { item, applicationType = item.GetType(item.GetName().Name + ".App", false) })
                .Where(a => a.applicationType != null && typeof(System.Windows.Application)
                .IsAssignableFrom(a.applicationType))
                .Select(a => a.item))
                .FirstOrDefault();

            string appFolder = Path.GetDirectoryName(wpfAssembly.Location);

            string iconFile = Path.Combine(appFolder, "files.ico");

            // Key1: Create ".synclopp" entry.
            Microsoft.Win32.RegistryKey key1 = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", true);

            key1.CreateSubKey("Classes");
            key1 = key1.OpenSubKey("Classes", true);

            key1.CreateSubKey(".syncloop");
            key1 = key1.OpenSubKey(".syncloop", true);
            key1.SetValue("", "DemoKeyValue"); // Set default key value

            key1.Close();

            // Key2: Create "DemoKeyValue\DefaultIcon" entry.
            Microsoft.Win32.RegistryKey key2 = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", true);

            key2.CreateSubKey("Classes");
            key2 = key2.OpenSubKey("Classes", true);

            key2.CreateSubKey("DemoKeyValue");
            key2 = key2.OpenSubKey("DemoKeyValue", true);

            key2.CreateSubKey("DefaultIcon");
            key2 = key2.OpenSubKey("DefaultIcon", true);
            key2.SetValue("", "\"" + iconFile + "\""); // Set default key value

            key2.Close();

            /**************************************************************/
            /**** Key3: Create "DemoKeyValue\shell\open\command" entry ****/
            /**************************************************************/
            Microsoft.Win32.RegistryKey key3 = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", true);

            key3.CreateSubKey("Classes");
            key3 = key3.OpenSubKey("Classes", true);

            key3.CreateSubKey("DemoKeyValue");
            key3 = key3.OpenSubKey("DemoKeyValue", true);

            key3.CreateSubKey("shell");
            key3 = key3.OpenSubKey("shell", true);

            key3.CreateSubKey("open");
            key3 = key3.OpenSubKey("open", true);

            key3.CreateSubKey("command");
            key3 = key3.OpenSubKey("command", true);
            key3.SetValue("", "\"" + wpfAssembly.Location + "\"" + " \"%1\""); // Set default key value

            key3.Close();

            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);

        }

    }
}
