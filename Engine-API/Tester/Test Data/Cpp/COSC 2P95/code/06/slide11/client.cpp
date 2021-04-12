#include <iostream>
#include "Bird.h"
using namespace std;

int main() {
	Bird b;
	cout<<b.getWingspan()<<endl;
	b.transformIntoMechaBird();
	b.flap();
}
