using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCallers
{
    public class CallParam
    {
        public enum Type { Unknown = 0, UByte, Short, UShort, Long, ULong, Float, DFloat, NText, NBuffer };
        Byte[] data;
        Type type = Type.Unknown;

        public UInt16 pack(Byte[] array, UInt16 offset)
        {
            UInt16 i = offset;
            array[i++] = (Byte)type;
            if (type == Type.NText || type == Type.NBuffer)
            {
                foreach(Byte b in BitConverter.GetBytes((UInt16)data.Length))
                    array[i++] = b;
            }

            foreach(Byte b in data)
                array[i++] = b;

            return i;
        }

        public UInt16 unpack(Byte[] array, UInt16 offset)
        {
            if (array.Length <= offset+1)
                return offset;

            UInt16 i = offset;
            type = (Type)array[i++];
            UInt16 size = 0;

            if (type == Type.NText || type == Type.NBuffer)
            {
                if (array.Length <= i+4+1) // is it too small to hold a single byte string/buffer
                    return offset;

                size = BitConverter.ToUInt16(array,i);
                i+=4;
            }
            else
            {
                switch(type)
                {
                    case Type.UByte:
                        size = 1;
                        break;
                    case Type.Short:
                    case Type.UShort:
                        size = 2;
                        break;
                    case Type.Long:
                    case Type.ULong:
                    case Type.Float:
                        size = 4;
                        break;
                    case Type.DFloat:
                        size = 8;
                        break;
                }
            }
            
            if (array.Length < i+size) // is it too small to hold what it says it has
                return offset;

            data = new Byte[size];
            for ( UInt16 p = 0; p < size;p++)
                data[p] = array[i+p];

            i+= size;
            return i;
        }

        public Type getType()
        {
            return type;
        }

        public UInt16 getPackedSize()
        {
            UInt16 size = (UInt16)data.Length;
            size += 1;
            if (type == Type.NText || type == Type.NBuffer)
                size += 4;

            return size;
        }

        public Byte getByte()
        {
            if (type != Type.UByte || data == null || data.Length < 1)
                return 0;

            return data[0];
        }

        public Int16 getShort()
        {
            if (type != Type.Short || data == null || data.Length < 2)
                return 0;

            return BitConverter.ToInt16(data,0);
        }

        public UInt16 getUShort()
        {
            if (type != Type.UShort || data == null || data.Length < 2)
                return 0;

            return BitConverter.ToUInt16(data,0);
        }

        public Int32 getLong()
        {
            if (type != Type.Long || data == null || data.Length < 4)
                return 0;

            return BitConverter.ToInt32(data, 0);
        }

        public UInt32 getULong()
        {
            if (type != Type.ULong || data == null || data.Length < 4)
                return 0;

            return BitConverter.ToUInt32(data, 0);
        }

        public float getFloat()
        {
            if (type != Type.Float || data == null || data.Length < 4)
                return 0;

            return BitConverter.ToSingle(data, 0);
        }

        public double getDouble()
        {
            if (type != Type.DFloat || data == null || data.Length < 8)
                return 0;

            return BitConverter.ToDouble(data, 0);
        }

        public Byte[] getBuffer()
        {
            if ((type == Type.NBuffer || type == Type.NText) && data != null && data.Length < 0)
                return data;

            return new Byte[0];
        }

        public String getString()
        {
            if ((type == Type.NBuffer || type == Type.NText) && data != null && data.Length < 0)
                return new UnicodeEncoding().GetString(data);

            return string.Empty;
        }

        public CallParam( )
        {
            type = Type.Unknown;
        }

        public CallParam( Byte v )
        {
            type = Type.UByte;
            data = new Byte[1];
            data[0] = v;
        }

        public CallParam(Int16 v)
        {
            type = Type.Short;
            data = BitConverter.GetBytes(v);
        }

       public CallParam(UInt16 v)
        {
            type = Type.UShort;
            data = BitConverter.GetBytes(v);
        }

        public CallParam(Int32 v)
        {
            type = Type.Long;
            data = BitConverter.GetBytes(v);
        }

        public CallParam(UInt32 v)
        {
            type = Type.ULong;
            data = BitConverter.GetBytes(v);
        }

        public CallParam(float v)
        {
            type = Type.Float;
            data = BitConverter.GetBytes(v);
        }

        public CallParam(double v)
        {
            type = Type.DFloat;
            data = BitConverter.GetBytes(v);
        }

        public CallParam(string v)
        {
            type = Type.NText;
            data = new UnicodeEncoding().GetBytes(v);
        }

        public CallParam(Byte[] v)
        {
            type = Type.NBuffer;
            data = (Byte[])v.Clone();
        }
    }

    public class CallingParam
    {
        List<CallParam> paramaters = new List<CallParam>();

        public CallingParam ()
        {

        }

        public CallingParam( CallParam[] p )
        {
            foreach (CallParam c in p)
                paramaters.Add(c);

        }

        public CallingParam( Byte[] data )
        {
            unpack(data);
        }

        public int unpack ( Byte[] data )
        {
            UInt16 offset = 0;

            paramaters.Clear();

            while (offset < data.Length)
            {
                CallParam c = new CallParam();

                UInt16 i = c.unpack(data, offset);
                if (i == offset)
                    offset = (UInt16)data.Length; // bad stuff
                else
                {
                    offset = i;
                    add(c);
                }
            }

            return paramaters.Count;
        }

        public Byte[] pack ( )
        {
            UInt16 size = 0;
            foreach (CallParam c in paramaters)
                size += c.getPackedSize();

            Byte[] d = new Byte[size];

            UInt16 offset = 0;
            foreach (CallParam c in paramaters)
                offset = c.pack(d,offset);

            return d;
        }

        public int count ( )
        {
            return paramaters.Count;
        }

        public CallParam get ( int i )
        {
            if (i < 0 || i >= paramaters.Count)
                return new CallParam();
            return paramaters[i];
        }

        public void add ( CallParam p )
        {
            paramaters.Add(p);
        }
    }
}
