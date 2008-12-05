// GPx01
// Copyright (c) 2005 - 2007 Jeff Myers
//
// This package is free software;  you can redistribute it and/or
// modify it under the terms of the license found in the file
// named COPYING that should have accompanied this file.
//
// THIS PACKAGE IS PROVIDED ``AS IS'' AND WITHOUT ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
//

#ifndef _GL_CAMERA_H_
#define _GL_CAMERA_H_

#include "openGL.h"
#include "vectorMath.h"

class Camera 
{
public:

	Camera();

	void execute ( void );

	void move ( float x, float y, float z );
	void pan ( float x, float y, float z );
	void rotate ( float r, float t, bool increment = true );

	void setTargetPullback ( float p, bool increment = true );

	void setTargetMode ( bool mode ) {targetMode = mode;}
	bool getTargetMode ( void ) {return targetMode;}

protected:
	Vector3 pos;
	Vector3 target;

	bool targetMode;
	float tilt; // up/down;
	float rot; // left/right
	float pullback;
};


class MatrixCamera 
{
public:

	MatrixCamera();

	void execute ( void );

	void move ( float x, float y, float z );
	void pan ( float x, float y, float z );
	void rotate ( float x, float y, float z );
	void circle ( float x, float y, float z );

	void lookAt ( float x, float y, float z );

	Vector3 getTarget ( void ) { return target; }
	Vector3 getUp ( void ) { return up; }

protected:
	Vector3 pos;
	Vector3 target;
	Vector3 up;
	Vector3 right;
	Vector3 forward;
};


#endif //_GL_CAMERA_H_

