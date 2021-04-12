#include <iostream>
using namespace std;

//Nope. Not going to make an x-men joke
//(unless that counts)
//((that doesn't count, right?))


//We use the word 'class' to define a class. Who'da thunkit?
class Dealie {
public:	//We'll come back to this in the next example
	int stuff;
	void wokka() {
		cout<<"Grobble grobble grobble! ("<<stuff<<")"<<endl;
	}
}; //<-Don't forget this semicolon!




int main() {
	Dealie d; //Warning to Java-ites! This has already been instantiated!
	d.wokka();
	cout<<d.stuff<<endl; //I'm sure this will end well...
	d.stuff=42;
	d.wokka();
	Dealie *e=&d;
	e->wokka();//Seem familiar?
}
