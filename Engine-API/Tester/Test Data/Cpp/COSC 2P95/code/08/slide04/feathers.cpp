#include <iostream>
#include "feathers.h"

namespace feathers {
	void Feathered::flap() {
		std::cout<<"Flaaap! Flap flap! Flap!"<<std::endl;
	}

	void Feathered::balance() {
		std::cout<<"I... don't know the sound of using wings to balance"<<std::endl;
	}

	void Feathered::writeOldTymeyLetters() {
		std::cout<<"You're never far from a quill when you grow them yourself! ...Ow!"<<std::endl;
	}
}
