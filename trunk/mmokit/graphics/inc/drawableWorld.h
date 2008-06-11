// crawableWorld.h : Defines the entry point for the console application.
//

#ifndef _DRAWABLE_WORLD_H_
#define _DRAWABLE_WORLD_H_
#include "collisionWorld.h"
#include "drawables.h"
#include "textures.h"

// base class for the world object.
// the final class will derive off

class DrawableMesh : public CollisionMesh, public DrawableItem, public GLDisplayListCreator
{
public:
	DrawableMesh():CollisionMesh(){list = GL_INVALID_ID;}
	virtual ~DrawableMesh(){};

	virtual void draw ( DrawableID id, int matID );

	virtual void buildGeometry ( GLDisplayList displayList );

	Matrix34		matrix;
	GLDisplayList	list;
protected:
};

class DrawableObject : public CollisionObject
{
public:
	DrawableObject():CollisionObject(){};
	virtual ~DrawableObject(){};

	virtual WorldMesh* newMesh ( void );
	virtual void deleteMesh ( WorldMesh* p );

	virtual void finalise ( void );

	virtual void draw ( void );

protected:
	std::vector<int>	textureIDs;
	std::vector<DrawableID>	drawableIDs;
};

class DrawableCell : public CollisionCell
{
public:
	DrawableCell():CollisionCell(){};
	virtual ~DrawableCell(){};

	virtual WorldObject* newObject ( void );
	virtual void deleteObject ( WorldObject* p );

	virtual void draw ( void );
};

class DrawableWorld : public CollisionWorld
{
public:
	DrawableWorld():CollisionWorld(){};
	virtual ~DrawableWorld(){};

	virtual WorldCell* newCell ( void );
	virtual void deleteCell ( WorldCell* p );

	virtual void draw ( void );
};

#endif _DRAWABLE_WORLD_H_
