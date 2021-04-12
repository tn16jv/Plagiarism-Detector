#include <iostream>
#include <string>
/*The way we can let a derived class make use of a base class's initialization is by adding extra parameters to the
 *derived class's constructor, and then passing it along in the derived class's constructor's member initialization list.
 */

//First, our base class:
class Person {
public:
	std::string name;
	int height;
	//Default constructor:
	Person(std::string name="", int height=170): name(name), height(height) {
		std::cout<<"Person Name: "<<name<<",\tHeight: "<<height<<std::endl;
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
	Student(std::string name="", std::string stdNum="",float tuitionOwed=0.0f, int height=170) 
		: stdNum(stdNum), tuitionOwed(tuitionOwed), Person(name,height) {
		std::cout<<"Student Number: "<<stdNum<<"\tTuition Owed: "<<tuitionOwed<<std::endl;
		std::cout<<"Inherited Base Name: "<<name<<"\tHeight: "<<height<<std::endl;
	}
	std::string getStdNum() const {return stdNum;}
	float getTuitionOwed() const {return tuitionOwed;}
	void payTuition(float amount) {tuitionOwed-=amount;}
	void enroll() {std::cout<<"Learning is fun!"<<std::endl;}
};

//We can chain the inheritance, and define a subtype of our subtype:
class GraduateStudent : public Student {
public:
	//GraduateStudent(float tuitionOwed=0.0f) : tuitionOwed(tuitionOwed) //This would not be legal
	GraduateStudent(float tuitionOwed=0.0f) : Student("","",tuitionOwed) {
		std::cout<<"Graduate Tuition Owed: "<<tuitionOwed<<std::endl;
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
	Student s("Robert","2135754",3700);
	std::cout<<std::endl<<"\tAbout to instantiate GraduateStudent:"<<std::endl;
	GraduateStudent g(3.50);
	std::cout<<std::endl<<"\tLecturer time, I guess:"<<std::endl;
	Lecturer ell;
	ell.name="Lrae";
}
