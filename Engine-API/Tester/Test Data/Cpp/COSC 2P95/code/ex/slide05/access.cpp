#include <iostream>
//Also relies on secretiveaccess.cpp

//These forward declarations are, of course, silly
int quack();
int tweet();


int cheese=8;
extern int geese;//Not going to work! (Refer to secretiveaccess to see why)
int meese=9;

class Data {
private:
	int cheese;
public:
	int meese;
	Data():cheese(3),meese(7){}
	int getCheese(){return cheese;}
};


void a() {
	std::cout<<"Inside function, cheese:"<<cheese<<std::endl;
	int cheese;
	std::cout<<"Inside function, cheese:"<<cheese<<std::endl;
	std::cout<<"Inside function, meese:"<<meese<<std::endl;
}

namespace hellojoe {
	int joe=11;
}


int main() {
	//std::cout<<geese<<std::endl;//can't do this one!
	//quack();//can't do this either; same reason
	tweet();//perfectly legal
	Data d;
	//std::cout<<d.cheese<<std::endl;//Not legal, because private!
	std::cout<<d.getCheese()<<std::endl;
	std::cout<<d.meese<<std::endl;
	a();
	std::cout<<hellojoe::joe<<std::endl;//Oh noeses... this won't work! Why?
}