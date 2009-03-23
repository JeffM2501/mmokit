using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;

using Lidgren;
using Lidgren.Network;

namespace _3dSpeeders
{
    public class LoadingScreen : Scene
    {
        NetClient client;

        string message;

        TextPrinter printer = new TextPrinter(TextQuality.High);
        Font sans_serif = new Font(FontFamily.GenericSansSerif, 32.0f);

        public LoadingScreen (NetClient c)
        {
            client = c;
        }

        public void reload()
        {
            message = "Please Stand By:";

            if (client.Status == NetConnectionStatus.Connecting)
                message += "Connecting to ";
            else if (client.Status == NetConnectionStatus.Connected)
                message += "Connected to ";
            else if (client.Status == NetConnectionStatus.Disconnecting)
                message += "Disconnecting from ";
            else if (client.Status == NetConnectionStatus.Disconnected)
                message += "Disconnected from ";
            else if (client.Status == NetConnectionStatus.Reconnecting)
                message += "Reconnecting to ";
            else
                message += "Unknown status for ";

            message += client.ServerConnection.RemoteEndpoint.ToString();
        }

        public override void draw(RenderStateArgs e)
        {
            base.draw(e);

            printer.Begin();
            printer.Print(message, sans_serif, Color.Wheat, new RectangleF(0, 0, e.x, e.y), TextPrinterOptions.Default, TextAlignment.Center);

            printer.End();
        }

        public void unload()
        {
        }
    }
}
