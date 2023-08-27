using NAudio.Wave;

namespace Command_Music {
    internal class Program {
        Command_Parser parser = new Command_Parser();
        Database_Object data = new Database_Object();
        List<string> musicQueue = new List<string>();
        int iter = 0;
        WaveOutEvent outputDevice = new WaveOutEvent();
        public static Task Main(string[] args) => new Program().MainAsync(args);

        public void registerCommands() {
            parser.registerCommand("volume", Lambdas.volume);
            parser.registerAlias("volume", "vol");
            parser.registerCommand("resume", Lambdas.resume);
            parser.registerAlias("resume", "res");
            parser.registerCommand("pause", Lambdas.pause);
            parser.registerAlias("pause", "pau");
        }
        public void queueDirectory(string directory, bool shuffle = false) {
            var filesRaw = Directory.EnumerateFiles(directory).ToList();
            for (int i = 0; i < filesRaw.Count(); i++) {
                if (filesRaw[i].EndsWith(".mp3") || filesRaw[i].EndsWith(".wav")) {
                    musicQueue.Add(filesRaw[i]);
                }
            }
            if (shuffle) {
                musicQueue = Statics.Shuffle(musicQueue);
            }
        }
        public void shuffleQueue() {
            musicQueue = Statics.Shuffle(musicQueue);
        }

        public void queueNext(string songPath) {
            musicQueue.Insert(iter + 1, songPath);
        }

        public void queueLast(string songPath) {
            musicQueue.Add(songPath);
        }

        public void queuePlaylist(Playlist songs, bool shuffle = false) {
            for (int i = 0; i < songs.directories.Count(); i++) {
                queueDirectory(songs.directories[i]);
            }
            for (int i = 0; i < songs.songs.Count(); i++) {
                queueLast(songs.songs[i]);
            }
            if (shuffle) {
                shuffleQueue();
            }
        }

        public async Task MainAsync(string[] args) {
            registerCommands();
            Console.WriteLine("Program Started");

            await data.populateSelf();
            var playlist = data.findPlaylist("anime");
            if (playlist != null) {
                queuePlaylist(playlist, true);
            }
            outputDevice.Init(new AudioFileReader((musicQueue[iter++])));
            outputDevice.Play();
            outputDevice.PlaybackStopped += Player_PlaybackFinished;

            while (true) {
                //Console.Clear();
                Console.Write("<Command Music>: ");
                parser.processInput(Console.ReadLine(), outputDevice, data);
                Console.WriteLine();
            }
            //await Task.Delay(-1);
        }


        // autoplay system, once file playback is finished it will play the next file
        private void Player_PlaybackFinished(object? sender, EventArgs e) {
            Console.WriteLine("File Finished");
            if (musicQueue == null) {
                return;
            }
            if (iter == musicQueue.Count() - 1) {
                if (data.repeatQueue) {
                    iter = 0;
                } else {
                    return;
                }
            }
            outputDevice.Init(new AudioFileReader((musicQueue[iter++])));
            outputDevice.Play();

        }
    }
}