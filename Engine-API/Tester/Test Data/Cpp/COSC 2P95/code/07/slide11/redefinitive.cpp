#include <iostream>
#include <string>
/*By combining what we've already learned, we can increase, expand, redefine, or hide functionality.
 */

class Person {
public:
	std::string name;
	int height;
	Person(std::string name="", int height=170): name(name), height(height) {}
	std::string getName() const {return name;}
	int getHeight() const {return height;}
	
	friend std::ostream& operator<<(std::ostream &out, const Person &p) {
		//Remember, friend functions aren't members, and there's no 'this'; hence the 'p.' to access 'getName'
		out<<"Hey. I'm "<<p.getName()<<".";
		return out;
	}
};

class Student : public Person {
public:
	std::string stdNum;
	float tuitionOwed;
	Student(std::string name="", std::string stdNum="",float tuitionOwed=0.0f, int height=170) 
		: stdNum(stdNum), tuitionOwed(tuitionOwed), Person(name,height) {}
	
	std::string getStdNum() const {return stdNum;}
	float getTuitionOwed() const {return tuitionOwed;}
	void payTuition(float amount) {tuitionOwed-=amount;}
	void enroll() {std::cout<<"Learning is fun!"<<std::endl;}
	
	friend std::ostream& operator<<(std::ostream &out, const Student &s) {
		out<<"Greetings! My name is "<<s.name<<".";
		//If we wanted to access the Person version here, we could just say: out<<static_cast<Person>(s)
		return out;
	}
};

class GraduateStudent : public Student {
private:
	//This graduate student's too antisocial to acknowledge having a name
	using Person::getName;
public:
	GraduateStudent(std::string name="", std::string stdNum="",float tuitionOwed=0.0f, int height=170)
	: Student(name,stdNum,tuitionOwed,height) {}
	
	void writeThesis() {std::cout<<"I don't wanna!"<<std::endl;}
	void payTuition(float amount) {
		std::cout<<"Are you kidding me?!? I thought the school paid ME; not vice versa!"<<std::endl;
		Student::payTuition(amount);
		std::cout<<"There! Fine! Done!"<<std::endl;
	}
};

class Lecturer : public Person {
public:
	Lecturer(std::string name):Person(name) {}
	
	void lecture() {std::cout<<"Blah blah blah, yackety schmackety, cool glass of OJ..."<<std::endl;}
	std::string getName() {return "If I tell you that, you'll put negative ratings on ratemyprofessors!";}
};

int main() {
	Person p("Moleman");
	Student s("Robert","2135754",3700);
	GraduateStudent g("B. Zebub","6666666",3.50,165);
	Lecturer ell("Lrae");
	std::cout<<"Person's record: "<<p<<std::endl;
	std::cout<<"Person's name: "<<p.getName()<<std::endl;
	
	std::cout<<"Student's record: "<<s<<std::endl;
	std::cout<<"Student's name: "<<s.getName()<<std::endl;
	
	//Can't do this, because getName is 'inaccessible'!
	//std::cout<<"Graduate Student's name: "<<g.getName()<<std::endl;
	
	//Considering Person's <<uses getName(), which getName() do you expect here?
	std::cout<<"Lecturer's record: "<<ell<<std::endl;
	std::cout<<"Lecturer's name: "<<ell.getName()<<std::endl;
	
	std::cout<<"Student pays 9001 tuition."<<std::endl;
	s.payTuition(9001);
	std::cout<<"Graduate Student pays 3.50 tuition."<<std::endl;
	g.payTuition(3.50);
}
