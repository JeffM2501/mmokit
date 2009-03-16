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

        public bool getBool()
        {
            if (data == null || data.Length < 1)
                return false;

            if (type == Type.UByte || type == Type.NBuffer || type == Type.NText)
                return data[0] != '0';

            if (type == Type.Short)
                return getShort() != 0;
            if (type == Type.UShort)
                return getUShort() != 0;
            if (type == Type.Long)
                return getLong() != 0;
            if (type == Type.ULong)
                return getLong() != 0;
            if (type == Type.Float)
                return getFloat() != 0.0f;
            if (type == Type.DFloat)
                return getDouble() != 0.0;

            return false;
        }

        public Byte getByte()
        {
            if (data == null || data.Length < 1)
                return 0;

            if (type == Type.UByte || type == Type.NBuffer || type == Type.NText)
                return data[0];

            if (type == Type.Short)
                return (Byte)getShort();
            if (type == Type.UShort)
                return (Byte)getUShort();
            if (type == Type.Long)
                return (Byte)getLong();
            if (type == Type.ULong)
                return (Byte)getLong();
            if (type == Type.Float)
                return (Byte)getFloat();
            if (type == Type.DFloat)
                return (Byte)getDouble();

            return 0;
        }

        public Int16 getShort()
        {
            if (data == null || data.Length < 1)
                return 0;

            if (type != Type.Short)
                return BitConverter.ToInt16(data,0);

            if (type == Type.NText)
                return Int16.Parse(getString());

            if (type == Type.UByte)
                return (Int16)getByte();
            if (type == Type.UShort)
                return (Int16)getUShort();
            if (type == Type.Long)
                return (Int16)getLong();
            if (type == Type.ULong)
                return (Int16)getLong();
            if (type == Type.Float)
                return (Int16)getFloat();
            if (type == Type.DFloat)
                return (Int16)getDouble();

            return 0;
        }

        public UInt16 getUShort()
        {
            if (data == null || data.Length < 1)
                return 0;

            if (type != Type.UShort)
                return BitConverter.ToUInt16(data, 0);

            if (type == Type.NText)
                return UInt16.Parse(getString());

            if (type == Type.UByte)
                return (UInt16)getByte();
            if (type == Type.Short)
                return (UInt16)getShort();
            if (type == Type.Long)
                return (UInt16)getLong();
            if (type == Type.ULong)
                return (UInt16)getLong();
            if (type == Type.Float)
                return (UInt16)getFloat();
            if (type == Type.DFloat)
                return (UInt16)getDouble();

            return 0;
        }

        public Int32 getLong()
        {
            if (data == null || data.Length < 1)
                return 0;

            if (type != Type.Long)
                return BitConverter.ToInt32(data, 0);

            if (type == Type.NText)
                return Int32.Parse(getString());

            if (type == Type.UByte)
                return (Int32)getByte();
            if (type == Type.Short)
                return (Int32)getShort();
            if (type == Type.UShort)
                return (Int32)getUShort();
            if (type == Type.ULong)
                return (Int32)getULong();
            if (type == Type.Float)
                return (Int32)getFloat();
            if (type == Type.DFloat)
                return (Int32)getDouble();

            return 0;
        }

        public UInt32 getULong()
        {
            if (data == null || data.Length < 1)
                return 0;

            if (type != Type.ULong)
                return BitConverter.ToUInt32(data, 0);

            if (type == Type.NText)
                return UInt32.Parse(getString());

            if (type == Type.UByte)
                return (UInt32)getByte();
            if (type == Type.Short)
                return (UInt32)getShort();
            if (type == Type.UShort)
                return (UInt32)getUShort();
            if (type == Type.Long)
                return (UInt32)getLong();
            if (type == Type.Float)
                return (UInt32)getFloat();
            if (type == Type.DFloat)
                return (UInt32)getDouble();

            return 0;
        }

        public float getFloat()
        {
            if (data == null || data.Length < 1)
                return 0;

            if (type != Type.Float)
                return BitConverter.ToSingle(data, 0);

            if (type == Type.NText)
                return float.Parse(getString());

            if (type == Type.UByte)
                return (float)getByte();
            if (type == Type.Short)
                return (float)getShort();
            if (type == Type.UShort)
                return (float)getUShort();
            if (type == Type.Long)
                return (float)getLong();
            if (type == Type.ULong)
                return (float)getULong();
            if (type == Type.DFloat)
                return (float)getDouble();

            return 0;
        }

        public double getDouble()
        {
            if (data == null || data.Length < 1)
                return 0;

            if (type != Type.DFloat)
                return BitConverter.ToDouble(data, 0);

            if (type == Type.NText)
                return Double.Parse(getString());

            if (type == Type.UByte)
                return (double)getByte();
            if (type == Type.Short)
                return (double)getShort();
            if (type == Type.UShort)
                return (double)getUShort();
            if (type == Type.Long)
                return (double)getLong();
            if (type == Type.ULong)
                return (double)getULong();
            if (type == Type.Float)
                return (double)getFloat();

            return 0;
        }

        public Byte[] getBuffer()
        {
            if ((type == Type.NBuffer || type == Type.NText) && data != null && data.Length < 0)
                return data;

            return (Byte[])data.Clone();
        }

        public String getString()
        {
            if ((type == Type.NBuffer || type == Type.NText) && data != null && data.Length < 0)
                return new UnicodeEncoding().GetString(data);

            if (type == Type.UByte)
                return getByte().ToString();
            if (type == Type.Short)
                return getShort().ToString();
            if (type == Type.UShort)
                return getUShort().ToString();
            if (type == Type.Long)
                return getLong().ToString();
            if (type == Type.ULong)
                return getULong().ToString();
            if (type == Type.Float)
                return getFloat().ToString();
            if (type == Type.DFloat)
                return getDouble().ToString();

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
