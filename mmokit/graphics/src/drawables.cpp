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
#include "drawables.h"
#include "materials.h"
#include "textures.h"

#define _USE_MATERIALS 0

DrawablesSystem::~DrawablesSystem()
{
}

DrawableID DrawablesSystem::addItem ( int id, DrawableItem* drawable )
{
	std::map<int,DrawablesList>::iterator itr = idList.find(id);
	if (itr == idList.end())
	{
		DrawablesList list;
		idList[id] = list;
		itr = idList.find(id);
	}
	DrawableRecord dr;
	dr.id = lastID++;
	dr.drawable = drawable;

	itr->second.push_back(dr);

	return dr.id;
}

void DrawablesSystem::drawAll ( void )
{
	std::map<int,DrawablesList>::iterator itr = idList.begin();

	TextureSystem	&ts = TextureSystem::Instance();
	MaterialsSystem	&ms = MaterialsSystem::Instance();
	GLColor white(1,1,1,1);

	Material	*lastMat = NULL;
	// if we are doing materials, do 2 passes, one with textures off, one with em on
	if (_USE_MATERIALS)
	{
		glDisable(GL_TEXTURE_2D);
		while (itr != idList.end())
		{
			Material *mat = ms.getMaterial(itr->first);
			if ( mat && mat->isTextured())
				mat->setGL(lastMat);
			else if ( !mat )
				white.setGL();
			else
			{
				itr++;
				continue;
			}

			for ( int i = 0; i < (int)itr->second.size(); i++ )
				itr->second[i].drawable->draw(itr->second[i].id,itr->first);
			
			lastMat = mat;
			itr++;
		}
	}

	// do all the items if we are using flat textures, and only the textured ones if we are doing materials
	itr = idList.begin();
	glEnable(GL_TEXTURE_2D);
	white.setGL();
	lastMat = NULL;

	while (itr != idList.end())
	{
		if (_USE_MATERIALS)
		{
			Material *mat = ms.getMaterial(itr->first);
			if ( !mat || !mat->isTextured())
			{
				itr++;
				continue;
			}
			mat->setGL(lastMat);
			lastMat = mat;
		}
		else
			ts.bind(itr->first);
		
		for ( int i = 0; i < (int)itr->second.size(); i++ )
			itr->second[i].drawable->draw(itr->second[i].id,itr->first);

		itr++;
	}
}

void DrawablesSystem::clearAll ( void )
{
	idList.clear();
	lastID = 0;
}

DrawablesSystem::DrawablesSystem()
{
	clearAll();
}

