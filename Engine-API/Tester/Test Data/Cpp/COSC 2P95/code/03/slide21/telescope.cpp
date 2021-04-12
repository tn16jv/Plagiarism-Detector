#include <iostream>
using namespace std;
//This one just revisits scope and extent a bit

int dealie=3;
int *ptr;

void teksetter(int i) {
	ptr=&i;
}

void grobble() {
	cout<<"Inside grobble: "<<dealie<<endl;
}

void wokka() {
	dealie=5;
	cout<<"wokka changed dealie"<<endl;
}

void stuff() {
	int dealie=7;
	cout<<"Inside stuff: "<<dealie<<endl;
}

void hmm() {
	cout<<"Starting hmm: "<<dealie<<endl;
	int dealie=20; //Even within a block, it matters when you define a variable
	cout<<"Ending hmm: "<<dealie<<endl;
}

void pegas(int k) {
	cout<<"I wonder what's in that thing that doesn't exist anymore? "<<*ptr<<endl;
}

void voltekka(int j) {
	pegas(j);
}

int main() {
	int i=4;
	teksetter(i);
	grobble();
	wokka();
	grobble();
	stuff();
	grobble();
	hmm();
	grobble();
	voltekka(5);
}
