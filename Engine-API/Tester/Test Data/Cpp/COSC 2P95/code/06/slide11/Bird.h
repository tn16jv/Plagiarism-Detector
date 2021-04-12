//Specification for Bird class
class Bird {
private:
	double wingspan;
	Bird(int ws) {}; //Unrelated, but have we picked up on what this does?
public:
	Bird(double wingspan=37.0); //Note that default parameters should be set in the specification
	void flap();
	void tweet();
	void transformIntoMechaBird();
	double getWingspan() {return wingspan;}//Really simple stuff can still be included
};
