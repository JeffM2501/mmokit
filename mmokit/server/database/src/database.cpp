// database.cpp : Defines the entry point for the console application.
//

#include "database.h"

#ifndef _WIN32
	#include <unistd.h>
#else
	#include <windows.h>
	#include <time.h>
#endif

int magicNumber = _MAGIC_DEFAULT_NUMBER;

// simple utils
void writeString ( FILE *fp, const std::string &str )
{
	unsigned short len = (unsigned short)str.size();
	fwrite(&len,sizeof(unsigned short),1,fp);
	if (len > 0)
		fwrite(str.c_str(),len,1,fp);
}

std::string readString ( FILE *fp )
{
	std::string str;

	unsigned short	len = 0;
	fread(&len,sizeof(unsigned short),1,fp);
	if ( len > 0 )
	{
		char *temp = (char*)malloc(len+1);
		temp[len] = 0;
		fread(temp,len,1,fp);

		str =  std::string(temp);
		free(temp);
	}
	return str;
}

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
	PeerData *data = findPeerData(peer);
	if(!data)
		return;

	if ( len < 4 )
		return;

	data->verified = net_Read32(data) == magicNumber;
	
	sendShortResult(peer,data->verified ? eVerifyOk | eVerifyFailed);
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

unsigned short readStringFromData ( std::string &str, unsigned char* data, unsigned short len );
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
		return false;

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
					memcpy(buffer,pos,buffer);
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
		}

		if (end-pos == 0)
		{
			data->leftoverData = NULL;
			data->size = 0;
		}
		else
		{
			data->size = end-pos;
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

	return peerData.erase(itr);
}

// database table

int DatabaseTable::getLabelIndex ( std::string &label )
{
	for ( unsigned int i = 0; i < labels.size(); i++ )
	{
		if ( label == labels[i])
			return (int)i;
	}
	return -1;
}

DatabaseRecord* DatabaseTable::findRecordByKey ( std::string &key )
{
	DatabaseRecordMap::iterator itr = records.find(key);
	if ( itr == records.end() )
		return NULL;

	return &(itr->second);
}

DatabaseItem* DatabaseTable::findRecordItemByKey ( std::string &key, std::string &label )
{
	DatabaseRecord* record = findRecordByKey(key);
	if (!record)
		return NULL;

	int index = getLabelIndex(label);
	if (index < 0)
		return NULL;

	return &(record->items[index]);
}

DatabaseItem* DatabaseTable::findRecordItemByKey ( std::string &key, int label )
{
	DatabaseRecord* record = findRecordByKey(key);
	if (!record)
		return NULL;

	if (label < 0 || label >= (int)record->items.size())
		return NULL;

	return &(record->items[label]);
}

void DatabaseTable::addRecord ( std::string &key )
{
	DatabaseRecord* record = findRecordByKey(key);
	if (record)
		return;

	DatabaseRecord newRecord;

	for ( unsigned int i = 0; i < labels.size(); i++ )
		newRecord.items.push_back(std::string(""));

	records[key] = newRecord;
}

void DatabaseTable::deleteRecord ( std::string &key )
{
	DatabaseRecordMap::iterator itr = records.find(key);
	if ( itr == records.end() )
		return;

	records.erase(itr);
}

void DatabaseTable::addLabel ( std::string &label, bool unique, std::string defaultData )
{
	if (unique)
	{
		for ( unsigned int i = 0; i < labels.size(); i++ )
		{
			if ( label == labels[i] )
				return;
		}
	}
	labels.push_back(label);

	DatabaseRecordMap::iterator itr = records.begin();
	while(itr != records.end())
	{
		itr->second.items.push_back(defaultData);
		itr++;
	}
}

void DatabaseTable::deleteLabel ( std::string &label )
{
	int index = getLabelIndex(label);
	if ( index < 0 )
		return;

	labels.erase(labels.begin()+index);

	DatabaseRecordMap::iterator itr = records.begin();
	while(itr != records.end())
	{
		itr->second.items.erase(itr->second.items.begin()+index);
		itr++;
	}
}

// database class
Database::Database( const char* filename )
{
	revision = diskRev = 0;
	if (filename)
		diskFile = filename;

	read();
}

Database::~Database()
{
	write();
}

bool Database::read ( void )
{
	if (!diskFile.size())
		return false;

	FILE *fp = fopen(diskFile.c_str(),"rb");
	if (!fp)
		return false;

	tables.clear();
	fread(&diskRev,sizeof(unsigned int),1,fp);
	revision = diskRev;

	unsigned int numTables = 0;
	fread(&numTables,sizeof(unsigned int),1,fp);
	if ( numTables == 0 )
	{
		fclose(fp);
		revision = diskRev = 0;
		return false;
	}

	for ( unsigned int i = 0; i < numTables; i++ )
	{
		DatabaseTable	table;
		std::string		tableName = readString(fp);

		unsigned int numLabels = 0;
		fread(&numLabels,sizeof(unsigned int),1,fp);

		if (numLabels > 0)
		{
			for ( unsigned int l = 0; l < numLabels; l++ )
				table.labels.push_back(readString(fp));

			unsigned int numRecords = 0;
			fread(&numRecords,sizeof(unsigned int),1,fp);
			
			if (numRecords > 0)
			{
				for ( unsigned int r = 0; r < numRecords; r++ )
				{
					DatabaseRecord	record;

					std::string key = readString(fp);

					for ( unsigned int f = 0; f < numLabels; f++ )
						record.items.push_back(DatabaseItem(readString(fp)));

					table.records[key] = record;
				}
			}
		}

		tables[tableName] = table;
	}

	fclose(fp);
}

bool Database::write ( void )
{
	if (!diskFile.size())
		return false;

	if (diskRev == revision)
		return true;

	FILE *fp = fopen(diskFile.c_str(),"wb");
	if (!fp)
		return false;

	unsigned int len = (unsigned int)tables.size();
	fwrite(&len,sizeof(unsigned int),1,fp);

	DatabaseTableMap::iterator tableItr = tables.begin();

	while ( tableItr != tables.end() )
	{
		writeString(fp,tableItr->first);

		DatabaseTable &table = tableItr->second;
		
		len = (unsigned int)table.labels.size();
		fwrite(&len,sizeof(unsigned int),1,fp);

		for ( unsigned int l = 0; l < table.labels.size();l++ )
			writeString(fp,table.labels[l]);

		len = (unsigned int)table.records.size();
		fwrite(&len,sizeof(unsigned int),1,fp);
		
		DatabaseRecordMap::iterator recordItr = table.records.begin();

		while ( recordItr != table.records.end() )
		{
			if ( recordItr->second.items.size() == table.labels.size() )
			{
				// key
				writeString(fp,recordItr->first);

				// fields
				for ( unsigned int f = 0; f < table.labels.size();f++ )
					writeString(fp,recordItr->second.items[f].data);
			}
			recordItr++;
		}

		tableItr++;
	}
}

DatabaseTable* Database::getTable ( std::string &table )
{
	DatabaseTableMap::iterator itr = tables.find(table);
	if (itr == tables.end())
		return NULL;
	return &(itr->second);
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
