using System.Runtime.InteropServices;

namespace Command_Music {
    internal class Statics {

        public static List<string> Shuffle(List<string> array) {
            int n = array.Count();
            Random rand = new Random();
            while (n > 1) {
                int k = rand.Next(n--);
                string temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
            return array;
        }

        public static string buildPath(string windowsPath) {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                return windowsPath;
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                return windowsPath.Replace('\\', '/');
            } else {
                return windowsPath; // not a supported OS for buildPath, we do not need total support
                                    // due to the current deployment plan (closed source, internal use only)
            }
        }



    }
}
