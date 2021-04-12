#include <iostream>
#include <iomanip>
using namespace std;
//Note that you might actually want to stop using that namespace, as they're separate for a reason...
void basicIn() {
	int k; double d;
	string s1,s3;
	char s2[20];
	cout<<"Give me a number between 1 and 10 (inclusive): ";
	cin>>k;
	cout<<"You chose: "<<k<<endl;
	cout<<"What do you think the temperature is in here? ";
	cin>>d;
	cout<<"Totally feels like "<<d<<" to me, too"<<endl;
	
	//This part can get a little screwy...
	cout<<"I think I need three words. What's the first? ";
	cin>>s1;
	cout<<"What's the second? ";
	cin>>s2;
	cout<<"And what's the third? ";
	cin>>s3;
	cout<<"You chose: "<<s1<<s2<<s3<<endl;
	cout<<"By the way, the second string is "<<sizeof(s2)<<" bytes"<<endl;
	
	cin.clear();cin.ignore(100,'\n');//This part's probably just being paranoid
	cout<<"Maybe... try that again? ";
	cin.getline(s2,20); //How many characters do you figure this can hold?
	cout<<"Okay, so this time: "<<s2<<" ("<<cin.gcount()<<")"<<endl;
	//We also have a getline() that uses a 'string' type as the string buffer
}

void boolIn() {
	bool b;
	cout.setf(ios::boolalpha);
	cin.setf(ios::boolalpha);
	cout<<"Try telling me false!"<<endl;
	cin>>b;
	cout<<b<<endl;
	cout<<"Try telling me true!"<<endl;
	cin>>b;
	cout<<b<<endl;
	cout<<"Uh... can we fix this?"<<endl;
}

void byChar() {
	char c;
	char buffer[20];
	while (cin.get(c))
		cout<<c;
	cout<<endl;
	//How do we actually... stop?
	//Note that there's also a version of get that accepts a buffer, as well as a 'read' function
	//For the most part, you shouldn't need to worry about little details
	//Most quirks between getline, get, and read deal with newline characters, so they'll *mostly* only matter for binary IO
	//	(where, for example, a 13 might just be a byte of 13)
	
	//You also have peek, unget, and putback available. You're encouraged to do a teensy bit of reading. That's fine, right?
}

int main() {
	//basicIn();
	//boolIn();
	byChar();
}
