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

#include "openGL.h"
#include "textures.h"
#include "TextUtils.h"
#include "OSFile.h"
#include "TextUtils.h"
#include <stdio.h>
#include <assert.h>

#ifdef _LOG_SUPPORT
#include "log.h"
#endif

#ifdef _DO_TIME_UNLOADS_
#include "timer.h"
#endif

#ifdef _USE_DEVIL_	// if we are using devil then we use the IL image loader
class ILImageLoader : public TextureImageLoader
{
public:
	ILImageLoader();
	virtual ~ILImageLoader();

	virtual bool load ( const char* filename, TextureImage& image, bool infoOnly = false );
	virtual bool saveScreen ( const char* filename );
};
#endif

#ifdef _USE_COREIMAGE_	// if we are using Core Image (Mac OS X) then we use the core image loader
class CoreImageLoader : public TextureImageLoader
{
public:
	CoreImageLoader();
	virtual ~CoreImageLoader();

	virtual bool load ( const char* filename, TextureImage& image, bool infoOnly = false );
	virtual bool saveScreen ( const char* filename );
};
#endif

//----------------TextureImage----------------
TextureImage::TextureImage()
{
	size[0] = size[1] = 0;
	bpp = 0;
	data = NULL;
}

TextureImage::~TextureImage()
{
	if (data)
		free (data);
}

//----------------TextureSystem----------------
TextureSystem::TextureSystem()
{
	bool loaded = false;

	lastID = 0;
	imageLoader = NULL;
	resetBind();
	resetBinds();

	/* set the image loader in order of precedence */
#ifdef _USE_DEVIL_
	loaded = setImageLoader(new ILImageLoader);
#endif

#ifdef _USE_COREIMAGE_
	loaded = setImageLoader(new CoreImageLoader);
#endif

	assert(loaded && "ERROR: Unable to set the image loader");
}

TextureSystem::~TextureSystem()
{
	if (imageLoader)
		delete(imageLoader);
	flush();
}

void TextureSystem::checkUnloads ( void )
{
#ifndef _DO_TIME_UNLOADS_
	return;
#else
	float currentTime = Timer::Instance().currentTime();

	std::map<int,trTextureInfo>::iterator itr = textureMap.begin();

	while( itr != textureMap.end() )
	{
		if ( (itr->second.boundID != GL_INVALID_ID)  && ((currentTime - itr->second.lastUsedTime) > unloadTime) )
		{
			glDeleteTextures(1,(const GLuint*)&itr->second.boundID);
			itr->second.boundID = GL_INVALID_ID;
		}
		itr++;
	}
#endif // time uloads
}

void TextureSystem::resetBind ( void )
{
	binds++; // asumed at least one extra GL bind
	lastBoundTexture = GL_INVALID_ID;
}

int TextureSystem::getID ( std::string texture )
{
	std::map<std::string, int>::iterator itr = 	textureNameMap.find(TextUtils::tolower(texture));
	if (itr != textureNameMap.end())
		return itr->second;

	trTextureInfo	newTexture;
	newTexture.name = texture;

	textureNameMap[TextUtils::tolower(texture)] = loadTexture(newTexture);

	return newTexture.id;
}

bool TextureSystem::bind ( int id )
{
	binds++;
	return bindRawTexture(getTexture(id));
}

bool TextureSystem::bindRawTexture ( trTextureInfo *texture )
{
	if (!texture)
		return false;

	if ( texture->boundID == GL_INVALID_ID)
		texture->boundID = bindTexture(*texture);

	if ( texture->boundID == GL_INVALID_ID )
		return false;

	if ( lastBoundTexture != texture->boundID )
		glBindTexture(GL_TEXTURE_2D,texture->boundID);

	lastBoundTexture = texture->boundID;

#ifdef _DO_TIME_UNLOADS_
	texture->lastUsedTime = Timer::Instance().currentTime();
#endif // time uloads

	glEnable(GL_TEXTURE_2D);
	return true;
}

void TextureSystem::flush ( void )
{
	invalidate();

	textureMap.clear();
	textureNameMap.clear();
}

void TextureSystem::invalidate ( void )
{
	std::map<int,trTextureInfo>::iterator itr = textureMap.begin();

	while( itr != textureMap.end() )
	{
		if ( itr->second.boundID != GL_INVALID_ID )
			glDeleteTextures(1,(const GLuint*)&itr->second.boundID);

		itr->second.boundID = GL_INVALID_ID;
		itr++;
	}
}

TextureSystem::trTextureInfo* TextureSystem::getTexture ( int id )
{
	if (id< 0)
		return NULL;

	std::map<int,trTextureInfo>::iterator itr = textureMap.find(id);
	if ( itr == textureMap.end() )
		return NULL;

	return &(itr->second);
}

int TextureSystem::loadTexture ( trTextureInfo &info )
{
	if (!imageLoader || !info.name.size())
		return -1;

	info.textureFilePath = pathFromName(info.name);
	info.id = -1;
	info.boundID = GL_INVALID_ID;

	COSFile	file(info.textureFilePath.c_str());

	TextureImage image;
	if (!imageLoader->load(file.GetOSName(),image,true))
		return -1;

	info.x = image.size[0];
	info.y = image.size[1];

	info.id = ++lastID;
	textureMap[lastID] = info;
	return lastID;
}

unsigned int TextureSystem::bindTexture ( trTextureInfo &info )
{
	if (!imageLoader)
	{
		glDisable(GL_TEXTURE_2D);
		return GL_INVALID_ID;
	}

	COSFile	file(info.textureFilePath.c_str());
	unsigned int boundID = GL_INVALID_ID;

	TextureImage image;
	if(!imageLoader->load(file.GetOSName(),image))
		return GL_INVALID_ID;

	glGenTextures(1,(GLuint*)&boundID );
	// and in the darkness bind it
	glBindTexture(GL_TEXTURE_2D,boundID );

	GLenum	eFormat = 0;

	switch(image.bpp)
	{
	case 1:
		eFormat = GL_LUMINANCE;
		break;

	case 2:
		eFormat = GL_LUMINANCE_ALPHA;
		break;

	case 3:
		eFormat = GL_RGB;
		break;

	case 4:
	default:
		eFormat = GL_RGBA;
		break;
	}

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR_MIPMAP_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
	glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST); 

	gluBuild2DMipmaps(GL_TEXTURE_2D,image.bpp,image.size[0],image.size[1],eFormat,GL_UNSIGNED_BYTE,image.data);

	return boundID;
}

std::string TextureSystem::pathFromName ( std::string name )
{
	std::string path = TextUtils::replace_all(name,std::string("//"),std::string("/"));
	path = TextUtils::replace_all(path,std::string(":"),std::string(""));

	return path;
}

void TextureSystem::drawImage ( int id, eAlignment align, float scale )
{
	trTextureInfo *texture = getTexture(id);
	if (!texture)
		return;
	bindRawTexture(texture);
	glQuad((float)texture->x,(float)texture->y,align,scale);
}

void TextureSystem::drawImage ( int id, eAlignment align, float x, float y )
{
	trTextureInfo *texture = getTexture(id);
	if (!texture)
		return;
	bindRawTexture(texture);
	glQuad(x,y,align,1.0);
}

void TextureSystem::saveScreenshot ( const char* fileName )
{
	if(imageLoader)
		imageLoader->saveScreen(fileName);
}

int	TextureSystem::getImageWidth ( int id )
{
	trTextureInfo *texture = getTexture(id);
	if (!texture)
		return -1;

	return texture->x;
}

int	TextureSystem::getImageHeight( int id )
{
	trTextureInfo *texture = getTexture(id);
	if (!texture)
		return -1;

	return texture->y;
}

#ifdef _USE_DEVIL_

#include <IL/il.h>
#include <IL/ilu.h>
#include <IL/ilut.h>

ILImageLoader::ILImageLoader()
{
	ilInit();
	ilutRenderer(ILUT_OPENGL);
}

ILImageLoader::~ILImageLoader()
{
}

bool ILImageLoader::load ( const char* filename, TextureImage& image, bool infoOnly )
{
	unsigned int id;

	ilGenImages(1, &id);
	ilBindImage(id);
	ilLoadImage((ILstring)filename);  // Loads into the current bound image
	int error = ilGetError();
	if (error)
		return false;

	image.size[0] = ilGetInteger(IL_IMAGE_WIDTH);
	image.size[1] = ilGetInteger(IL_IMAGE_HEIGHT);
	image.bpp = ilGetInteger(IL_IMAGE_BYTES_PER_PIXEL);

	if ( image.data )
		free(image.data);
	image.data = NULL;
	if (!infoOnly)
	{
		image.data = (unsigned char*) malloc (image.size[0]*image.size[1]*image.bpp);
		memcpy(image.data,ilGetData(),image.size[0]*image.size[1]*image.bpp);
	}

	ilDeleteImages(1, &id);
	return true;
}

bool ILImageLoader::saveScreen ( const char* filename )
{
	unsigned int tempID;
	ilGenImages(1, &tempID);
	ilBindImage(tempID);
	ilutGLScreen();
	ilSaveImage((ILstring)filename);
	ilDeleteImages(1, &tempID);
	return true;
}

#endif /* _USE_DEVIL_ */


#ifdef _USE_COREIMAGE_

CoreImageLoader::CoreImageLoader()
{
}

CoreImageLoader::~CoreImageLoader()
{
}

bool CoreImageLoader::load ( const char* filename, TextureImage& image, bool infoOnly )
{
  CFStringRef fileString = CFStringCreateWithCString(NULL, filename, kCFStringEncodingUTF8);
  CFURLRef fileURL = CFURLCreateWithFileSystemPath(NULL, fileString, kCFURLPOSIXPathStyle, NULL);
  CFRelease(fileString);

  CGImageSourceRef imageSourceRef = CGImageSourceCreateWithURL(fileURL, NULL);
  if (imageSourceRef != NULL) {
    CGImageRef imageRef = CGImageSourceCreateImageAtIndex(imageSourceRef, 0, NULL);
    image.size[0] = CGImageGetWidth(imageRef);
    image.size[1] = CGImageGetHeight(imageRef);
    // Core Graphics only supports 32 bit images, so have it rendered as such regardless of the input
    image.bpp = 4; // CGImageGetBitsPerPixel(imageRef) / 8;

    //    printf("%s image is %dx%d, %d bpp\n", filename, image.size[0], image.size[1], image.bpp);

    if (!infoOnly) {

      if (image.data) {
	free(image.data);
      }
      image.data = (unsigned char*) malloc(image.size[0]*image.size[1]*image.bpp);

      CGColorSpaceRef colorSpaceRef = CGColorSpaceCreateWithName(kCGColorSpaceGenericRGB);
      CGBitmapInfo bitmapInfo = kCGImageAlphaNoneSkipLast; // still need a 4 bpp buffer
      if (image.bpp == 4) {
	bitmapInfo = kCGImageAlphaPremultipliedLast;
      }
      CGContextRef contextRef = CGBitmapContextCreate(image.data, image.size[0], image.size[1], 8, image.size[0]*image.bpp, colorSpaceRef, bitmapInfo);
      CGColorSpaceRelease(colorSpaceRef);

      CGRect rect = {{0,0},{image.size[0], image.size[1]}};
      CGContextDrawImage(contextRef, rect, imageRef);

      CGContextRelease(contextRef);
    }

    CGImageRelease(imageRef);
    return true;
  }
  return false;
}

bool CoreImageLoader::saveScreen ( const char* filename )
{
	return false;
}

#endif /* _USE_CORE_IMAGE */
