/*
MIT License

Copyright (c) 2017 Rafael cosentino garcia

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

//VersionData V0.0.2
using UnityEngine;

namespace DekosTools.VersionControl
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
                    if (TA != null)
                    {
                        version = TA.text;
                    }
                    else
                    {
                        version = "0.0.0";
                    }
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
                    if (TA != null)
                    {
                        build = TA.text;
                    }
                    else {
                        build = "0000";
                    }
                }
                return build;

            }
        }

    }
}
