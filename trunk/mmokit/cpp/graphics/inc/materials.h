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
#ifndef _MATERIALS_H_
#define _MATERIALS_H_

#include <string>
#include <map>

#include "openGL.h"

typedef unsigned int MaterialID;
#define _INVALID_MATERIAL 0xFFFFFFFF

class Material
{
public:
	MaterialID id;

	Material ( float r = 1, float g = 1, float b = 1, float a = 1 );
	Material ( const char* texture, float a = 1 );
	Material ( const Material* mat );
	Material ( const Material& mat );

	void setDiffuse ( float r, float g, float b, float a = 1 );
	void setAmbient ( float r, float g, float b );
	void setSpecular ( float r, float g, float b );
	void setShine ( float s );
	void setTexture ( const char* texture );

	void setGL ( Material *lastMaterial = NULL );
	bool isTextured ( void );

	bool operator == ( const Material &m ) const;

protected:
	int			texutreID;
	GLColor		diffuse;
	GLColor		ambient;
	GLColor		specular;

	float		shine;
};

class MaterialsSystem
{
public:
	static MaterialsSystem& Instance()
	{
		static MaterialsSystem ms;
		return ms;
	} 

	Material* newMaterial ( void );
	Material* newMaterial ( float r = 1.0f , float g = 1.0f , float b = 1.0f , float a = 1.0f );
	Material* newMaterial ( const char* texture, float r, float g, float b, float a = 1.0f );
	Material* newMaterial ( const char* texture, float a = 1.0f );

	Material* newMaterial ( std::string texture, float r, float g, float b, float a = 1.0f );
	Material* newMaterial ( std::string texture, float a = 1.0f );

	Material* newMaterial( const Material &mat );

	Material* getMaterial ( MaterialID id );

	void deleteMaterial ( MaterialID id );

	void setMaterial ( MaterialID id, Material *lastMaterial = NULL );

protected:
	MaterialsSystem();

	MaterialID						lastID;

	typedef std::map<MaterialID,Material*> MaterialMap;
	MaterialMap	materials;

	Material* findMat ( Material* mat );
};
#endif //_MATERIALS_H_

