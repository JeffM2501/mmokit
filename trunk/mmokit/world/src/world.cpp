// world.cpp : Defines the entry point for the console application.
//

#include "world.h"
#include "textUtils.h"

using namespace TextUtils;
using namespace std;


//WorldObject
WorldMesh* WorldObject::newMesh ( void )
{
	return new WorldMesh();
}

void WorldObject::deleteMesh ( WorldMesh* p )
{
	delete(p);
}

//WorldCell
WorldObject* WorldCell::newObject ( void )
{
	return new WorldObject();
}

void WorldCell::deleteObject ( WorldObject* p )
{
	delete(p);
}

//World
WorldCell* World::newCell ( void )
{
	return new WorldCell();
}

void World::deleteCell ( WorldCell* p )
{
	delete(p);
}

void World::clear ( void )
{
	for (size_t c = 0; c < cells.size(); c++)
	{
		WorldCell *cell = cells[c];

		if (cell)
		{
			for (size_t o = 0; o < cell->objects.size(); o++)
			{	
				WorldObject *object = cell->objects[o];
				if (object)
				{
					for (size_t m = 0; m < object->meshes.size(); m++)
					{
						WorldObject::ObjectMesh &mesh = object->meshes[m];
						if (mesh.mesh)
							object->deleteMesh(mesh.mesh);
					}
					cell->deleteObject(object);
				}
			}
			deleteCell(cell);
		}
	}
	cells.clear();
	name = "";
}


//WorldStreamReader
WorldStreamReader::WorldStreamReader(World &w): world(w)
{
}

bool WorldStreamReader::read ( std::istream &input )
{
	string token;
	input >> token;

	world.clear();

	WorldCell *cell = NULL;
	WorldObject * object = NULL;
	WorldObject::ObjectMesh mesh;

	if ( same_no_case(token,"world:") )
	{
		// read the name
		input >> world.name;
		while (token.size())
		{
			input >> token;

			if (same_no_case(token,"cell:"))
			{
				if (cell)
					world.cells.push_back(cell);

				cell = world.newCell();

				size_t cellID;
				input >> cellID;
			}
			else if (same_no_case(token,"object:"))
			{
				if (object)
				{
					if (cell)
						cell->objects.push_back(object);
				}
				if (cell)
					object = cell->newObject();
				else
					object = NULL;

				size_t objectID;
				input >> objectID;
			}
			else
			{
				char temp[512];
				input.getline(temp,512);
			}
		}
	}

	if (cell)
		world.cells.push_back(cell);

	return world.cells.size() > 0;
}

//WorldStreamWriter
WorldStreamWriter::WorldStreamWriter(World &w): world(w)
{
}

void WorldStreamWriter::dumpAttributes ( ostream &output, const char* tag, const AttributeList &attributes )
{
	for ( AttributeList::const_iterator a = attributes.begin(); a != attributes.end(); a++ )
		output << "attribute: " << tag << ": " << a->first << " " << a->second << "\n";
}

bool WorldStreamWriter::write ( std::ostream &output )
{
	if (!world.cells.size())
		return false;

	output << "world: " << world.name << "\n";

	dumpAttributes(output,"world",world.attributes);

	for ( size_t c = 0; c < world.cells.size(); c++ )
	{
		WorldCell *cell = world.cells[c];
		if (cell)
		{
			output << "cell: " << c << "\n";

			dumpAttributes(output,"cell",cell->attributes);

			for ( size_t o = 0; o < cell->objects.size(); o++ )
			{
				output << "object: " << o << "\n";
				WorldObject * object = cell->objects[o];

				dumpAttributes(output,"object",object->attributes);

				output << "transform: " << object->transform << "\n";

				for ( size_t m = 0; m < object->meshes.size(); m++ )
				{
					const WorldObject::ObjectMesh& mesh = object->meshes[m];

					output << "mesh: " << mesh.material << "\n";

					for ( size_t v = 0; v < mesh.mesh->verts.size(); v++ )
						output << "vert: " << mesh.mesh->verts[v] << "\n";

					for ( size_t n = 0; n < mesh.mesh->norms.size(); n++ )
						output << "norm: " << mesh.mesh->norms[n] << "\n";

					for ( size_t u = 0; u < mesh.mesh->uvs.size(); u++ )
						output << "uv: " << mesh.mesh->uvs[u] << "\n";

					for ( size_t f =0; f < mesh.mesh->faces.size(); f++ )
					{
						WorldMesh::Face &face = mesh.mesh->faces[f];
						output << "face: " << f << "\n";
						output << "normal: " << face.normal << "\n";

						for ( size_t v = 0; v < face.corners.size(); v++ )
							output << "corner: " << face.corners[v].v << " " << face.corners[v].n << " " << face.corners[v].v << "\n";
					}
				}
			}
		}
	}
	return true;
}

