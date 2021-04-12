//There's really no need for the include guard here
#ifndef ARCHAEOPTERYX_H
#define ARCHAEOPTERYX_H
#include "Velociraptor.h"
#include "ornithology.h"

namespace paleontology {
	class Archaeopteryx : public Velociraptor, public ornithology::Bird {};
}
#endif
