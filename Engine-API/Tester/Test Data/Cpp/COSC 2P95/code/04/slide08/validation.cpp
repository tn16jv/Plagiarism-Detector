#include <iostream>
#include <iomanip>
using namespace std;
//Note that you might actually want to stop using that namespace, as they're separate for a reason...

void wrongType() {
	int val;
	cout<<"Please enter a number. (wink wink) ";
	cin>>val;
	cout<<cin.fail()<<endl;
}

void recover() {
	int val;
	do {
		cout<<"Please enter a number. (wink wink) ";
		cin>>val;
	if (cin.good()) break;
		cin.clear();//We know it failed. That's okay.
		cin.ignore(100,'\n');//The contents of the stream aren't useful. We need to discard them.
	} while (true);
	//You could .ignore here as well, if you didn't want extra text to be considered for the next request
	//But, would that be enough if you entered *a lot* of extra data?
}
//There are other tests available. There's a helpful table at the end of here:
// http://en.cppreference.com/w/cpp/io/ios_base/iostate
//And you can see how to explicitly read/clear/set state here:
// http://en.cppreference.com/w/cpp/io/basic_ios

void stringBased() {
	string response;
	bool assumeGood=true;
	cout<<"What's your favourite colour? ";
	getline(cin,response);
	for (int i=0;i<response.length();i++) { //Let's verify character-by-character...
		//Let's see if it's an alphanumeric character!
		if (isalpha(response[i])) continue;
		
		//We'll permit spaces, but isspace might not be ideal. 32 is the ASCII code for a space.
		if (response[i]==32) continue;
		
		//If we got here, then the character is something other than what's approved
		assumeGood=false;
		break;
	}
	cout<<(assumeGood?"Yay! Good answer!":"sigh...")<<endl;
	//Of course, if you wanted to force repetition until valid input, just slap it all into another loop
}
//Of course, we have lots of other tests. e.g. iscntrl, isalphanum, isgraph, isprint, isspace, isdigit, isxdigit, ispunct...

int main() {
	//wrongType();
	//recover();
	stringBased();
	
	//If we wanted to, we could combine the two concepts a bit: read into a string, validate, and then restream into correct type
}
