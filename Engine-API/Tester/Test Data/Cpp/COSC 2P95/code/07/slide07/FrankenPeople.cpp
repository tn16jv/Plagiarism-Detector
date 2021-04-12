#include <iostream>
#include <string>
/*This is all effectively the same classes from the previous example.
 *The only change is that, now, we can see what constructors are being called, and when.
 */

//First, our base class:
class Person {
public:
	std::string name;
	int height;
	//Default constructor:
	Person(std::string name="", int height=170): name(name), height(height) {
		std::cout<<"I am not an animal! I am a Person being!"<<std::endl;
	}
	//Remember: a 'const' member function promises to not modify the object
	std::string getName() const {return name;}
	int getHeight() const {return height;}
};

//Now, our first derived class:
class Student : public Person { //We'll explain the 'public' part soon
public:
	std::string stdNum;
	float tuitionOwed;
	Student(std::string stdNum="", float tuitionOwed=0.0f) : stdNum(stdNum), tuitionOwed(tuitionOwed) {
		std::cout<<"Oh joy. More examples. Nah, I didn't have anything better to do..."<<std::endl;
	}
	std::string getStdNum() const {return stdNum;}
	float getTuitionOwed() const {return tuitionOwed;}
	void payTuition(float amount) {tuitionOwed-=amount;}
	void enroll() {std::cout<<"Learning is fun!"<<std::endl;}
};

//We can chain the inheritance, and define a subtype of our subtype:
class GraduateStudent : public Student {
public:
	GraduateStudent() {
		std::cout<<"What is sleep? Did I used to have that? I don't remember."<<std::endl;
	}
	void writeThesis() {std::cout<<"I don't wanna!"<<std::endl;}
};

//This one is a derived class of Person, but not related to students
class Lecturer : public Person {
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
	Student s("2135754",3700);
	s.name="Robert";
	std::cout<<std::endl<<"\tAbout to instantiate GraduateStudent:"<<std::endl;
	GraduateStudent g;
	std::cout<<std::endl<<"\tLecturer time, I guess:"<<std::endl;
	Lecturer ell;
	ell.name="Lrae";
}
