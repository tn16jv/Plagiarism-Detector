#Feathers makefile!

CC=g++
CFLAGS=-Wall -c

all:	feathers.o

feathers.o:	feathers.cpp feathers.h
	$(CC) $(CFLAGS) -c feathers.cpp

clean:
	@rm -f *.o
#The @ means it won't echo this command to the console
#The -f is so it doesn't think it's a mistake if it can't find any .o's
#We could also say -rm *.o, to ignore the 'error'
