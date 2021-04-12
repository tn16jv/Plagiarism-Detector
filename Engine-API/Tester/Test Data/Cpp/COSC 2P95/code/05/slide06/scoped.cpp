#include <iostream>
using namespace std;
//Note: compile with C++11 standard

//Let's try this again, but contain our enumerated options within their types
enum class Colour {
	PURPLE,
	VIOLET,
	PERIWINKLE
};

enum class Flavour {
	BLUE_RASPBERRY,
	ORANGE,
	PURPLE
};



int main() {
	enum class Pen {
		PURPLE,
		PRETTY_PURPLE
	};
	Pen p(Pen::PURPLE);
	//Flavour f=PURPLE; //Not legal at all!
	Flavour f=Flavour::PURPLE;
	//Colour c=Flavour::PURPLE; //All kinds of illegal
	Colour c=static_cast<Colour>(Flavour::PURPLE); //Don't. do. this.
	//cout<<c<<endl; //Not legal
	cout<<(int)c<<endl;
}
