#include <iostream>
using namespace std;

//Constructors make objects ready for use.
//Typically, they initialize values and, when necessary, allocate additional memory

class Dealie {
private:
	//Note: don't declare a private constructor unless you want to prevent others from being able to use it (but that's a good use for it)
	int stuff;
	void continuePontification() {
		cout<<"Oh, there they go!"<<endl;
	}
public:
	Dealie(int s)
	{
		stuff=s;
	}
	void pontificate() {
		cout<<"They call'em fingers, but I never see'em fing."<<endl;
		continuePontification();
	}
	int getStuff() { //Accessors let you explicitly control what else can see a private instance variable
		return stuff;
	}
	
	//You can also create updaters (conventionally starting with 'set') to changing them
}; //<-Don't forget this semicolon!

class Stuff {
private:
	int abba;
	const int bebe; //This isn't a problem! (but only because of our handy-dandy member initializer list below)
public:
	//Member initializer lists can make for convenient shorthand, and also make actual behaviour stand out
	Stuff(int a, int b): abba(a), bebe(b)
		{}
	int getBebe(){
		return bebe;
	}
};

/*
class Wokka {
private:
	int numbers[5];
public:
	Wokka(): numbers{1,2,3,4,5} {} //Completely fine with C++11; otherwise, expect to need to add code to the constructor body
};
//*/


int main() {
	Dealie d(4);//Direct initialization
	cout<<d.getStuff()<<endl;
	
	//Initialization lists work for public instance variables - even without constructors - but don't play nice when the instance
	//variables are private
	//Dealie e={5};
	
	//Dealie f{6}; //Uniform initialization (but only starting with C++11)
	
	//Note: this is no longer legal! Since we've explicitly defined a constructor, the 'default constructor' is no longer provided for us
	//Dealie g;
	
	Stuff s(3,5);
	cout<<s.getBebe()<<endl;
}
