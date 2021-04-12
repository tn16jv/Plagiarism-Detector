#include <iostream>
#include <fstream>

class Simple {
private:
	std::string name;
public:
	Simple(std::string name):name(name) {}
	friend std::ostream& operator<<(std::ostream &out,Simple &si);
};

std::ostream& operator<<(std::ostream &out,Simple &si) {
	out<<si.name;
	std::fstream fs;
	return out;
}

int main() {
	Simple s1("fuzzy"),s2("wuzzy"),s3("bar");
	std::cout<<s1;
	std::cout<<s2;
	std::cout<<s3;
	std::cout<<std::endl;
	//This actually may genuinely work... or it may fail horribly.
	std::cout<<s1<<s2<<s3<<std::endl;
	//What can we do to ensure that it will definitely work?
}