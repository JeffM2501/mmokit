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
#ifndef _TEXTURES_H_
#define _TEXTURES_H_

#include <string>
#include <map>

#include "openGL.h"

class TextureImage
{
public:
	TextureImage();
	~TextureImage();

	int				size[2];
	unsigned char	*data;
	int				bpp;
};

class TextureImageLoader
{
public:
	virtual ~TextureImageLoader(){};

	virtual bool load ( const char* filename, TextureImage& image, bool infoOnly = false ) = 0;
	virtual bool saveScreen ( const char* filename ) = 0;
};

class TextureSystem
{
public:

	float setUnloadTime ( float time ){ unloadTime = time; }
	void checkUnloads ( void );

	static TextureSystem& Instance()
	{
		static TextureSystem tm;
		return tm;
	}

	~TextureSystem();

	int getID ( std::string texture );
	int getID ( const char* texture ){return getID(std::string(texture));}

	bool bind ( int id );

	void invalidate ( void );

	void flush ( void );

	void resetBind ( void );

	// utility functions
	void drawImage ( int id, eAlignment align = eCenter, float scale = 1.0f );
	void drawImage ( int id, eAlignment align, float x, float y );
	void saveScreenshot ( const char* fileName );

	int	getImageWidth ( int id );
	int	getImageHeight( int id );

	// timing functions
	int getBinds ( void ){return binds;}
	void resetBinds ( void ){binds=0;}

	// image loader
	bool setImageLoader ( TextureImageLoader *loader ) {
	  if (loader) {
	    imageLoader = loader;
	    return true;
	  }
	  return false;
	}
protected:

	TextureSystem();

	int lastID;
	std::map<std::string, int> textureNameMap;

	typedef struct 
	{
		std::string textureFilePath;
		int			id;
		int			x;
		int			y;
		std::string name;
		unsigned int boundID;
		float		lastUsedTime;
	}trTextureInfo;

	std::map<int,trTextureInfo> textureMap; 

	trTextureInfo* getTexture ( int id );

	int loadTexture ( trTextureInfo &info );

	unsigned int bindTexture ( trTextureInfo &info );

	std::string pathFromName ( std::string name );

	bool bindRawTexture ( trTextureInfo *texture );

	unsigned int lastBoundTexture;

	int	binds;

	TextureImageLoader	*imageLoader;

	float				unloadTime;
};

#endif //_TEXTURES_H_

