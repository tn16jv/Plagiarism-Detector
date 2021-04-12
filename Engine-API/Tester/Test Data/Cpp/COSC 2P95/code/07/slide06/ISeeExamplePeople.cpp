#include <iostream>
#include <string>
/*Of course, we *could* simply write several independent classes, but there'd be a few problems:
 *	1. We'd have redundant code
 *	2. Related to #1, we'd have the 'update problem'
 *	3. We get no polymorphism
 *	4. We make Santa Claus cry
 */

//First, our base class:
class Person {
public:
	std::string name;
	int height;
	//Default constructor:
	Person(std::string name="", int height=170): name(name), height(height) {}
	//Remember: a 'const' member function promises to not modify the object
	std::string getName() const {return name;}
	int getHeight() const {return height;}
};

//Now, our first derived class:
class Student : public Person { //We'll explain the 'public' part soon
public:
	std::string stdNum;
	float tuitionOwed;
	Student(std::string stdNum="", float tuitionOwed=0.0f) : stdNum(stdNum), tuitionOwed(tuitionOwed) {}
	std::string getStdNum() const {return stdNum;}
	float getTuitionOwed() const {return tuitionOwed;}
	void payTuition(float amount) {tuitionOwed-=amount;}
	void enroll() {std::cout<<"Learning is fun!"<<std::endl;}
};

//We can chain the inheritance, and define a subtype of our subtype:
class GraduateStudent : public Student {
public:
	void writeThesis() {std::cout<<"I don't wanna!"<<std::endl;}
};

//This one is a derived class of Person, but not related to students
class Lecturer : public Person {
public:
	void lecture() {std::cout<<"Blah blah blah, yackety schmackety, cool glass of OJ..."<<std::endl;}
};

int main() {
	Person p("Moleman");
	Student s("2135754",3700);
	s.name="Robert";
	GraduateStudent g;
	Lecturer ell;
	ell.name="Lrae";
	std::cout<<"We have: "<<p.getName()<<", "<<s.getName()<<", "<<g.getName()<<", and "<<ell.getName()
		<<"."<<std::endl;
	g.writeThesis();
	std::cout<<g.getHeight()<<std::endl;
	ell.lecture();
}
