#include <iostream>
using namespace std;
// This example works just fine. No warnings, no concerns.
// However, try swapping the two procedures, and see what happens.

void proclamation() {
	cout<<"Outlaw Country!"<<endl;
	exclamation();
}

void exclamation() {
	cout<<"Woo!"<<endl;
}

int main() {
	proclamation();
}
