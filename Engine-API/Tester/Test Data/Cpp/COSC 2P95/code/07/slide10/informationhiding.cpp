#include <iostream>
#include <string>
/*This example introduces the 'protected' access specifier, as well as expanding on inheritance access specifiers.
 */

//First, our base class:
class Person {
private:
	int height;
protected:
	std::string name; //Derived classes can access this, but 'main' can't
public:
	//Default constructor:
	Person(std::string name="", int height=170): name(name), height(height) {
		std::cout<<"Person Name: "<<name<<",\tHeight: "<<height<<std::endl;
	}
	std::string getName() const {return name;}
	int getHeight() const {return height;}
	void setHeight(int height) {this->height=height;}
};

//Because of this inheritance access specifier, getName and getHeight are only available to this class and
//derived classes (but not 'main').
//The same *would* be true for setHeight, but... we were clever.
//The 'name' member variable is unaffected, and we still can't access 'height' directly.
class Student : protected Person {
public:
	std::string stdNum;
	float tuitionOwed;
	Student(std::string name="", std::string stdNum="",float tuitionOwed=0.0f, int height=170) 
		: stdNum(stdNum), tuitionOwed(tuitionOwed), Person(name,height) {
		std::cout<<"Student Number: "<<stdNum<<"\tTuition Owed: "<<tuitionOwed<<std::endl;
		//std::cout<<"Inherited Base Name: "<<name<<"\tHeight: "<<height<<std::endl;//We can't see height
		std::cout<<"Inherited Base Name: "<<name<<"\tHeight: "<<getHeight()<<std::endl;
	}
	std::string getStdNum() const {return stdNum;}
	float getTuitionOwed() const {return tuitionOwed;}
	void payTuition(float amount) {tuitionOwed-=amount;}
	void enroll() {std::cout<<"Learning is fun!"<<std::endl;}
	using Person::setHeight;//No parentheses; This changes the access modifier just for the setHeight method
};


//This one is a derived class of Person, but not related to students
class Lecturer : private Person {
public:
	Lecturer() {
		std::cout<<"Hilarious joke! Hilarious joke! Hilarious joke!"<<std::endl;
	}
	void lecture() {std::cout<<"Blah blah blah, yackety schmackety, cool glass of OJ..."<<std::endl;}
};

int main() {
	std::cout<<"\tAbout to instantiate Person:"<<std::endl;
	Person p("Moleman");
	std::cout<<std::endl<<"\tAbout to instantiate Student:"<<std::endl;
	Student s("Robert","2135754",3700);
	//std::cout<<"Height: "<<s.getHeight()<<std::endl;//Couldn't do this anymore
	s.setHeight(200);//Although this is just fine, because we changed the access modifier
	std::cout<<std::endl<<"\tLecturer time, I guess:"<<std::endl;
	Lecturer ell;
	
}
