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

#include "camera.h"

Camera::Camera()
{
	pullback = 10;
	targetMode = true;
	tilt = 0; // up/down;
	rot = 0; // left/right
}

void Camera::execute ( void )
{
	if (targetMode)
	{
		glLoadIdentity ();
		glTranslatef(0,0,-pullback);						// pull back on allong the zoom vector
		glRotatef(tilt, 1.0f, 0.0f, 0.0f);					// pops us to the tilt
		glRotatef(-rot, 0.0f, 1.0f, 0.0f);					// gets us on our rot
		glTranslatef(-pos.x(),-pos.x(),pos.x());			// take us to the pos
		glRotatef(-90, 1.0f, 0.0f, 0.0f);					// gets us into XY

	}
	else
	{
		glLoadIdentity ();
		//glRotatef(rot, 0.0f, 0.0f, 1.0f);
		glRotatef(-tilt, 1.0f, 0.0f, 0.0f);
		glRotatef(-rot, 0.0f, 1.0f, 0.0f);
		glTranslatef(-pos.x(),-pos.x(),pos.x());
		glRotatef(-90, 1.0f, 0.0f, 0.0f);
	}
}
void Camera::setTargetPullback ( float p, bool increment )
{
	if (increment)
		pullback += p;
	else
		pullback = p;
}

void Camera::move ( float x, float y, float z )
{
	pos = Vector3(x,y,z);
}

void Camera::pan ( float x, float y, float z )
{
	// get the XY vector
	float vec[3];
	vec[0] = (float)sinf(rot*fastRad_con);
	vec[1] = (float)cosf(rot*fastRad_con);
	vec[2] = (float)sinf(tilt*fastRad_con);

	pos += Vector3(vec[0]*y,-vec[1]*y,0);

	if (!targetMode)
		pos.z(pos.z()+-vec[2]*y);

	//vec[0] = (float)sinf((rot+90.0f)*fastRad_con);
	//vec[1] = (float)cosf((rot+90.0f)*fastRad_con);
	pos += Vector3(vec[1]*x,vec[0]*x,0);

	vec[0] = (float)sinf(rot*fastRad_con) * (float)cosf(tilt*fastRad_con);
	vec[1] = (float)cosf(rot*fastRad_con) * (float)cosf(tilt*fastRad_con);
	vec[2] = (float)cos(tilt*fastRad_con);

	if (!targetMode)
		pos += Vector3(vec[0]*z,vec[1]*z,vec[2]*z);
	else
		pos.z(pos.z()+z);
}

void Camera::rotate ( float r, float t, bool increment )
{
	if (!increment)
	{
		rot = r;
		tilt = t;
		return;
	}

	rot += r;
	tilt += t;
}

// the busted camera
MatrixCamera::MatrixCamera()
{
	target.y(1.0f);
	up.z(1.0f);
	forward = target;
	right.x(1.0f);
}

void MatrixCamera::execute ( void )
{
	gluLookAt(pos.x(),pos.y(),pos.z(),target.x(),target.y(),target.z(),up.x(),up.y(),up.z());
}

void MatrixCamera::move ( float x, float y, float z )
{
	Vector3 newLoc(x,y,z);
	Vector3 delta = newLoc - pos;
	target += delta;
	pos = newLoc;
}

void MatrixCamera::pan ( float h, float v, float d )
{
	Vector3 viewVec = target-pos;
	viewVec.normalise();

	Vector3 newLoc = pos;

	// slide along H
	newLoc += right * h ;

	// slide along V
	newLoc += up * v ;

	// slide along D
	newLoc -= forward * d;

	// move the target point the same distance
	target += newLoc - pos;
	pos = newLoc;
}

void MatrixCamera::rotate ( float h, float v, float d )
{
	// just modify the target axis
	Matrix34 matrix;

	if (fabs(v) > REALY_REALY_SMALL)
	{
		matrix.identity();
		Vector3 vec = target-pos;
		// spin around the v axis
		matrix.rotation(up,sinf(fastRad_con*v),cosf(fastRad_con*v));
		matrix.transformNorm(vec);
		target = pos + vec;
	}

	return;
	if (fabs(h) > REALY_REALY_SMALL)
	{
		Matrix34 matrix2;
		matrix.identity();
		// rotate around the h axis
		Vector3 vec2 = target-pos;
		Vector3 hVec;
		hVec.cross(up,vec2);
		hVec.normalise();
		matrix2.rotation(hVec,sinf(fastRad_con*h),cosf(fastRad_con*h));
		matrix2.transformPos(vec2);
		matrix2.transformPos(up);
		target = pos + vec2;
	}
}

void MatrixCamera::circle ( float h, float v, float d )
{
	// keep the target, move the camera
	Matrix34 matrix;

	if (fabs(v) > REALY_REALY_SMALL)
	{
		matrix.identity();
		Vector3 vec = pos-target;
		// spin around the v axis
		matrix.rotation(up,sinf(fastRad_con*v),cosf(fastRad_con*v));
		matrix.transformNorm(vec);
		pos = target + vec;
	}

	return;
	if (fabs(h) > REALY_REALY_SMALL)
	{
		// rotate around the h axis
		matrix.identity();
		Vector3 vec2 = pos-target;
		Vector3 hVec;
		hVec.cross(up,vec2);
		hVec.normalise();
		matrix.rotation(hVec,sinf(fastRad_con*h),cosf(fastRad_con*h));
		matrix.transformPos(vec2);
		matrix.transformPos(up);
		pos = target + vec2;
	}
}

void MatrixCamera::lookAt ( float x, float y, float z )
{
	Vector3 newTarget(x,y,z);
	if (newTarget.close(pos))
		return;

	Vector3 v1 = target-pos;
	v1.normalise();
	Vector3 v2 = newTarget-pos;
	v2.normalise();

	Vector3 delta = v1-v2;
	if (!v1.close(v2))	// if there is some change recomput the up
	{
		Vector3 norm;
		norm.cross(v1,v2);
		norm.normalise();

		float dot = v1.dot(v2);
		float angle = acos(dot)/fastRad_con;

		Matrix34 matrix;
		// spin around the normal axis
		matrix.rotation(norm,sinf(fastRad_con*angle),cosf(fastRad_con*angle));
		matrix.transformPos(up);
		up.normalise();

		matrix.transformPos(right);
		right.normalise();
	}

	target = newTarget;

	forward = target-pos;
	forward.normalise();

}
