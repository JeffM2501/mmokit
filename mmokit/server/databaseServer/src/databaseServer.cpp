// databaseServer.cpp : Defines the entry point for the console application.
//

#include "databaseServer.h"

#ifndef _WIN32
	#include <unistd.h>
#else
	#include <windows.h>
	#include <time.h>
#endif

int magicNumber = _MAGIC_DEFAULT_NUMBER;


bool quit = false;

int main(int argc, char* argv[])
{
	short	port = 4411;
	int		maxListens = 128;

	float	sleepValue = 0.1f;

	Database				database("database.db");

	DatabaseServer			serverListener;
	serverListener.database = &database;

	TCPConnection			&tcpConnection = TCPConnection::instance();
	TCPServerConnection		*tcpServer = tcpConnection.newServerConnection(port,maxListens);	

	if (!tcpServer)
		return -1;

	tcpServer->addListener(&serverListener);

	while (!quit)
	{
		tcpConnection.update();
		OSSleep(sleepValue);
	}

	return 0;
}

DatabaseServer::PeerData* DatabaseServer::findPeerData (TCPServerConnectedPeer* peer )
{
	std::map<TCPServerConnectedPeer*,PeerData>::iterator itr = peerData.find(peer);
	if (itr == peerData.end())
		return NULL;

	return &(itr->second);
}

bool DatabaseServer::connect ( TCPServerConnection *connection, TCPServerConnectedPeer *peer )
{
	if(!database)
		return false;

	if (!findPeerData(peer))
	{
		PeerData data;
		peerData[peer] = data;
	}

	PeerData *data = findPeerData(peer);

	data->verified = false;

	// ask for the magic key
	unsigned char message[4];
	net_Write16(eVerifyMessage,message);
	net_Write16(4,message+2);
	peer->sendData(message,sizeof(unsigned short));

	return true;
}
void DatabaseServer::handleVerifyMessage ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len )
{
	PeerData *peerData = findPeerData(peer);
	if(!peerData ||!data)
		return;

	if ( len < 4 )
		return;

	peerData->verified = net_Read32(data) == magicNumber;
	
	DatabaseResponceCodes responce = eVerifyOk;
	if ( !peerData->verified )
		responce = eVerifyFailed;
	sendShortResult(peer,responce);
}


bool DatabaseServer::checkVerified ( TCPServerConnectedPeer *peer )
{
	PeerData *data = findPeerData(peer);
	if(!data)
		return false;

	if (!data->verified)
	{
		sendShortResult(peer,eVerifyFailed);
		return false;
	}
	return true;
}

unsigned short DatabaseServer::readStringFromData ( std::string &str, unsigned char* data, unsigned short len )
{
	str = "";

	if ( len < 2 )
		return len;

	unsigned short temp16 = net_Read16(data);
	
	if ( temp16 == 0 )
		return 2;

	char *temp = (char*)malloc(temp16+1);
	temp[temp16] = 0;

	memcpy(temp,data,temp16);
	str = temp;
	free (temp);

	return temp16+2;
}

void DatabaseServer::sendShortResult ( TCPServerConnectedPeer *peer, DatabaseResponceCodes code, unsigned short param )
{
	char buffer[6] = {0};

	net_Write16((unsigned short)code,buffer);
	net_Write16(6,buffer+2);
	net_Write16(param,buffer+4);
	peer->sendData(buffer,6);
}

void DatabaseServer::handleTableExists ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len )
{
	if(!checkVerified(peer))
		return;

	if ( len < 5 )
		return;
	
	unsigned short param = net_Read16(data);
	unsigned short pos = 2;

	std::string table;
	pos += readStringFromData(table,data+pos,len-pos);

	DatabaseTable *dbTable = database->getTable(table);
	if (!table.size() || !dbTable)
	{
		sendShortResult(peer,eNoSuchTable,param);
		return;
	}

	sendShortResult(peer,eTableValid,param);
}

void DatabaseServer::handleAddTable ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len )
{
	if(!checkVerified(peer))
		return;

	if ( len < 7 )
		return;

	unsigned short param = net_Read16(data);
	unsigned short pos = 2;

	std::string table;
	pos += readStringFromData(table,data+pos,len-pos);
	
	unsigned short lablels = net_Read16(data+pos);
	pos += 2;

	DatabaseTable *dbTable = database->getTable(table);
	if (!table.size() || dbTable)
	{
		sendShortResult(peer,eInvalidTable,param);
		return;
	}

	dbTable = database->newTable(table);
	if ( lablels > 0 )
	{
		for ( unsigned int i = 0; i < lablels; i++ )
		{
			std::string label;
			pos += readStringFromData(label,data+pos,len-pos);
			std::string defaultData;
			pos += readStringFromData(defaultData,data+pos,len-pos);
			dbTable->addLabel(label,false,defaultData);
		}
	}
	sendShortResult(peer,eTableValid,param);
}

void DatabaseServer::handleRecordExists ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len ){}
void DatabaseServer::handleTableFieldList ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len ){}
void DatabaseServer::handleAddTableField ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len ){}
void DatabaseServer::handleRemoveTableField ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len ){}
void DatabaseServer::handleAddRecord ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len ){}
void DatabaseServer::handleGetRecord ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len ){}
void DatabaseServer::handleGetRecordField ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len ){}
void DatabaseServer::handleGetTableKeys ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len ){}
void DatabaseServer::handleRemoveRecord ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len ){}
void DatabaseServer::handleForceShutdown ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len ){}


void DatabaseServer::handleMessage ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len )
{
	switch(code)
	{
		case eVerifyMessage:
			handleVerifyMessage(peer,code,data,len);
			break;
		default:
			break;
	}
}

void DatabaseServer::pending ( TCPServerConnection *connection, TCPServerConnectedPeer *peer, unsigned int count )
{
	if(!database)
		return;

	PeerData *data = findPeerData(peer);
	if(!data)
		return;

	// build up the megga message

	unsigned char* blob = data->leftoverData;
	unsigned int totalSize = data->size;

	tvPacketList &packets = peer->getPackets();
	for (unsigned int i = 0; i < packets.size(); i++ )
	{
		unsigned int packetSize;
		unsigned char* packetData = packets[i].get(packetSize);
		unsigned char* temp = (unsigned char*)malloc(packetSize + totalSize);

		if (blob)
			memcpy(temp,blob,totalSize);
		memcpy(temp+totalSize,packetData,packetSize);
		if(blob)
			delete(blob);
		blob = temp;
		totalSize += packetSize;
	}

	// parse any messages we have left;

	if (blob)
	{
		unsigned char* pos = blob;
		unsigned char* end = blob+totalSize;

		bool done = false;
		while ( !done )
		{
			if (end-pos >4)
			{
				unsigned short temp16 = net_Read16(pos);
				DatabaseMessages	code = (DatabaseMessages)temp16;
				unsigned short len = net_Read16(pos);

				if ( end-pos >= 4 + len )
				{
					pos += 4;
					unsigned char* buffer = (unsigned char*)malloc(len);
					memcpy(buffer,pos,len);
					pos += len;

					handleMessage(peer,code,buffer,len);
					free(buffer);
				}
				else
					done = true;
			}
			else
				done = true; 
		}

		if (end-pos == 0)
		{
			data->leftoverData = NULL;
			data->size = 0;
		}
		else
		{
			data->size = (unsigned int)(end-pos);
			data->leftoverData = (unsigned char*)malloc(data->size);
			memcpy(data->leftoverData,pos,data->size);
		}

		free(blob);
	}
}

void DatabaseServer::disconnect ( TCPServerConnection *connection, TCPServerConnectedPeer *peer, bool forced  )
{
	if(!database)
		return;

	std::map<TCPServerConnectedPeer*,PeerData>::iterator itr = peerData.find(peer);
	if (itr == peerData.end())
		return;

	peerData.erase(itr);
}


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
