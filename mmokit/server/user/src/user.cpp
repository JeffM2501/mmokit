// user.cpp : Defines the entry point for the console application.
//

#include "database.h"
#include "tcpConnection.h"
#include "netUtils.h"
#include <vector>
#include <map>
#include <string>

#define _INVALID_ID 0xFFFFFFFF

class UserLoginListener : public TCPServerDataPendingListener
{
public:
	virtual bool connect ( TCPServerConnection *connection, TCPServerConnectedPeer *peer );
	virtual void pending ( TCPServerConnection *connection, TCPServerConnectedPeer *peer, unsigned int count );
	virtual void disconnect ( TCPServerConnection *connection, TCPServerConnectedPeer *peer, bool forced = false );

	Database	*database;

protected:

	class UserPeer : public ShortShortNetworkPeer
	{
	public:
		UserPeer()
		{
			userID = _INVALID_ID;
			peer = NULL;
		}

		UserPeer(TCPServerConnectedPeer*p){ userID = _INVALID_ID; peer = p;}
		unsigned int userID;
		TCPServerConnectedPeer *peer;

		virtual bool messagePending ( unsigned short code, unsigned short len, char *data );

	protected:
		void handleLogin ( unsigned short len, char* data );
	};

	std::map<TCPServerConnectedPeer*,UserPeer> peerMap;
	UserPeer* getPeerData ( TCPServerConnectedPeer* peer )
	{
		std::map<TCPServerConnectedPeer*,UserPeer>::iterator itr = peerMap.find(peer);
		if (itr == peerMap.end())
			return NULL;

		return &itr->second;
	}
};

class TokenRetrevalListener : public TCPServerDataPendingListener
{
public:
	virtual bool connect ( TCPServerConnection *connection, TCPServerConnectedPeer *peer );
	virtual void pending ( TCPServerConnection *connection, TCPServerConnectedPeer *peer, unsigned int count );
	virtual void disconnect ( TCPServerConnection *connection, TCPServerConnectedPeer *peer, bool forced = false );

	Database	*database;

	class TokenPeer : public ShortShortNetworkPeer
	{
	public:
		bool verified;
	};

	std::map<TCPServerConnectedPeer*,TokenPeer> peerMap;
	TokenPeer* getPeerData ( TCPServerConnectedPeer* peer )
	{
		std::map<TCPServerConnectedPeer*,TokenPeer>::iterator itr = peerMap.find(peer);
		if (itr == peerMap.end())
			return NULL;

		return &itr->second;
	}
};

unsigned short userListenPort = 2501;
unsigned short tokenListenPort = 2502;

unsigned int maxTokenListens = 10;
unsigned int maxUserListens = 250;

// utils
// sleep util
void OSSleep ( float fTime )
{
#ifdef _WIN32
	Sleep((DWORD)(1000.0f * fTime));
#else
	usleep((unsigned int )(100000 * fTime));
#endif
}

bool quit = false;
float sleepValue = 1.0f;

// table names commonly used
std::string userDataTable = "user_data";
std::string userNameTable = "user_names";

//record names used
std::string userID = "user_ID";
std::string userName = "user_name";
std::string passwordHash = "password_hash";
std::string verified = "verified";
std::string email = "email";
std::string type = "type";
std::string blocked = "blocked";

// record IDs
unsigned int userDataTableUserNameField = 0;

void setupDatabase ( Database &database )
{
	if ( database.getTable(userDataTable) == NULL )
	{
		DatabaseTable *table = database.newTable(userDataTable);
		table->addLabel(userName);
		table->addLabel(passwordHash);
		table->addLabel(verified);
		table->addLabel(email);
		table->addLabel(type);
		table->addLabel(blocked);
	}

	DatabaseTable *userData = database.getTable(userDataTable);

	userDataTableUserNameField = userData->getLabelIndex(userName);

	// build up a reverse name lookup table
	// also makes it easy to check for used names
	if ( database.getTable(userNameTable) == NULL )
	{
		DatabaseTable *table = database.newTable(userNameTable);

		table->addLabel(userID);
		if ( userData->records.size() )
		{
			DatabaseRecordMap::iterator itr = userData->records.begin();
			while ( itr != userData->records.end() )
			{
				DatabaseRecord *record = table->addRecord(itr->second.items[userDataTableUserNameField].data);
				record->items[0] = itr->first;
			}
		}
	}

	database.write();
}

int main(int argc, char* argv[])
{
	Database	database("characters.dbf");

	TCPConnection			&tcpConnection = TCPConnection::instance();

	TokenRetrevalListener	tokenListener;
	tokenListener.database = &database;
	TCPServerConnection		*tokenServer = tcpConnection.newServerConnection(tokenListenPort,maxTokenListens);	
	tokenServer->addListener(&tokenListener);

	UserLoginListener		userListener;
	userListener.database = &database;
	TCPServerConnection		*userServer = tcpConnection.newServerConnection(userListenPort,maxUserListens);	
	userServer->addListener(&userListener);

	if (!userServer || !tokenServer)
		return -1;

	while (!quit)
	{
		tcpConnection.update();
		OSSleep(sleepValue);
	}

	tcpConnection.kill();

	return 0;
}

bool UserLoginListener::UserPeer::messagePending ( unsigned short code, unsigned short len, char *data )
{
	if (!peer || !data || !len)
		return true;

	//switch(code)
	//{

	//}

	return true;
}

bool UserLoginListener::connect ( TCPServerConnection *connection, TCPServerConnectedPeer *peer )
{
	if (!getPeerData(peer))
		peerMap[peer] = UserPeer(peer);

	return true;
}

void UserLoginListener::pending ( TCPServerConnection *connection, TCPServerConnectedPeer *peer, unsigned int count )
{
	UserPeer	*userPeer = getPeerData(peer);
	if (!userPeer)
		return;

	tvPacketList &packets = peer->getPackets();
	unsigned int packetSize = 0;

	for ( unsigned int i = 0; i < packets.size(); i++ )
		userPeer->receive(packetSize,(char*)packets[i].get(packetSize));
}

void UserLoginListener::disconnect ( TCPServerConnection *connection, TCPServerConnectedPeer *peer, bool forced )
{
	if (getPeerData(peer))
		peerMap.erase(peerMap.find(peer));
}

bool TokenRetrevalListener::connect ( TCPServerConnection *connection, TCPServerConnectedPeer *peer )
{
	return true;
}

void TokenRetrevalListener::pending ( TCPServerConnection *connection, TCPServerConnectedPeer *peer, unsigned int count )
{

}

void TokenRetrevalListener::disconnect ( TCPServerConnection *connection, TCPServerConnectedPeer *peer, bool forced )
{

}



