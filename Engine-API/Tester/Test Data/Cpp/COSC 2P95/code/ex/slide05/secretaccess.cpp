#include <iostream>

//static at the file scope hides variables and functions from the linker
static int geese=8;//We don't usually need to hide variables as much; it just guards against extern
static int quack() {std::cout<<"quack!"<<std::endl;return 8;}


int tweet() {std::cout<<"tweet!"<<std::endl;return quack();}