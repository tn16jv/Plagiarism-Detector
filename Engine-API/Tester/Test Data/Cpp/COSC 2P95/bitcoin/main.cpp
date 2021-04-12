#include <iostream>
#include <unistd.h>
#include <cstdlib>
#include <signal.h>
#include <pthread.h>
#include "main.h"

volatile int target;
volatile bool continuing;
static const int NUMTHREADS = 4;
pthread_mutex_t olock;
pthread_mutex_t cout_lock;
volatile int occupied;

void* hashwork(void* unnecessary){
	unsigned long nonce;
	unsigned int hash;
	while(continuing){
		genULong(nonce);
		hash=calcHash(nonce);
		if(leadingZeroes(hash) >= target){
			pthread_mutex_lock(&cout_lock);
			std::cout<<"Mining successful:" <<std::endl;
			std::cout<<"\t";
			printSixtyFour(nonce);
			std::cout<<"\tHashed: ";
			printThirtyTwo(hash);
			std::cout<<std::endl;
			pthread_mutex_unlock(&cout_lock);
		}
	}
	pthread_mutex_lock(&olock);
	occupied--;
	std::cout << "Thread is done.." << std::endl;
	pthread_mutex_unlock(&olock);
}
void printThirtyTwo(unsigned int word) {
	for (int i=0;i<32;i++)
		std::cout<<(((0x80000000>>i)&word)?1:0);
	std::cout<<std::endl;
}
void printSixtyFour(unsigned long word) {
	for (int i=0;i<64;i++)
		std::cout<<(((0x8000000000000000>>i)&word)?1:0);
	std::cout<<std::endl;
}
unsigned int calcHash(unsigned long nonce) {
	unsigned int hash=0;
	for (int i=15;i>=0;i--) {
		hash=hash*17+((nonce>>(4*i))&0x0F);
	}	
	return hash;
}
void genULong(unsigned long &nonce) {
	nonce=0;
	for (int i=63;i>=0;i--) {
		nonce<<=1;
		nonce|=random()%2;
	}
}

int leadingZeroes(unsigned int value) {
	for (int i=0;i<32;i++)
		if ((value>>(31-i))&1) return i;
	return 32;
}

int menu(){
	int c;
	std::cout<<"Enter number of leading zeroes to look for (0 to quit)"<<std::endl;
	std::cin >> c;
	return c;
}

void interrupted(int sig){  // sig does not matter for us.
	std::cout << "\n Computations are complete.\n Stopping..." << std::endl;
	continuing = false;
}

int main(){
	srand(time(NULL));

	pthread_t ct[NUMTHREADS]; //thread pool

	if (signal(SIGINT,interrupted)==SIG_ERR) {
		std::cout<<"Unable to change signal handler."<<std::endl;
		return 1;
	}

	int choice;
	while(true){
		choice = menu();
		std::cout<<choice<<std::endl;
		if(choice <= 0) break;
		target = choice;
		continuing = true;
		for(int i=0; i<NUMTHREADS; i++){
			pthread_mutex_lock(&olock);
			pthread_create(&ct[i], NULL, &hashwork, NULL);
			occupied++;			
			pthread_mutex_unlock(&olock);
		}
		while(occupied>0)sleep(1);
	}

	std::cout<<"we Did it"<<std::endl;
	return 0;
}
