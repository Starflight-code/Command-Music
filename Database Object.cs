using Newtonsoft.Json;

namespace Command_Music {
    internal class Database_Object {
        public bool shuffle;
        public bool repeatSong;
        public bool repeatQueue;
        public List<Playlist> playlists = new List<Playlist>();

        public async Task updateSelf() {
            string? json = JsonConvert.SerializeObject(playlists);
            if (json == null) { return; }
            await File.WriteAllTextAsync(Statics.buildPath(Directory.GetCurrentDirectory() + "\\data.db"), json);
        }
        public async Task populateSelf() {
            string json = await File.ReadAllTextAsync(Statics.buildPath(Directory.GetCurrentDirectory() + "\\data.db"));
            List<Playlist>? list = JsonConvert.DeserializeObject<List<Playlist>>(json);
            if (list == null) { return; }
            playlists.AddRange(list);
        }

        public Playlist? findPlaylist(string name) {
            for (int i = 0; i < playlists.Count(); i++) {
                if (playlists[i].name.ToLower() == name.ToLower()) {
                    return playlists[i];
                }
            }
            return null;
        }

    }
    internal class Playlist {
        public string name;
        public List<string> directories;
        public List<string> songs;
        public Playlist(string name) {
            directories = new List<string>();
            songs = new List<string>();
            this.name = name;
        }
        public void addSong(string songPath) {
            songs.Add(songPath);
        }
        public void addSong(List<string> songPaths) {
            songs.AddRange(songPaths);
        }

        public void addDirectory(string directoryPath) {
            directories.Add(directoryPath);
        }

        public void addDirectory(List<string> directoryPaths) {
            directories.AddRange(directoryPaths);
        }
    }
}
