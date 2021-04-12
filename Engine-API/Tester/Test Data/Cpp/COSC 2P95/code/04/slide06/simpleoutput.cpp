#include <iostream>
#include <iomanip>
using namespace std;
//Note that you might actually want to stop using that namespace, as they're separate for a reason...

//Simple, basic output, like we've already seen
void basicOut() {
	cout<<"Basic Output:"<<endl;
	cout<<"Integer:\t"<<13<<"\t"<<-13<<endl;
	cout<<"Float:\t"<<12.8<<"\t"<<-12.8<<endl;
	cout<<"Character:\t"<<'B'<<"\t"<<-'B'<<"\t"<<-(-'B')<<"\t"<<(char)-(-'B')<<"\t"
		<<sizeof('B')<<"\t"<<sizeof(-'B')<<endl;
	cout<<"True:\t"<<true<<"\tFalse:\t"<<false<<endl;
}

void widthOut() {
	cout<<"Columnar Output:"<<endl;
	cout<<"One"<<setw(10)<<"Two"<<"Three"<<endl;//Don't expect the manipulator to make a permanent change here
	cout<<"Current width: "<<cout.width()<<endl;
	cout.width(10);
	cout<<"One"<<setw(10)<<"Two"<<"Three"<<endl;//Again, field width is just for the next token
	cout<<setw(10)<<left<<"One"<<setw(10)<<"Two"<<"Three"<<endl;
	cout<<setw(10)<<internal<<"One"<<setw(10)<<"Two"<<"Three"<<endl;//Not interesting, right?
	cout<<setw(10)<<13<<setw(10)<<internal<<-13<<-8<<endl;//Look at this one reeeally carefully!
	//Note that justification *does* persist until reset
	
	cout<<setfill('~');
	cout<<setw(10)<<right<<"One"<<setw(10)<<"Two"<<"Three"<<endl;
	cout<<setw(10)<<internal<<"One"<<setw(10)<<"Two"<<"Three"<<endl;
	cout<<setw(10)<<internal<<13<<setw(10)<<internal<<-13<<-8<<endl;
	cout.width(10);cout.unsetf(ios::internal);cout.setf(ios::left);cout<<"hi"<<endl;//Note 'turning off' the internal
	cout.setf(ios::right,ios::adjustfield);cout<<setw(10)<<"oh"<<endl;//If we specify the format group, no need to turn left off
}

void dataOut() {
	cout.setf(ios::boolalpha);//No need for a format group here; it's either on or off
	cout<<true<<false<<endl;
	cout<<noboolalpha<<false<<true<<endl;//Aww. Back to boring. :(
	cout<<showpos;
	cout<<1<<2<<3<<4<<-1<<-2<<-3<<-4<<endl;
	cout.unsetf(ios::showpos);
	cout<<1<<2<<3<<4<<-1<<-2<<-3<<-4<<endl;
	
	cout<<uppercase<<"hello"<<"class"<<"how"<<"are"<<"you?"<<endl;//Doesn't do what you think. Trust me.
	cout<<1234567.8<<endl;//THERE it is!
	cout.unsetf(ios::uppercase);
	cout<<1234567.8<<endl;//Yup. Thought so.
	
	cout<<hex;
	cout<<"Ten: "<<10<<"\tSixteen: "<<16<<"\tNegative One"<<-1<<endl;//Again, uppercase might be nice here
	cout.setf(ios::dec,ios::basefield);//Mind if I skip octal?
	
	
	cout<<showpoint<<1<<2<<3<<4<<5<<endl;
	cout<<1.<<2.<<3.<<4.<<5.<<endl;
	cout<<1.23456789<<"\t"<<"Current precision: "<<cout.precision()<<endl;
	
	cout<<setprecision(10);
	cout<<1.23456789<<9.87654321<<endl;
	cout<<fixed<<12<<"\t"<<1.2<<"\t"<<123456.7<<endl;
	cout<<scientific<<12<<"\t"<<1.2<<"\t"<<123456.7<<endl;
	//Would controlling the number of decimal places be appropriate for, say, money? Why or why not?
}



int main() {
	basicOut();
	widthOut();
	dataOut();
}
