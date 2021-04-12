#include <iostream>

/*Using extern is pretty dang powerful. You can circumvent the need for parameters, share memory between modules, and in
 *certain limited uses inspect external values.
 *It's also usually a terrible idea:
  *	*It's far harder to trace what's going on, just by reading the code
  *	*It can present thread safety issues
  *	*If you thought side effects were problematic, externs are just... nasty
  */

extern int grobble;

int main() {
	std::cout<<"Engaging clairvoyance... "<<grobble<<std::endl;
}
