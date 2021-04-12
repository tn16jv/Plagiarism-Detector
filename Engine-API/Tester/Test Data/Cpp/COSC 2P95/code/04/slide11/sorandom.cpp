#include <iostream>
#include <fstream>
#include <string>
using namespace std;


int main() {
	fstream io;
	io.open("sample.txt",ios::in|ios::out|ios::ate);//Open for IO, start cursor at end of file (of course, IO is the default anyway)
	string s;
	io>>s;
	cout<<s<<endl;//THIS DOES NOTHING! (because we're already at the end
	io.clear();
	
	io.seekg(0,ios::beg);//'get' position is for reading
	io>>s;
	cout<<s<<endl;//We can get the first word now!
	
	io.seekp(-10,ios::end);//'put' position is for writing
	io<<"steak";
	//There's also a ios::cur, to locate relative to the current position
	cout<<"Currently at: "<<io.tellg()<<endl;
	cout<<"Also at: "<<io.tellp()<<endl;
	//If retaining positions in variables, use the streampos type
	
	//When opening in binary mode, consider using read and write (both of which expect char[] buffers)
	
	//Have I linked this yet?
	//	http://www.cplusplus.com/doc/tutorial/files/
	
	io.close();
}
