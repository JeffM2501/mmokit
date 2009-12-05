using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using Lidgren.Network;
using System.Threading;

namespace Hosts
{
    public class CryptoHost : Host
    {
        static int RSA_KEY_MESSAGE = 1;
        static int CRYPTO_SECRET_MESSAGE = 2;
        static int CRYPTO_SECRET_VERIFY = 3;
        static int CRYPTO_ACCEPT = 4;
        static int CRYPTO_DENY = 5;

        protected enum CryptoHostState
        {
            InitalConnect,
            GotSecret,
            Authenticated,
            Invalid,
        }

        protected class CryptoHostConnection
        {
            public string secretVerifyString = string.Empty;
            public CryptoHostState state = CryptoHostState.InitalConnect;

            AesManaged aes;

            public void SetSecret ( byte[] buffer )
            {
                aes = new AesManaged();
                aes.Key = buffer;
                aes.IV = buffer;
            }

            public byte[] EncryptBuffer ( byte[] buffer )
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream encrypt = new CryptoStream(ms,aes.CreateEncryptor(),CryptoStreamMode.Write))
                    {
                        encrypt.Write(buffer, 0, buffer.Length);
                        encrypt.FlushFinalBlock();
                        encrypt.Close();
                        return ms.ToArray();
                    }
                }
            }

            public byte[] DecryptBuffer ( byte[] buffer )
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream decrypt = new CryptoStream(ms, aes.CreateDecryptor(),CryptoStreamMode.Write))
                    {
                        decrypt.Write(buffer, 0, buffer.Length);
                        decrypt.FlushFinalBlock();
                        decrypt.Close();
                        return ms.ToArray();
                    }
                }
            }
        }

        protected Dictionary<NetConnection, CryptoHostConnection> CryptoClients = new Dictionary<NetConnection, CryptoHostConnection>();

        protected RSAParameters RSAKey;
        protected RSACryptoServiceProvider RSA;

        protected NetBuffer RSAKeybuffer;

        public CryptoHost() :base()
        {}

        public CryptoHost( int port ) : base(port)
        {}

        protected override void Init( int port )
        {
            // get the crypto key
            RSA = new RSACryptoServiceProvider();
            RSAKey = RSA.ExportParameters(false);

            XmlSerializer s = new XmlSerializer(typeof(RSAParameters));
            StringWriter writer = new StringWriter();
            s.Serialize(writer, RSAKey);

            RSAKeybuffer = new NetBuffer();
            RSAKeybuffer.Write(RSA_KEY_MESSAGE);
            RSAKeybuffer.Write(writer.ToString());
            base.Init(port);
        }

        protected override void UserConnected(NetConnection sender)
        {
            CryptoHostConnection connection;
            if (CryptoClients.ContainsKey(sender))
                connection = CryptoClients[sender];
            else
            {
                connection = new CryptoHostConnection();
                CryptoClients.Add(sender, new CryptoHostConnection());
            }
            connection.state = CryptoHostState.InitalConnect;

            // send them the rsa key
            server.SendMessage(RSAKeybuffer, sender, NetChannel.ReliableInOrder1);
        }

        protected override void UserDisconnected(NetConnection sender)
        {
            if (CryptoClients.ContainsKey(sender))
                CryptoClients.Remove(sender);
            base.UserDisconnected(sender);
        }

        protected override NetBuffer ProcessInboundMessage(NetConnection from, NetBuffer message)
        {
            CryptoHostConnection connection;
            if (CryptoClients.ContainsKey(from))
                connection = CryptoClients[from];
            else
                return message;

            int code = -1;

            switch (connection.state)
            {
                case CryptoHostState.Authenticated:
                    return DecryptMessage(connection, message);

                case CryptoHostState.Invalid:
                    return null;

                case CryptoHostState.InitalConnect:
                    {
                        // should be the response with a random key encoded with the shit
                        code = message.ReadInt32();
                        if (code == CRYPTO_SECRET_MESSAGE)
                        {
                            connection.state = CryptoHostState.GotSecret;
                            connection.SetSecret(RSA.Decrypt(message.ReadBytes(message.ReadInt32()),false));
                            connection.secretVerifyString = new Random().Next().ToString();

                            // crypto the random string with the secret and send it back
                            NetBuffer b = new NetBuffer();
                            b.Write(CRYPTO_SECRET_VERIFY);
                            byte[] cryptoBuffer = connection.EncryptBuffer(new UTF8Encoding().GetBytes(connection.secretVerifyString));
                            b.Write(cryptoBuffer.Length);
                            b.Write(cryptoBuffer);
                            server.SendMessage(b, from, NetChannel.ReliableInOrder1);
                        }
                        else
                        {
                            NetBuffer errorBuffer = new NetBuffer();
                            errorBuffer.Write(CRYPTO_DENY);
                            errorBuffer.Write("Invalid Secret");
                            server.SendMessage(errorBuffer, from, NetChannel.ReliableInOrder1);
                            from.Disconnect("CryptoError", 1);
                            connection.state = CryptoHostState.Invalid;
                        }
                        return null;
                    }

                case CryptoHostState.GotSecret:
                    {
                        // should be the response with the properly encrypted sample
                        code = message.ReadInt32();
                        if (code == CRYPTO_SECRET_VERIFY)
                        {
                            // set em as real and let the base class call any events it needs to
                            string verify = new UTF8Encoding().GetString(connection.DecryptBuffer(message.ReadBytes(message.ReadInt32())));

                            if (verify == connection.secretVerifyString)
                            {
                                connection.state = CryptoHostState.Authenticated;
                                NetBuffer b = new NetBuffer();
                                b.Write(CRYPTO_ACCEPT);
                                server.SendMessage(b, from, NetChannel.ReliableInOrder1);
                                base.UserConnected(from);
                                return null;
                            }
                        }

                        NetBuffer errorBuffer = new NetBuffer();
                        errorBuffer.Write(CRYPTO_DENY);
                        errorBuffer.Write("Invalid Verify");
                        server.SendMessage(errorBuffer, from, NetChannel.ReliableInOrder1);
                        from.Disconnect("CryptoError", 1);
                        connection.state = CryptoHostState.Invalid;
                        return null;
                    }
            }
            return message;
        }

        protected virtual NetBuffer DecryptMessage ( CryptoHostConnection connection, NetBuffer buffer )
        {
            return new NetBuffer(connection.DecryptBuffer(buffer.ReadBytes(buffer.LengthBytes)));
        }

        protected override NetBuffer ProcessOutboundMessage(NetConnection to, NetBuffer message)
        {
            if (CryptoClients.ContainsKey(to))
            {
                CryptoHostConnection connection = CryptoClients[to];
                if (connection.state == CryptoHostState.Authenticated)
                    return new NetBuffer(connection.EncryptBuffer(message.ReadBytes(message.LengthBytes)));
            }
            return message;
        }
    }
}
