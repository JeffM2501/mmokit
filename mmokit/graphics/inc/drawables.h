// GPx01
// Copyright (c) 2005 - 2006 Jeff Myers
//
// This package is free software;  you can redistribute it and/or
// modify it under the terms of the license found in the file
// named COPYING that should have accompanied this file.
//
// THIS PACKAGE IS PROVIDED ``AS IS'' AND WITHOUT ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
//
#ifndef _DRAWABLES_H_
#define _DRAWABLES_H_

#include "openGL.h"
#include <map>
#include <vector>

typedef unsigned int DrawableID;

class DrawableItem
{
public:
	virtual ~DrawableItem(){};

	virtual void draw ( DrawableID id, int matID );
};

class DrawablesSystem
{
public:
	static DrawablesSystem& Instance()
	{
		static DrawablesSystem ds;
		return ds;
	} 

	~DrawablesSystem();

	DrawableID addItem ( int id, DrawableItem* drawable );

	void drawAll ( void );
	void clearAll ( void );

protected:
	DrawablesSystem();

	typedef struct 
	{
		DrawableID	id;
		DrawableItem* drawable;
	}DrawableRecord;

	typedef std::vector<DrawableRecord>	DrawablesList;

	std::map<int,DrawablesList>	idList;

	DrawableID lastID;
};

#endif //_DRAWABLES_H_

