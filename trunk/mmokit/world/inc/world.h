// world.h : Defines the entry point for the console application.
//

#ifndef _WORLD_H_
#define _WORLD_H_
#include <vector>
#include <map>
#include <string>
#include "vectorMath.h"
#include <iostream>

// base class for the world object.
// the final class will derive off

typedef std::map<std::string,std::string> AttributeList;

class WorldMesh
{
public:
	WorldMesh(){};
	virtual ~WorldMesh(){};

	std::vector<Vector3> verts,norms;
	std::vector<Vector2> uvs;

	class  FaceVert
	{
	public:
		FaceVert(int _v = -1, int _n = -1, int _u = -1)
		{
			v = _v;
			n = _n;
			u = _u;
		}

		FaceVert(const FaceVert& fv)
		{
			v = fv.v;
			n = fv.n;
			u = fv.u;
		}
		int v,n,u;
	};

	typedef struct  
	{
		std::vector<FaceVert> corners;
		Vector3 normal;
	}Face;

	std::vector<Face>	faces;
};

class WorldObject
{
public:
	WorldObject(){};
	virtual ~WorldObject(){};

	class ObjectMesh
	{
	public:
		std::string material;
		WorldMesh	*mesh;

		ObjectMesh()
		{
			mesh = NULL;
		}
	};

	typedef std::vector<ObjectMesh> ObjectMeshList;

	ObjectMeshList meshes;

	Matrix34			transform;

	virtual WorldMesh* newMesh ( void );
	virtual void deleteMesh ( WorldMesh* p );

	AttributeList attributes;
};

class WorldCell
{
public:
	WorldCell(){};
	virtual ~WorldCell(){};

	typedef std::vector<WorldObject*> WorldObjectList;

	WorldObjectList objects;

	virtual WorldObject* newObject ( void );
	virtual void deleteObject ( WorldObject* p );

	AttributeList attributes;
};

class World
{
public:
	World(){};
	virtual ~World(){};


	void clear ( void );

	virtual WorldCell* newCell ( void );
	virtual void deleteCell ( WorldCell* p );

	AttributeList attributes;

	typedef std::vector<WorldCell*> WorldCellList;

	WorldCellList cells;

	std::string name;
};

class WorldStreamReader
{
public:
	WorldStreamReader(World &w);
	virtual ~WorldStreamReader(){};

	void setWorld(World &w){world = w;}

	bool read ( std::istream &input );

protected:
	World	&world;
};

class WorldStreamWriter
{
public:
	WorldStreamWriter(World &w);
	virtual ~WorldStreamWriter(){};

	void setWorld(World &w){world = w;}

	bool write ( std::ostream &output );

protected:
	void dumpAttributes ( ostream &output, const char* tag, const AttributeList &attributes );
	World	&world;
};

#endif _WORLD_H_
