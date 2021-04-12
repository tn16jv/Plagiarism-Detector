#Paleontology makefile!

CC=g++
CFLAGS=-Wall -c

#This should go without saying, but a BETTER way to do this is to actually use the ornithology
#library properly (e.g. as a static library)
all:	Dinosaur.o Velociraptor.o Archaeopteryx.o

Dinosaur.o:	Dinosaur.cpp paleontology.h
	$(CC) $(CFLAGS) -c Dinosaur.cpp -I ../feathers

Velociraptor.o:	Velociraptor.cpp Velociraptor.h paleontology.h
	$(CC) $(CFLAGS) -c Velociraptor.cpp  -I../feathers

Archaeopteryx.o: Archaeopteryx.cpp Archaeopteryx.h paleontology.h
	$(CC) $(CFLAGS) -c Archaeopteryx.cpp -I../feathers -I../ornithology

clean:
	@rm -f *.o
