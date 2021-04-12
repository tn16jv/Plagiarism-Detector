#Ornithology makefile!

CC=g++
CFLAGS=-Wall -c

all:	Bird.o Raptor.o

Bird.o:	Bird.cpp ornithology.h
	$(CC) $(CFLAGS) -c Bird.cpp -I ../feathers
#Of course, the -I could have also been put into a macro variable

Raptor.o:	Raptor.cpp Raptor.h ornithology.h
	$(CC) $(CFLAGS) -c Raptor.cpp  -I../feathers

clean:
	@rm -f *.o
