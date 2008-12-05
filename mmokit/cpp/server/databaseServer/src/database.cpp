// database.cpp : Defines the entry point for the console application.
//

#include "database.h"

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

DatabaseRecord* DatabaseTable::addRecord ( std::string &key )
{
	DatabaseRecord* record = findRecordByKey(key);
	if (record)
		return record;

	DatabaseRecord newRecord;

	for ( unsigned int i = 0; i < labels.size(); i++ )
		newRecord.items.push_back(std::string(""));

	records[key] = newRecord;

	return &records[key];
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

bool Database::read ( const char* filename )
{
	if (filename)
		diskFile = filename;

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
	return true;
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
	fclose(fp);
	return true;
}

DatabaseTable* Database::newTable ( std::string &table )
{
	DatabaseTableMap::iterator itr = tables.find(table);
	if (itr != tables.end())
		return &(itr->second);

	tables[table] = DatabaseTable();
	return &tables[table];
}

DatabaseTable* Database::getTable ( std::string &table )
{
	DatabaseTableMap::iterator itr = tables.find(table);
	if (itr == tables.end())
		return NULL;
	return &(itr->second);
}


