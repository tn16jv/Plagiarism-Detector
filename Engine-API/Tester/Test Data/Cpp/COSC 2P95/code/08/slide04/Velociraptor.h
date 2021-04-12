#include "paleontology.h"
#include "feathers.h"

namespace paleontology {
	class Velociraptor : public Dinosaur, public feathers::Feathered {};
}
