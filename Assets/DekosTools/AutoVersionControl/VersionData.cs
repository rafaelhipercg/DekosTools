using UnityEngine;

namespace Dekos.AutoVersionControler
{
    public static class VersionData
    {
        static string version = null;
        public static string Version
        {
            get
            {
                if (version == null)
                {
                    TextAsset TA = Resources.Load("Ver") as TextAsset;
                    version = TA.text;
                }
                return version;

            }
        }

        static string build = null;
        public static string Build
        {
            get
            {
                if (build == null)
                {
                    TextAsset TA = Resources.Load("Build") as TextAsset;
                    version = TA.text;
                }
                return version;

            }
        }

    }
}
