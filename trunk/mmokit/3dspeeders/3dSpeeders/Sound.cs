using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

using OpenTK.Audio;

namespace _3dSpeeders
{
    class SoundSystem
    {
        AudioContext context;

        static public SoundSystem instance = new SoundSystem();

        Dictionary<string,int> sounds = new Dictionary<string,int>();
        List<int> channels = new List<int>();

        object BDL = new object();

        bool quit = false;

        public SoundSystem ()
        {
            context = new AudioContext();
        }

        public int addSound ( string path )
        {
            return addSound(new FileInfo(path));
        }

        public int addSound (FileInfo file )
        {
            string name = file.Name;
            if (sounds.ContainsKey(name))
                return sounds[name];

            AudioReader reader = new AudioReader(file.FullName);

            int bufferID = AL.GenBuffer();
            AL.BufferData(bufferID, reader.ReadToEnd());

            lock (BDL)
            {
                sounds.Add(name, bufferID);
            }

            return bufferID;
        }

        int findFreeChannel ()
        {
            lock(BDL)
            {
                foreach (int c in channels)
                {
                    ALSourceState state = AL.GetSourceState(c);
                    if (state == ALSourceState.Stopped)
                        return c;
                }

                int channel = AL.GenSource();
                channels.Add(channel);
                return channel;
            }
        }

        public int playSound ( int soundID )
        {
            if (!AL.IsBuffer(soundID))
                return -1;

            int channel = findFreeChannel();
            AL.SourceStop(channel);


            AL.Source(channel, ALSourcei.Buffer, soundID);


         //   AL.BindBufferToSource(soundID,channel);
            AL.SourcePlay(channel);

            return channel;
        }

        public void update ( )
        {
            lock(BDL)
            {
                foreach (int c in channels)
                {
                    ALSourceState state = AL.GetSourceState(c);
                    if (state == ALSourceState.Stopped)
                    {
                        // do stuff?
                        AL.DeleteSource(c);
                        channels.Remove(c);
                    }
                }

            }
        }

        public void exit ()
        {
            lock(BDL)
            {
                quit = true;
            }
        }

        bool quitinTime()
        {
            lock (BDL)
            {
                return quit;
            }
        }

        public void threadUpdate ()
        {
            lock (BDL)
            {
                quit = false;
            }

            while (!quitinTime())
            {
                update();
                Thread.Sleep(100);
            }
        }

        public void stopAll ()
        {
            lock (BDL)
            {
                foreach (int c in channels)
                {
                    AL.SourceStop(c);
                    AL.DeleteSource(c);
                }
                channels.Clear();
            }
        }

        public void cleanup ()
        {
            lock(BDL)
            {
                foreach (int c in channels)
                {
                    AL.SourceStop(c);
                    AL.DeleteSource(c);
                }

                channels.Clear();

                foreach(KeyValuePair<string,int> sound in sounds)
                {
                    AL.DeleteBuffer(sound.Value);
                }
                sounds.Clear();
            }
        }
    }
}
