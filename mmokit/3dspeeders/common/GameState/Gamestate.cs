using System;
using System.Collections.Generic;
using System.Text;

using World;
using OpenTK.Math;

namespace GameStates
{
    public delegate bool GameStateUpdateCallback(GameStateObject obj);

    public class GameStateUpdateRecord
    {
        public Vector3 Position;
        public Vector3 LinearVelocity;
        public Vector3 Rotation;
        public Vector3 RotaryVelocity;
    }

    public class GameStateObject
    {
        public object Tag;

        public GameStateUpdateCallback UpdateCallback;

        public GameStateUpdateRecord CurrentState;
        public GameStateUpdateRecord LastState;

        public double StartTime = 0;
        public double LastUpdateTime = 0;
        public double LifeTime = 0;

        public GameStateObject (GameStateUpdateCallback cb, object tag, double now)
        {
            UpdateCallback = cb;
            Tag = tag;
            StartTime = now;
            LastUpdateTime = now;
        }

        public void StateUpdate ( GameStateUpdateRecord state, double now )
        {
            LastState = state;
            LastUpdateTime = now;
        }

        public void DRUpdate( double now )
        {
            LifeTime = now - StartTime;
            UpdateCallback(this);
        }
    }

    public class GameState
    {
        public ObjectWorld world;

        public Dictionary<object, GameStateObject> StateObjects = new Dictionary<object, GameStateObject>();

        public GameState (ObjectWorld w)
        {
            world = w;
        }

        public void Add ( object tag, GameStateObject obj )
        {
            if (tag == null || obj == null)
                return;

            StateObjects.Add(tag, obj);
        }

        public void Remove( object tag )
        {
            if (tag == null)
                return;

            if (StateObjects.ContainsKey(tag))
                StateObjects.Remove(tag);
        }
    }
}
