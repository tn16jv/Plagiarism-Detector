#include <iostream>
#include <unistd.h>
#include <cstdlib>
#include <signal.h>
#include <pthread.h>

static const int NUMTHREADS=4;
volatile bool continuing;
volatile int occupied;
pthread_mutex_t lock; //Our mutual exclusion lock

//At this level, this header signature can be ignored
void* busywork(void* unnecessary) {
	while (continuing) {
		std::cout<<"Thread"<<std::endl;
		usleep(100000);
	}
	pthread_mutex_lock(&lock);
	occupied--;
	std::cout<<"Exiting thread."<<std::endl;
	pthread_mutex_unlock(&lock);
}

void peek(int sig) {
	std::cout<<"Currently processing: "<<(continuing?"Yes":"No")<<std::endl;
}

//We don't really care what 'sig' is for this example
void interrupted(int sig) {
	std::cout<<"\nComputations complete.\nHalting now..."<<std::endl;
	continuing=false;
}

int main() {
	pthread_t ct[NUMTHREADS];
	
	continuing = true;
	std::cout<<"About to commence; PID: "<<getpid()<<std::endl;

	
	if (signal(SIGINT,interrupted)==SIG_ERR) {
		std::cout<<"Unable to change signal handler."<<std::endl;
		return 1;
	}
	
	if (signal(SIGUSR1,peek)==SIG_ERR) {
		std::cout<<"Unable to change signal handler."<<std::endl;
		return 1;
	}
	for (int i=0;i<NUMTHREADS;i++) {
		pthread_mutex_lock(&lock);//reserve lock
		pthread_create(&ct[i], NULL, &busywork, NULL);
		occupied++;
		pthread_mutex_unlock(&lock);//release lock
	}
	//we don't need the mutex here, because we aren't changing occupied
	while (occupied>0){
		std::cout<<"main"<<std::endl;
		sleep(1);
	}
	std::cout<<"Execution complete."<<std::endl;

}