 // database.cpp : Defines the entry point for the console application.
//

#ifndef _DATABASE_H_
#define _DATABASE_H_

#include "databaseMessages.h"
#include "tcpConnection.h"
#include <stdio.h>
#include <string>
#include <vector>
#include <map>

// sleep util
void OSSleep ( float fTime );

class DatabaseItem
{
public:
	DatabaseItem(std::string _data){data = _data;}
	std::string data;
};

typedef std::vector<DatabaseItem> DatabseItemList;

class DatabaseRecord
{
public:
	DatabseItemList	items;
};

typedef std::map<std::string,DatabaseRecord> DatabaseRecordMap;
typedef	std::vector<std::string> DatabaseLabelList;

class DatabaseTable
{
public:
	DatabaseRecordMap	records;
	DatabaseLabelList	labels;

	int getLabelIndex ( std::string &label );

	DatabaseRecord* findRecordByKey ( std::string &key );

	DatabaseItem* findRecordItemByKey ( std::string &key, std::string &label );
	DatabaseItem* findRecordItemByKey ( std::string &key, int label );

	void addRecord ( std::string &key );
	void deleteRecord ( std::string &key );
	void addLabel ( std::string &label, bool unique = true, std::string defaultData = std::string(""));
	void deleteLabel ( std::string &label);
};

typedef std::map<std::string, DatabaseTable> DatabaseTableMap;

class Database 
{
public:
	Database( const char* filename );
	virtual ~Database();

	bool read ( void );
	bool write ( void );

	DatabaseTable*	getTable ( std::string &table );

	DatabaseTable* newTable ( std::string &table );

	void invalidate ( void  ){revision++;}
protected:
	unsigned int		revision;
	unsigned int		diskRev;
	std::string			diskFile;
	DatabaseTableMap	tables;
};

class DatabaseServer : public TCPServerDataPendingListener
{
public:
	virtual bool connect ( TCPServerConnection *connection, TCPServerConnectedPeer *peer );
	virtual void pending ( TCPServerConnection *connection, TCPServerConnectedPeer *peer, unsigned int count );
	virtual void disconnect ( TCPServerConnection *connection, TCPServerConnectedPeer *peer, bool forced = false );

	Database *database;

protected:
	class PeerData
	{
	public:
		bool			verified;
		unsigned char*	leftoverData;
		unsigned int	size;

		PeerData()
		{
			verified = false;
			leftoverData = NULL;
			size = 0;
		}
	};

	PeerData* findPeerData (TCPServerConnectedPeer* peer );

	std::map<TCPServerConnectedPeer*,PeerData> peerData;
	
	bool checkVerified ( TCPServerConnectedPeer *peer );
	unsigned short readStringFromData ( std::string &str, unsigned char* data, unsigned short len );
	void sendShortResult ( TCPServerConnectedPeer *peer, DatabaseResponceCodes code, unsigned short param = 0 );

	// messages
	void handleMessage ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len );

	void handleVerifyMessage ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len );
	void handleTableExists ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len );
	void handleAddTable ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len );
	void handleRecordExists ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len );
	void handleTableFieldList ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len );
	void handleAddTableField ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len );
	void handleRemoveTableField ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len );
	void handleAddRecord ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len );
	void handleGetRecord ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len );
	void handleGetRecordField ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len );
	void handleGetTableKeys ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len );
	void handleRemoveRecord ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len );
	void handleForceShutdown ( TCPServerConnectedPeer *peer, DatabaseMessages code, unsigned char* data, unsigned short len );

};


#endif //_DATABASE_H_

