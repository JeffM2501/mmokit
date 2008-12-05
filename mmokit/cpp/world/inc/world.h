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

class AttributeList
{
public:
	virtual ~AttributeList(){};
		
	bool exists ( const std::string &key );
	bool exists ( const char* key );

	const std::string& get ( const std::string &key );
	const std::string& get ( const char* key );

	void set ( const std::string &key, const std::string &value );
	void set ( const char* key, const std::string &value );
	void set ( const std::string &key, const  char* value );
	void set ( const char *key, const char* value );

	std::map<std::string,std::string> attributes;
}; 

class World;

class WorldMesh
{
public:
	WorldMesh(World*w = NULL):world(w){};
	virtual ~WorldMesh(){};

	virtual void finalise ( void );

	std::vector<Vector3> verts,norms;
	std::vector<Vector2> uvs;

	class  FaceVert
	{
	public:
		FaceVert(size_t _v = 0, size_t _n = 0, size_t _u = -1)
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
		size_t v,n,u;
	};

	typedef struct  
	{
		std::vector<FaceVert> corners;
		Vector3 normal;
	}Face;

	std::vector<Face>	faces;
	World	*world;
};

class WorldObject
{
public:
	WorldObject(World*w = NULL):world(w){};
	virtual ~WorldObject(){};

	virtual void finalise ( void );

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
	World *world;
};

class WorldCell
{
public:
	WorldCell(World*w = NULL):world(w){};
	virtual ~WorldCell(){};

	virtual void finalise ( void );

	typedef std::vector<WorldObject*> WorldObjectList;

	WorldObjectList objects;

	virtual WorldObject* newObject ( void );
	virtual void deleteObject ( WorldObject* p );

	AttributeList attributes;
	World *world;
};

class WorldMaterial
{
public:
	WorldMaterial(World*w = NULL):world(w){};
	virtual ~WorldMaterial(){};

	virtual void finalise ( void ){};

	std::string name;
	std::string texture;

	AttributeList attributes;
	World *world;
};
 
class World
{
public:
	World(){};
	virtual ~World(){clear();}

	virtual void finalise ( void );

	void clear ( void );

	virtual WorldCell* newCell ( void );
	virtual void deleteCell ( WorldCell* p );

	virtual WorldMaterial* newMaterial ( void );
	virtual void deleteMaterial ( WorldMaterial* p );

	AttributeList attributes;

	typedef std::vector<WorldCell*> WorldCellList;
	typedef std::vector<WorldMaterial*> WorldMaterialList;

	WorldCellList cells;
	WorldMaterialList materials;

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
