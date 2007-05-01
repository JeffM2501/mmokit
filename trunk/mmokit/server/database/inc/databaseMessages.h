// databaseMessages.h : Defines the entry point for the console application.
//

#ifndef _DATABASE_MESSAGES_H_
#define _DATABASE_MESSAGES_H_

typedef enum
{
	eNullResponce = 0,
	eVerifyFailed,
	eVerifyOk,
	eGeneralError,
	eNoSuchRecord,
	eRecordValid,
	eNoSuchTable,
	eInvalidTable,
	eTableValid,
	eNoSuchLabel,
	eLabelValid,
	eRecordReturn,
	eFieldReturn,
	eFieldListReturn,
	eKeyListReturn,
	eLastResponce = 0xFFFF
}DatabaseResponceCodes;

typedef enum
{
	eNullMessage = 0,
	eVerifyMessage,
	eTableExists,
	eAddTable,
	eRecordExists,
	eTableFieldList,
	eAddTableField,
	eRemoveTableField,
	eAddRecord,
	eGetRecord,
	eGetRecordField,
	eGetTableKeys,
	eRemoveRecord,
	eForceShutdown,
	eLastMessage = 0xFFFF
}DatabaseMessages;

#define _MAGIC_DEFAULT_NUMBER 10005

#endif //_DATABASE_MESSAGES_H_