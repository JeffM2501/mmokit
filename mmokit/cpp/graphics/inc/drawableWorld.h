// crawableWorld.h : Defines the entry point for the console application.
//

#ifndef _DRAWABLE_WORLD_H_
#define _DRAWABLE_WORLD_H_
#include "collisionWorld.h"
#include "drawables.h"
#include "textures.h"
#include "materials.h"

// base class for the world object.
// the final class will derive off

class DrawableMesh : public CollisionMesh, public DrawableItem, public GLDisplayListCreator
{
public:
	DrawableMesh(World*w):CollisionMesh(w){list = GL_INVALID_ID;}
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
	DrawableObject(World*w):CollisionObject(w){};
	virtual ~DrawableObject(){};

	virtual WorldMesh* newMesh ( void );
	virtual void deleteMesh ( WorldMesh* p );

	virtual void finalise ( void );

	virtual void draw ( void );

protected:
	std::vector<MaterialID>	materialIDs;
	std::vector<DrawableID>	drawableIDs;
};

class DrawableCell : public CollisionCell
{
public:
	DrawableCell(World*w):CollisionCell(w){};
	virtual ~DrawableCell(){};

	virtual WorldObject* newObject ( void );
	virtual void deleteObject ( WorldObject* p );

	virtual void draw ( void );
};

class DrawableMaterial : public WorldMaterial
{
public:
	DrawableMaterial(World*w):WorldMaterial(w){material = _INVALID_MATERIAL;}
	virtual ~DrawableMaterial();

	virtual void finalise ( void );

	MaterialID material;
};

class DrawableWorld : public CollisionWorld
{
public:
	DrawableWorld():CollisionWorld(){};
	virtual ~DrawableWorld(){};

	virtual void finalise ( void );

	virtual WorldCell* newCell ( void );
	virtual void deleteCell ( WorldCell* p );

	virtual WorldMaterial* newMaterial( void );
	virtual void deleteMaterial ( WorldMaterial* p );

	virtual void draw ( void );

	DrawableMaterial* findMaterial ( const std::string &name );

protected:
	std::map<std::string,size_t> materialNameMap;
};

#endif _DRAWABLE_WORLD_H_
