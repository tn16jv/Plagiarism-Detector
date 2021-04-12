#include <iostream>
#include <fstream>
#include <string>
using namespace std;

//Note: don't trust this to do a byte-to-byte copy!
void output() {
	ofstream of("data.txt",ios::app);//We open it to append to it. If it doesn't exist, it'll be created
	while (cin) {//streams can be used as a conditional test, equivalent to 'good'
		string line;
		getline(cin,line);
		of<<line<<endl;
		of.flush();//Probably overkill for something like this
	}
	
	of.close();
}

//Let's read the file back now!
void input() {
	ifstream inf("data.txt",ios::binary);
	while (inf) {
		char byte;
		inf.read(&byte,1);//<-Misused. But I'm lazy
		cout<<byte;
	}
}

//There are several file modes one can use. Take a gander: http://en.cppreference.com/w/cpp/io/ios_base/openmode

int main() {
	//First, let's read from the keyboard and write the result out to a file
	//output();
	input();
}
