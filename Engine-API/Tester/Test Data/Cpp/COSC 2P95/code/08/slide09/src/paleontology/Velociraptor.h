//The only reason we have the include guard here is because of the addition of archaeopteryx, to allow for the possibility of both species
//having their headers included by the same client
#ifndef VELOCIRAPTOR_H
#define VELOCIRAPTOR_H

//I could put the include guard for paleontology here as well, but there's really no point
#include "paleontology.h"
#include "feathers.h"

namespace paleontology {
	class Velociraptor : public Dinosaur, public feathers::Feathered {};
}
#endif
