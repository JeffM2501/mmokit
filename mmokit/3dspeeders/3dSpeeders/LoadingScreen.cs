using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lidgren;
using Lidgren.Network;

using Axiom;
using Axiom.Core;
using Axiom.Graphics;
using Axiom.Overlays;

namespace _3dSpeeders
{
    public class LoadingScreen
    {
        NetClient client;

        public LoadingScreen (NetClient c)
        {
            client = c;
        }

        public void reload()
        {
            string message = "Please Stand By:";

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

            Overlay o = OverlayManager.Instance.GetByName("3dSpeeders/ConnectionOverlay");
            if (o == null)
                throw new Exception("Could not find overlay named 'Core/DebugOverlay'.");
            o.Show();

            OverlayElement element = OverlayElementManager.Instance.GetElement("3dSpeeders/Connection/StatusLine");
            if (element != null)
                element.Text = message;

        }

        public void unload()
        {
            Overlay o = OverlayManager.Instance.GetByName("3dSpeeders/ConnectionOverlay");
            if (o == null)
                throw new Exception("Could not find overlay named 'Core/DebugOverlay'.");

            o.Dispose();
        }
    }
}
