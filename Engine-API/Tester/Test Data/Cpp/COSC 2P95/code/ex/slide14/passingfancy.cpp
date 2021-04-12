#include <iostream>

struct wokka {
	int things;
	int morethings;
};

void operation(wokka *w) {
	w->morethings=w->things*2;
	w->things/=2;
}

int main() {
	wokka grobble={3,0};
	std::cout<<"Before: "<<grobble.things<<'\t'<<grobble.morethings<<std::endl;
	operation(&grobble);
	std::cout<<"After: "<<grobble.things<<'\t'<<grobble.morethings<<std::endl;
}