#include <iostream>
using namespace std;


struct Employee {
	char name[80]; //Of course, you could use the 'string' type
	char id[8];
	union {double hourly; double salary;} pay;
	enum {HOURLY,SALARIED} type;
	bool stillEmployed;
};

//We could try using a pointer...
void tryToPayMore(Employee *e, double amt) {
	//(*e).pay.hourly+=amt; //Explicitly dereferencing is okay...
	e->pay.hourly+=amt; //But this form is provided to us as shorthand
}

int main() {
	Employee emps[]={
		{"Chester","1337898",{12.50},Employee::HOURLY,true},
		{"Meghan","7334590",{62250.88},Employee::SALARIED,true},
		{"Goliath","1",{0.01},Employee::HOURLY,false}};
	cout<<emps[1].name<<"\t"<<(emps[1].type?emps[1].pay.salary:emps[1].pay.hourly)<<"\t"<<emps[1].stillEmployed<<endl;
	
	
	tryToPayMore(&emps[0],2.00);
	cout<<emps[0].name<<"\t"<<(emps[0].type?emps[0].pay.salary:emps[0].pay.hourly)<<"\t"<<emps[0].stillEmployed<<endl;
	tryToPayMore(&emps[0],80.00);
	cout<<emps[0].name<<"\t"<<(emps[0].type?emps[0].pay.salary:emps[0].pay.hourly)<<"\t"<<emps[0].stillEmployed<<endl;
	
	//Whaaat?!?
}
