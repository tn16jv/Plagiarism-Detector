#include <iostream>
#include <cmath>
#include <iomanip>
using namespace std;
int funPrompt(){
	double op;
	int res;
	bool valid;
	do{
		valid=true;
		cerr<<"Select your function:"<<endl;
		cerr<<"1. sin(x)cos(y)"<<endl;
		cerr<<"2. sin(x)+cos^2(y/2)-x/y"<<endl;
		cerr<<"3. 1/2 sin(x) + 1/2 cos(y)"<<endl;
		cerr<<"4. 1/2 sin(x) + xcos(3y)"<<endl;
		cerr<<"0. Quit"<<endl;
		cin>>op;
		res = op;
		if (res<0 || res>4||cin.fail()){
			cerr<<"Please enter a value between 0 & 4.\n"<<endl;
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

double fillGrid(double x, double y, int fun){
	double xd, yd, xy;
	switch(fun){
		case 1:
			xd = sin(x);
			yd = cos(y);
			return xd*yd;
		case 2:
			xd = sin(x);
			yd = cos(0.5*y)*cos(0.5*y);
			xy = x/y;
			return xd+yd-xy;
		case 3:
			xd = 0.5*(sin(x));
			yd = 0.5*(cos(y));
			return cos(0)*(xd+yd);
		case 4:
			xd = 0.5*(sin(x));
			yd = x*(cos(3*y));
			return xd+yd;
	}
}

double shiftVal(int n, double val){
	return n+val;
}

int main(){
	int size, fun, map, grad;
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
			double grid[size];
			int x=0, y=0;
			for (int i=0; i<size; i++){
				grid[i]=fillGrid(xDom[x],yDom[y],fun);
				grid[i]=shiftVal(1, grid[i]);
				if (x==grad-1){
					x=0;
					y++;
				}
				else{x++;}
			}
			map = mapPrompt();
			int h=0;
			int j=0;
			for (int i=0; i<size; i++){
				if(map==0){
					cout<<(grid[i]<0?"X":"O");
				}
				else{
					cout<<"("<<setprecision(2)<<grid[i]<<")";
				}
				j++;
				if(j==grad){
					cout<<"\n";
					h++;
					j=0;
				}
			}
		}
	}while(fun != 0);
}