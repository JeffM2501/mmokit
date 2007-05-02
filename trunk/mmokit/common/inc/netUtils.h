//netUtils.h
// common utils for TCP networking that does an unsigned short as a message
// followed by an unsigned short for message lenght


#ifndef _NET_UTILS_H_
#define _NET_UTILS_H_

#include "tcpConnection.h"
#include <vector>

class ShortShortNetworkPeer
{
protected:
	typedef struct 
	{
		unsigned short code;
		unsigned short len;
		char* data;
	}PendingMessage;

	std::vector<PendingMessage>	pendingMessages;

	unsigned int size;
	char		 *data;

public:
	virtual ~ShortShortNetworkPeer()
	{
		flushPendingMessages();
	}

	// called when a full message is received, if the derived class clears the message
	// return true and it won't be added to the pending messages
	virtual bool messagePending ( unsigned short code, unsigned short len, char *data )
	{
		return false;
	}
	
	// Add more data to our list of pending data
	void receive ( unsigned int newSize, char* newData )
	{
		// add this new data to our existing data we had left over from last time
		if (newData && newSize)
		{
			char *temp = (char*)malloc(newSize+size);

			if (data)
				memcpy(temp,data,size);

			memcpy(temp+size,newData,newSize);
			if(data)
				delete(data);
			data = temp;
			size += newSize;
		}
		
		// walk this data and see what we can parse out
		if ( data )
		{
			char* pos = data;
			char* end = data+size;

			bool done = false;
			while ( !done )
			{
				if (end-pos >4)	// is there even enough left for a message of 0 size
				{
					PendingMessage message;
					message.code = net_Read16(pos);
					message.len = net_Read16(pos);

					if ( end-pos >= 4 + message.len )
					{
						pos += 4;
						message.data = (char*)malloc(message.len);
						memcpy(message.data,pos,message.len);
						pos += message.len;

						if (messagePending (message.code,message.len,message.data))	// see if our children want to handle it right now
							free(message.data);
						else
							pendingMessages.push_back(message);	// if they don't then we'll keep it till somone does
					}
					else
						done = true;
				}
				else
					done = true; 
			}

			if (end-pos == 0)
			{
				free(data);
				data = NULL;
				size = 0;
			}
			else
			{
				// there was data left over, so lets just keep the
				// part we didnt' read;
				char* temp = data;
				size = (unsigned int)(end-pos);
				data = (char*)malloc(size);
				memcpy(data,pos,size);

				free(temp);
			}
		}
	}

	// returns the number of unhandled pending messags
	unsigned int getPendingMessageCount( void ) 
	{
		return (unsigned int)pendingMessages.size();
	}

	// returns the data pointer for a pending message
	const char* getPendingMessageData ( unsigned int index )
	{
		if ( index >= pendingMessages.size() )
			return NULL;
		return pendingMessages[index].data;
	}

	// returns the size fof a pending message
	unsigned short getPendingMessageSize ( unsigned int index )
	{
		if ( index >= pendingMessages.size() )
			return 0;
		return pendingMessages[index].len;
	}

	// returns the code for a pending message
	unsigned short getPendingMessageCode ( unsigned int index )
	{
		if ( index >= pendingMessages.size() )
			return 0;
		return pendingMessages[index].code;
	}

	// returns all the info for a pending message
	bool getPendingMessage ( unsigned int index, const char **d, unsigned short &c, unsigned short &s )
	{
		if ( index >= pendingMessages.size() )
			return false;

		*d = pendingMessages[index].data;
		c = pendingMessages[index].code;
		s = pendingMessages[index].len;
		return *d != NULL;
	}

	// clears all the pending messages we have left over
	// call this after all messages are handled
	void flushPendingMessages ( void )
	{
		for ( unsigned int i = 0; i < pendingMessages.size(); i++ )
			free(pendingMessages[i].data);

		pendingMessages.clear();
	}
};

#endif //_NET_UTILS_H_