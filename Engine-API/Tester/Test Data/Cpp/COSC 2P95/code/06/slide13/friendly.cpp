#include <iostream>
using namespace std;

class Extro;

class Intro {
private:
	int grobble;
	int wokka;
public:
	Intro(int grobble, int wokka):grobble(grobble),wokka(wokka){}
	friend int inspect(Intro &in, Extro &ext);
	friend class Extro;
};

class Extro {
	int stuff;
public:
	Extro(int stuff=8):stuff(stuff){}
	void dealie(Intro &in) {cout<<in.grobble<<"\t"<<in.wokka<<endl;}
	friend int inspect(Intro &in, Extro &ext);
};

int inspect(Intro &in, Extro &ext) {
	return in.grobble+in.wokka+ext.stuff;
}

int main() {
	Intro in(3,4);
	Extro ext;
	cout<<inspect(in,ext)<<endl;
	ext.dealie(in);
}
