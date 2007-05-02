 // database.cpp : Defines the entry point for the console application.
//

#ifndef _DATABASE_H_
#define _DATABASE_H_

#include <stdio.h>
#include <string>
#include <vector>
#include <map>

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

#endif //_DATABASE_H_

