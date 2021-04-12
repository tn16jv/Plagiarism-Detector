#include <iostream>
#include <unistd.h>
#include <cstdlib>
#include <signal.h>
#include <pthread.h>
#include <cmath>
#include "climber.h"

volatile double gX, gY;  //coordinates of the global minimum
volatile double minimum = 10000000; //global minimum
volatile bool continuing; //continue running program
int numthreads; //number of threads in thread pool
pthread_mutex_t olock;
pthread_mutex_t lock;
volatile int occupied; //indicates if thread is active

//This is thhe search for the global min that each thread performs
void* climb(void* unnecessary){
	double x=0;  //x pos of hill-climber
	double y=0;  //y pos of hill-climber
	double xR;   //x coord of next move
	double yR;   //y coord of next move
	double z;    //height calculated for each possible move
	double height=0; //height of hill-climber
	bool moved;  //whether or not the thread used one of 4 possible moves
	while(continuing){
		moved = false;
		//Get the lowest height of 4 possible random moves from current position
		for(int i=0; i<4; i++){
			
			//Generate new x coord
			xR = (double)((int)(fRand(-5.0,5.0)*10))/10;
			if (x+xR > 512)
				xR = 512;
			else if (x+xR < -512)
				xR = -512;
			else
				xR = x+xR;
			
			//Generate new y coord
			yR = (double)((int)(fRand(-5.0,5.0)*10))/10;
			if (y+yR > 512)
				yR = 512;
			else if (y+yR < -512)
				yR = -512;
			else
				yR = y+yR;
			
			//Check height of new coords
			z = calcHeight(xR,yR);
			if(z < height){		//change height if move is valid
				height = z;
				x = xR;
				y = yR;
				moved = true;
			}
		}
		//Randomize a complete new position if no moves are valid
		if(!moved){
			x = (rand() % 1025) - 512;
			y = (rand() % 1025) - 512;
			height = calcHeight(x,y);
		}
		//Lock the thread and change global min if desired
		pthread_mutex_lock(&lock);
		if(height < minimum){
			minimum = height;
			gX = x;
			gY = y;
		}
		pthread_mutex_unlock(&lock);
		
	}
	pthread_mutex_lock(&olock);
	occupied--;
	std::cout << "\nThread is done.." << std::endl;
	pthread_mutex_unlock(&olock);
}

//Produces the height of a specified coordinate in the Egg-Holder function
double calcHeight(double x, double y){
	double a = std::abs(x/2+(y+47));
	a = std::sqrt(a);
	a = std::sin(a);
	a = -1*(y+47)*a;
	double b = std::abs(x-(y+47));
	b = std::sqrt(b);
	b = std::sin(b);
	b = x*b;
	return a-b;
}

//Produces a random double within a specified range
double fRand(double fMin, double fMax){
    double f = (double)rand() / RAND_MAX;
    return fMin + f * (fMax - fMin);
}

//Prompts user to enter number of hill climbers or quit
int menu(){
	int c;
	std::cout<<"\nEnter number of hill climbers (8 = max & 0 = quit): ";
	std::cin >> c;
	return c;
}

//Handles the SIGINT signal
void interrupted(int sig){  // sig does not matter for us.
	std::cout << "\n Global Best Height: " << minimum << std::endl;
	std::cout << "\n X Position: " << gX << std::endl;
	std::cout << "\n Y Position: " << gY << std::endl;
	continuing = false;
}

//Handles the SIGUSR1 signal
void probe(int sig){
	std::cout << "\n Current Global Minimum: " << minimum << std::endl;
}

int main(){
	srand(time(NULL));
	
	std::cout<<"About to commence; PID: "<<getpid()<<std::endl;
	
	pthread_t ct[numthreads]; //thread pool
	
	if (signal(SIGINT,interrupted)==SIG_ERR) {
		std::cout<<"Unable to change signal handler."<<std::endl;
		return 1;
	}
	
	if (signal(SIGUSR1,probe)==SIG_ERR) {
		std::cout<<"Unable to change signal handler."<<std::endl;
		return 1;
	}

	int choice;
	while(true){
		choice = menu();
		if(choice <= 0) break;
		numthreads = choice;
		continuing = true;
		for(int i=0; i<numthreads; i++){
			pthread_mutex_lock(&olock);
			pthread_create(&ct[i], NULL, &climb, NULL);
			occupied++;			
			pthread_mutex_unlock(&olock);
		}
		while(occupied>0)sleep(1);
	}

	return 0;
}
