// worldTest.cpp : Defines the entry point for the console application.
//

#include "world.h"
#include <string>
#include <iostream>
#include <fstream>

World	world;

void buildTestWorld ( void )
{
	world.name = "sample";

	WorldCell *cell = world.newCell();
	cell->attributes[std::string("TestAtrib1")] = "Value";

	WorldObject *object = cell->newObject();
	object->attributes[std::string("TestAtrib2")] = "Value2";

	object->transform.identity();

	WorldObject::ObjectMesh mesh;
	mesh.material = "white";
	mesh.mesh = object->newMesh();

	mesh.mesh->verts.push_back(Vector3(0,0,0));
	mesh.mesh->verts.push_back(Vector3(1,0,0));
	mesh.mesh->verts.push_back(Vector3(0,1,0));

	mesh.mesh->norms.push_back(Vector3(0,0,1));

	mesh.mesh->uvs.push_back(Vector2(0,0));
	mesh.mesh->uvs.push_back(Vector2(1,0));
	mesh.mesh->uvs.push_back(Vector2(0,1));

	WorldMesh::Face face;

	face.normal.z(1);
	face.corners.push_back(WorldMesh::FaceVert(0,0,0));
	face.corners.push_back(WorldMesh::FaceVert(1,0,1));
	face.corners.push_back(WorldMesh::FaceVert(2,0,2));

	mesh.mesh->faces.push_back(face);
	object->meshes.push_back(mesh);
	cell->objects.push_back(object);
	world.cells.push_back(cell);
}

int main(int argc, char* argv[])
{
	buildTestWorld();

	std::ofstream oStream1("./test.wrld");
	WorldStreamWriter writer(world);

	writer.write(oStream1);
	oStream1.close();

	std::ifstream iStream("./test.wrld");
	WorldStreamReader reader(world);

	reader.read(iStream);
	iStream.close();

	std::ofstream oStream2("./test2.wrld");
	writer.setWorld(world);
	writer.write(oStream2);
	oStream2.close();

	return 0;
}

