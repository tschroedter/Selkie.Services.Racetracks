using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks.Windows.Service
{
    [ExcludeFromCodeCoverage]
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            string version = string.Format("{0}.{1}.{2}",
                                           assemblyVersion.Major,
                                           assemblyVersion.Minor,
                                           assemblyVersion.Build);

            serviceInstaller1.DisplayName += " " + version;
            serviceInstaller1.ServiceName += " " + version;
        }

        private void serviceProcessInstaller1_AfterInstall(object sender,
                                                           InstallEventArgs e)
        {
            string path = Context.Parameters [ "assemblypath" ];
            string myAssembly = Path.GetFullPath(path);
            string directoryName = Path.GetDirectoryName(myAssembly);

            if ( directoryName == null )
            {
                throw new NullReferenceException("Could not get directory name for path '{0}'!".Inject(path));
            }

            string logPath = Path.Combine(directoryName,
                                          "Logs");
            Directory.CreateDirectory(logPath);
            ReplacePermissions(logPath,
                               WellKnownSidType.LocalServiceSid,
                               FileSystemRights.FullControl);
        }

        private static void ReplacePermissions(string filepath,
                                               WellKnownSidType sidType,
                                               FileSystemRights allow)
        {
            FileSecurity sec = File.GetAccessControl(filepath);
            var sid = new SecurityIdentifier(sidType,
                                             null);
            sec.PurgeAccessRules(sid); //remove existing
            sec.AddAccessRule(new FileSystemAccessRule(sid,
                                                       allow,
                                                       AccessControlType.Allow));
            File.SetAccessControl(filepath,
                                  sec);
        }
    }
}