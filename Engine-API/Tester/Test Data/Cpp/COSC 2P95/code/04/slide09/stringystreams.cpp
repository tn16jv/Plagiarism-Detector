#include <iostream>
#include <iomanip>
#include <sstream>
using namespace std;
//Note that you might actually want to stop using that namespace, as they're separate for a reason...

void ss() {
	string response;
	bool assumeGood=true;
	int value;
	stringstream stream;//This form is used for both input and output
	cout<<"How old is J-block? In years, I mean... ";
	getline(cin,response);
	for (int i=0;i<response.length();i++) { //Let's verify character-by-character...
		//Let's see if it's a numeric character!
		if (isdigit(response[i])) continue;
		
		//If we got here, then the character is something other than what's approved
		assumeGood=false;
		break;
	}
	if (assumeGood) {
		stream<<response;
		stream>>value;
		cout<<"Hmmm... I guess it *could* be "<<value<<" years old..."<<endl;
		return;
	}
	cout<<"Ten. Ten... times."<<endl;
}

void additional() {
	stringstream s;
	s.str("We can preload a string, if we like.");
	s<<"Y";
	string st=s.str();
	cout<<st<<endl;
}

int main() {
	ss();
	additional();
}
