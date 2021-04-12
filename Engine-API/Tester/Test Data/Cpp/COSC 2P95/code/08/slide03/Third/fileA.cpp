#include <iostream>
#include <string>
#include "fileB.h"

/*Namespaces are arguably a better way to control access to functions. At the very least, it's a superior way to avoid accidental
 *naming conflicts.
 *Note that you don't have to define everything within a namespace all at once. You can add more to it later.
 *
 *Also, when 'using' a namesapce, don't forget that you can do so within a block, so you can automatically fall out of the using clause
 *the moment you leave the block.
 *Similarly, you never *need* to use 'using'; you can always explicitly specify namespaces to access.
 */

namespace Abba {
	std::string wokka() {
		return "A-Abba-Wokka";
	}
}

namespace Friendo {
	std::string grobble() {
		return "A-Friendo-grobble";
	}
}


int main() {
	//std::cout<<wokka()<<std::endl; //Wouldn't be legal, because wokka doesn't exist in the "global namespace"
	std::cout<<Abba::wokka()<<std::endl; //<-Entirely legal
	{
		using namespace Abba;
		std::cout<<wokka()<<std::endl; //<-Also legal
	}
	//std::cout<<wokka()<<std::endl; //<-Once again illegal
	
	using Abba::wokka;
	std::cout<<wokka()<<std::endl; //<-Also legal
	
	std::cout<<Betta::dealie()<<std::endl; //Namespace declared in another file
	
	//We can construct a namespace from multiple locations
	std::cout<<Friendo::grobble()<<std::endl;
	std::cout<<Friendo::stuff()<<std::endl;
}
