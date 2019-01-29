using Pixel_3_Toolkit.Models;
using Pixel_3_Toolkit.Properties;

using System.Diagnostics;
using System.Reflection;

namespace Pixel_3_Toolkit.Util
{
    public sealed class Tools
    {
        private static readonly Tools instance = new Tools();

        public enum VersionCheckResult
        {
            OLDER,
            MATCH,
            NEWER
        }

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Tools()
        {
        }

        private Tools()
        {
            
        }

        public VersionCheckResult CheckVersion(Toolkit toolkit)
        {
            throw new Exceptions.RemoteVersionCodeMalformedException("hello");

            int remoteVersion = toolkit.VersionCode;
            // Determine client version
            int localVersion = int.Parse(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);

            if (remoteVersion > localVersion)
            {
                return VersionCheckResult.NEWER;
            }
            else if (remoteVersion == localVersion)
            {
                return VersionCheckResult.MATCH;
            }
            else if (remoteVersion < localVersion)
            {
                return VersionCheckResult.NEWER;
            }
            else
            {
                throw new Exceptions.RemoteVersionCodeMalformedException(toolkit.VersionCode.ToString());
            }
        }

        // TODO: Make one for the images versioncode

        public static Tools Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
