// database.cpp : Defines the entry point for the console application.
//

#include "tcpConnection.h"

#ifndef _WIN32
	#include <unistd.h>
#else
#include <windows.h>
	#include <time.h>
	#include <stdio.h>
#endif

bool quit = false;

// sleep util
void OSSleep ( float fTime )
{
#ifdef _WIN32
	Sleep((DWORD)(1000.0f * fTime));
#else
	usleep((unsigned int )(100000 * fTime));
#endif
}

class DatabaseServer : public TCPServerDataPendingListener
{
public:
	virtual bool connect ( TCPServerConnection *connection, TCPServerConnectedPeer *peer );
	virtual void pending ( TCPServerConnection *connection, TCPServerConnectedPeer *peer, unsigned int count );
	virtual void disconnect ( TCPServerConnection *connection, TCPServerConnectedPeer *peer, bool forced = false );
};

int main(int argc, char* argv[])
{
	short	port = 4411;
	int		maxListens = 128;

	float sleepValue = 0.1f;

	DatabaseServer			serverListener;

	TCPConnection			&tcpConnection = TCPConnection::instance();
	TCPServerConnection		*tcpServer = tcpConnection.newServerConnection(port,maxListens);	

	if (!tcpServer)
		return -1;

	tcpServer->addListener(&serverListener);

	while (!done)
	{

	}

	return 0;
}

