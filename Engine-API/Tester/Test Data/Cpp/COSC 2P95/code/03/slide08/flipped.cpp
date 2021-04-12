#include <iostream>
using namespace std;
//The only way we can define 'proclamation' before 'exclamation' is if proclamation knows the signature of exclamation.
//We achieve this with a forward declaration.

//Note that we don't need the function body; just the header
void exclamation();

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
