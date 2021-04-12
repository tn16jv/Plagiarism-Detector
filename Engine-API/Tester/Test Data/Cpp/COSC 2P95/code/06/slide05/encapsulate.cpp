#include <iostream>
using namespace std;

//Note that 'encapsulation' has different meanings, depending on both context and who you ask.
//When it comes to only limiting visibility, I'll be almost exclusively restricting myself to the term, "information hiding"

class Dealie {
private:
	int stuff; //Private instance variables typically follow a more explicit convention (e.g. m_stuff, _stuff, etc.)
	void continuePontification() {
		cout<<"Oh, there they go!"<<endl;
	}
public:
	void pontificate() {
		cout<<"They call'em fingers, but I never see'em fing."<<endl;
		continuePontification();
	}
	int getStuff() { //Accessors let you explicitly control what else can see a private instance variable
		return stuff;
	}
	
	//You can also create updaters (conventionally starting with 'set') to changing them
}; //<-Don't forget this semicolon!




int main() {
	Dealie d;
	d.pontificate();
	cout<<d.getStuff()<<endl;
	//d.continuePontification(); //illegal, as it's private
	//cout<<d.stuff<<endl; //ditto
}
