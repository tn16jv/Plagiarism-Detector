#include <iostream>
using namespace std;

class Dealie {
private:
	int item;
public:
	Dealie(): item(1) {cout<<"I'm alive? ... I'M ALIVE! THIS IS AMAZING!"<<endl;}
	void increment() {item++;}
	int getCount() {return item;}
	
	~Dealie() {cout<<"...what?? no... no! You can't do this to me! I've learned so much! Counted so much! "<<item<<" times!"<<endl;}
};

class Wokka {
private:
	int *data;
	int dLength;
	void initialize(int length) {
		dLength=length;
		data=new int[dLength];
	}
public:
	Wokka(){
		initialize(5);
	}
	Wokka(int length) {
		initialize(length);
	}
	~Wokka() {
		delete[] data;
		cout<<"DESTROYED!"<<endl;
	}
};

int main() {
	cout<<"Hey, let's enter a block? (or we could also demosntrate with a function)"<<endl;
	{ //If we want, we can declare arbitrary blocks. That's also useful for namespaces.
		Dealie d;
		d.increment();d.increment();d.increment();
		cout<<d.getCount()<<endl;
		d.increment();d.increment();d.increment();
	}
	cout<<"Whoops."<<endl;
	{
		Wokka w(8);
	}
	Wokka *w=new Wokka;
	delete w;
}
