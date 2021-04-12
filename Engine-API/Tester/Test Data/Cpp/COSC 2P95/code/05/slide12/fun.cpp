#include <iostream>
using namespace std;


struct Jib{
	int i;
	float f;
	void (*jab)();
	double (*job)(int,float); //Of course, these don't have to be in structs
};

void exclaim() {
	cout<<"I believe I might be fond of cheese."<<endl;
}

double mooltipass(int i, float f) {
	return i*f;
}

double addendum(int i, float f) {
	return i+f;
}

int main() {
	Jib j={2,3.2f};
	j.jab=&exclaim; //You can get away without the &, but it's advisable
	j.jab();
	
	j.job=&mooltipass;
	cout<<j.job(j.i,j.f)<<endl;
	j.job=&addendum;
	cout<<j.job(j.i,j.f)<<endl;
	
}
