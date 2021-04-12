#include <iostream>


class Alpha {
public:
	void land() {std::cout<<"Alefgard"<<std::endl;}
	void common() {std::cout<<"Need some more earth blocks..."<<std::endl;}
};

class Betta:public Alpha {
private:
	using Alpha::land;
public:
	void town() {std::cout<<"Cantlin, ";land();}
	void common() {std::cout<<"Wonder when I get iron..."<<std::endl;}
};

int main() {
	Alpha a;
	Betta b;
	a.land();
	//b.land();//Not accessible
	b.town();
	a.common();
	b.common();
	b.Alpha::common();
	std::cout<<std::endl;
	Alpha *ab=&b;
	ab->common();//Huh. Is that what we wanted to happen?
}