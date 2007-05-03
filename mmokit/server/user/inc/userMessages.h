// userMessages.h

#ifndef _USER_MESSAGES_H_
#define _USER_MESSAGES_H_

typedef enum
{
	eNullUserNetResponce = 0,
	eNoUsernameUserNetResponce,
	eInvalidPasswordUserNetResponce,
	ePasswordOKUserNetResponce,
	eGeneralErrorUserNetResponce,
	eLastUserNetResponce = 0xFFFF
}UseNetResponces;

typedef enum
{
	eNullUserNetMessage = 0,
	eUserLoginNetMessage,
	eNewUserNetMessage,
	eLastUserNetMessage = 0xFFFF
}UseNetMessages;

#endif //_USER_MESSAGES_H_