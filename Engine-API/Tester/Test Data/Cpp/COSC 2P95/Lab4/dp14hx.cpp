#include <iostream>
#include <cmath>
#include <iomanip>
#include <fstream>
using namespace std;

int funPrompt(){
	double op;
	int res;
	bool valid;
	do{
		valid=true;
		cerr<<"Select your function:"<<endl;
		cerr<<"1. cos(z)(1/2 sin(x) + 1/2 cos(y))"<<endl;
		cerr<<"0. Quit"<<endl;
		cin>>op;
		res = op;
		if (res<0 || res>1||cin.fail()){
			cerr<<"Please enter a 0 or 1.\n"<<endl;
			cin.clear();
			cin.ignore(100,'\n');
			valid=false;
		}
	}while(!valid);
	return res;
}
int framePrompt(){
	double op;
	int res;
	bool valid;
	do{
		valid=true;
		cerr<<"Number of frames: ";
		cin>>op;
		res = op;
		if(res<=0||cin.fail()){
			cout<<"Please enter a number greater than 0.\n"<<endl;
			cin.clear();
			cin.ignore(100,'\n');
			valid=false;
		}
	}while(!valid);
	return res;
}
int gradPrompt(){
	double op;
	int res;
	bool valid;
	do{
		valid=true;
		cerr<<"Number of graduations per axis: ";
		cin>>op;
		res = op;
		if(res<=0||cin.fail()){
			cout<<"Please enter a number greater than 0.\n"<<endl;
			cin.clear();
			cin.ignore(100,'\n');
			valid=false;
		}
	}while(!valid);
	return res;
}

int mapPrompt(){
	double op;
	int res;
	bool valid;
	do{
		valid=true;
		cerr<<"(0) Bitmap or (1) Values? ";
		cin>>op;
		res = op;
		if (res<0||res>1||cin.fail()){
			cout<<"Please enter 0 or 1.\n"<<endl;
			cin.clear();
			cin.ignore(100,'\n');
			valid=false;
		}
	}while(!valid);	
	return res;
}

double fillXDom(double i, double g){
	double x=-4;
	double grad=10/g;
	return x+(grad*i);
}

double fillYDom(double i, double g){
	double y=-12;
	double grad=17/g;
	return y+(grad*i);
}

double fillGrid(double x, double y, double z, int fun){
	double xd, yd, xy;
	switch(fun){
		case 3:
			xd = sin(x);
			yd = cos(y);
			return xd*yd;
		case 2:
			xd = sin(x);
			yd = cos(0.5*y)*cos(0.5*y);
			xy = x/y;
			return xd+yd-xy;
		case 1:
			xd = 0.5*(sin(x));
			yd = 0.5*(cos(y));
			return cos(z)*(xd+yd);
		case 4:
			xd = 0.5*(sin(x));
			yd = x*(cos(3*y));
			return xd+yd;
	}
}

double shiftVal(int n, double val){
	return 127.5*(n+val);
}

double fillZDom(int i, int frames){
	double inc = 6.28318530718/frames;
	return i*inc;
}

int main(){
	int size, fun, map, grad, frames;
	do{
		fun =funPrompt();
		if (fun!=0){
			grad = gradPrompt();
			size = grad*grad;
			double xDom[grad];
			double yDom[grad];
			for (int i=0; i<grad; i++){
				xDom[i]=fillXDom(i, grad);
				yDom[i]=fillYDom(i, grad);
			}
			frames = framePrompt();
			double zDom[frames];
			for (int i=0; i<frames; i++){
				zDom[i]=fillZDom(i, frames);
			}
			ofstream myfile;
			double grid[size];
			for (int f=0; f<frames; f++){
				char num[] = {"anim0000.pgm"};
				if (f<10){
					num[7] = char(48+f);
				}
				else if (f<100){
					double g = f/10;
					int h = f-(int(g)*10);
					num[6] = char(48+int(g));
					num[7] = char(48+h);
				}
				myfile.open (num);
				int j=0;
				char s = 32;
				char n = 10;
				myfile<<"P2"<<n<<"# "<<num<<n<<grad<<s<<grad<<n<<255<<n;
				int x=0, y=0;
				for (int i=0; i<size; i++){
					grid[i]=fillGrid(xDom[x],yDom[y],zDom[f],fun);
					grid[i]=shiftVal(1, grid[i]);
					if (x==grad-1){
						x=0;
						y++;
					}
					else{x++;}
					myfile<<int(grid[i])<<s;
					j++;
					if(j==grad){
						myfile<<n;
						j=0;
					}
				}
				myfile.close();	
			}
		}
	}while(fun != 0);
}