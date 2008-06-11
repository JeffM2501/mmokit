// collisionWorld.h : Defines the entry point for the console application.
//

#ifndef _COLLISION_WORLD_H_
#define _COLLISION_WORLD_H_
#include "world.h"

// base class for the world object.
// the final class will derive off

typedef std::map<std::string,std::string> AttributeList;

class CollisionMesh : public WorldMesh
{
public:
	CollisionMesh():WorldMesh(){};
	virtual ~CollisionMesh(){};

	virtual void finalise ( void );

	virtual bool collide ( const Vector3 &pos, const float rad, Vector3 *hit);
	virtual bool collide ( const Vector3 &pos, const Vector3 &size, Vector3 *hit);
	virtual bool collide ( const Vector3 &pos, const Vector3 &size, const Matrix34& transform, Vector3 *hit);

	Vector3 center;
	Vector3 bboxSize;
	float radius;
};

class CollisionObject : public WorldObject
{
public:
	CollisionObject():WorldObject(){};
	virtual ~CollisionObject(){};

	virtual WorldMesh* newMesh ( void );
	virtual void deleteMesh ( WorldMesh* p );

	virtual bool collide ( const Vector3 &pos, const float rad, Vector3 *hit);
	virtual bool collide ( const Vector3 &pos, const Vector3 &size, Vector3 *hit);
	virtual bool collide ( const Vector3 &pos, const Vector3 &size, const Matrix34& transform, Vector3 *hit);
};

class CollisionCell : public WorldCell
{
public:
	CollisionCell():WorldCell(){};
	virtual ~CollisionCell(){};

	virtual WorldObject* newObject ( void );
	virtual void deleteObject ( WorldObject* p );

	virtual bool collide ( const Vector3 &pos, const float rad, Vector3 *hit);
	virtual bool collide ( const Vector3 &pos, const Vector3 &size, Vector3 *hit);
	virtual bool collide ( const Vector3 &pos, const Vector3 &size, const Matrix34& transform, Vector3 *hit);
};

class CollisionWorld : public World
{
public:
	CollisionWorld():World(){};
	virtual ~CollisionWorld(){};

	virtual WorldCell* newCell ( void );
	virtual void deleteCell ( WorldCell* p );

	virtual bool collide ( const Vector3 &pos, const float rad, Vector3 *hit);
	virtual bool collide ( const Vector3 &pos, const Vector3 &size, Vector3 *hit);
	virtual bool collide ( const Vector3 &pos, const Vector3 &size, const Matrix34& transform, Vector3 *hit);
};

#endif _COLLISION_WORLD_H_
