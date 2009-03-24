using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using OpenTK.Audio;

namespace _3dSpeeders
{
    public class Channel
    {
        public int id = -1;
        public int source = -1;
        public bool enabled = false;
        public bool looped = false;
        public float gain = 0;

        public Channel (int id)
        {
            source = id;
            enabled = true;
        }

        public void dispose ()
        {
            AL.SourceStop(source);
            AL.DeleteSource(source);

            source = -1;
            enabled = false;
        }

        public void stop ()
        {
            if (enabled)
                AL.SourceStop(source);

            enabled = false;
        }
        
        public void play ( int buffer )
        {
            volume(1.0f);
            AL.Source(source, ALSourcei.Buffer, buffer);
            AL.SourcePlay(source);
        }

        public void volume ( float vol )
        {
            gain = vol;
            AL.Source(source, ALSourcef.Gain, vol);
        }
    }

    class SoundSystem
    {
        AudioContext context;

        static public SoundSystem instance = new SoundSystem();

        Dictionary<string,int> sounds = new Dictionary<string,int>();
        List<Channel> channels = new List<Channel>();

        object BDL = new object();

        bool quit = false;

        float volume = 0.5f;

        public SoundSystem ()
        {
            context = new AudioContext();
        }

        public void setMasterVolume( float vol )
        {
            volume = vol;
            AL.Listener(ALListenerf.Gain, vol);
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

        Channel findFreeChannel()
        {
            lock(BDL)
            {
                foreach (Channel c in channels)
                {
                    if (!c.enabled)
                        return c;

                    ALSourceState state = AL.GetSourceState(c.source);
                    if (state == ALSourceState.Stopped)
                    {
                        c.enabled = false;
                        return c;
                    }
                }

                Channel chan = new Channel(AL.GenSource());
                chan.id = channels.Count;
                channels.Add(chan);
                return chan;
            }
        }


        // non blocking for use inside the thread (for repeats and playlists, etc..)
        int playSampleInNewChannel (int soundID)
        {
            Channel channel = findFreeChannel();

            channel.stop();
            channel.play(soundID);

            return channel.id;
        }

        public int playSound (int soundID)
        {
            lock(BDL)
            {
                if (!AL.IsBuffer(soundID))
                    return -1;

                return playSampleInNewChannel(soundID);
            }
        }

        Channel getChannel ( int id )
        {
            if (id < 0 || id >= channels.Count)
                return null;

            return channels[id];
        }

        public bool stopChannel ( int channel )
        {
            lock(BDL)
            {
                Channel chan = getChannel(channel);
                if (chan == null)
                    return false;

                chan.stop();
            }
            return true;
        }

        public bool setChannelVolume ( int channel, float vol )
        {
            lock (BDL)
            {
                Channel chan = getChannel(channel);
                if (chan == null)
                    return false;

                chan.volume(vol);
            }
            return true;
        }

        public void update ( )
        {
            lock(BDL)
            {
                context.Process();

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
                foreach (Channel c in channels)
                {
                    c.dispose();
                }
                channels.Clear();
            }
        }

        public void cleanup ()
        {
            lock(BDL)
            {
                foreach (Channel c in channels)
                {
                    c.dispose();
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
