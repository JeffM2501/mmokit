// database.cpp : Defines the entry point for the console application.
//

#include "tcpConnection.h"

#ifndef _WIN32
	#include <unistd.h>
#else
	#include <windows.h>
	#include <time.h>
#endif
#include <stdio.h>
#include <string>
#include <vector>
#include <map>

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

class DatabaseItem
{
public:
	DatabaseItem(std::string _data){data = _data;}
	std::string data;
};

class DatabaseRecord
{
public:
	std::vector<DatabaseItem>	items;
};

class DatabaseTable
{
public:
	std::map<std::string,DatabaseRecord>	records;
	std::vector<std::string>				labels;

	int getLabelIndex ( std::string &label );

	DatabaseRecord* findRecordByKey ( std::string &key );

	DatabaseItem* findRecordItemByKey ( std::string &key, std::string &label );
	DatabaseItem* findRecordItemByKey ( std::string &key, int label );
};

class Database 
{
public:
	Database( const char* filename );
	virtual ~Database();

	bool read ( void );
	bool write ( void );

	DatabaseTable*	getTable ( std::string &table );

	void invalidate ( void  ){revision++;}
protected:
	unsigned int		revision;
	unsigned int		diskRev;
	std::string			diskFile;
	std::map<std::string, DatabaseTable>	tables;
};

int main(int argc, char* argv[])
{
	short	port = 4411;
	int		maxListens = 128;

	float	sleepValue = 0.1f;

	DatabaseServer			serverListener;

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

bool DatabaseServer::connect ( TCPServerConnection *connection, TCPServerConnectedPeer *peer )
{

}

void DatabaseServer::pending ( TCPServerConnection *connection, TCPServerConnectedPeer *peer, unsigned int count )
{
}

void DatabaseServer::disconnect ( TCPServerConnection *connection, TCPServerConnectedPeer *peer, bool forced = false )
{
}


class DatabaseTable
{
public:
	std::map<std::string,DatabaseRecord>	records;
	std::vector<std::string>				labels;

	int getLabelIndex ( std::string &label );

	DatabaseRecord& findRecordByKey ( std::string &key );

	DatabaseItem& findRecordItemByKey ( std::string &key, std::string &label );
	DatabaseItem& findRecordItemByKey ( std::string &key, int label );
};


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


std::string readString ( FILE *fp )
{
	std::string str;

	unsigned short	len = 0;
	fread(&len,sizeof(unsigned short),1,fp);
	if ( len > 0 )
	{
		char *temp = (char)malloc(len+1);
		temp[len] = 0;
		fread(temp,len,1,fp);

		str =  std::string(temp);
		free(temp);
	}
	return str;
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

	fclose(fp)
}

void writeString ( FILE *fp, std::string &str )
{
	unsigned short len = (unsigned short)str.size();
	fwrite(&len,sizeof(unsigned short),1,fp);
	if (len > 0)
		fwrite(str.c_str(),len,1,fp);
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

	std::map<std::string, DatabaseTable>::iterator tableItr = tables.begin();

	while ( tableItr != tables.end() )
	{
		writeString(fp,tableItr->first);

		DatabaseTable &table = tableItr->second;
		
		len = table.labels.size();
		fwrite(&len,sizeof(unsigned int),1,fp);

		for ( unsigned int l = 0; l < table.labels.size();l++ )
			writeString(fp,table.labels[l]);

		len = table.records.size();
		fwrite(&len,sizeof(unsigned int),1,fp);
		
		std::map<std::string,DatabaseRecord>::iterator recordItr = table.records.begin();

		while ( recordItr != table.records.end() )
		{
			if ( recordItr->second.items.size() == table.labels.size() )
			{
				writeString(fp,recordItr->first);

				for ( unsigned int f = 0; f < table.labels.size();f++ )
				recordItr++;
			}
		}


		tableItr++;

	}
}

DatabaseTable* Database::getTable ( std::string &table )
{
	std::map<std::string, DatabaseTable>::iterator itr = tables.find(table);
	if (itr == tables.end())
		return NULL;
	return &(itr->second);
}

