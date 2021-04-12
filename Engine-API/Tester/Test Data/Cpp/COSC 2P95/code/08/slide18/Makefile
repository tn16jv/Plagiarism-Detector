#Hey! I'm a comment!

#Let's start with some basic variable declarations
#We can now use this as $(CC)
CC=g++

#Compile flags might be defined to avoid having to type the same thing to each compile line
CFLAGS=-Wall -c

#It wouldn't be unheard of to add an extra DEBUG, with a -g here, but we aren't using
#the debugger, so... nah. No need for it in this example.

#Though you could use whatever variable names you like, there are commonly-recommended
#ones. You can read Gnu's list here:
#http://www.gnu.org/software/make/manual/make.html#Implicit-Variables


#The first target to not begin with a period will be the default target, irrespective
#of what it's named.
#That said, let's still use 'all'
#This will compile the necessary libraries, as well as both 'applications' (trivial
#though they may be)
#A rule starts with a target name and a colon, followed by dependencies
all:	libfeathers libornithology libpaleontology clients

#We COULD also include targets for constructing our libraries (static or dynamic)
#If we actually thought we'd be using them separately, they'd likely do so in a
#separate makefile
#Be careful when doing so, though. Recall that, for shared objects, you need to
#compile differently!

clients: libfeathers libornithology libpaleontology client/clientBirdies.cpp client/clientRAWR.cpp
	$(CC) -o birdDemo client/clientBirdies.cpp feathers/*.o ornithology/*.o -I./feathers/ -I./ornithology/
	$(CC) -o dinoDemo client/clientRAWR.cpp feathers/*.o ornithology/Bird.o paleontology/*.o -I./feathers -I./ornithology -I./paleontology
	@echo "Complete!"

#We're going to use a recursive make
libfeathers:
	$(MAKE) -C feathers
#Note that recipe commands MUST be tabbed!

libornithology:
	$(MAKE) -C ornithology

libpaleontology:
	$(MAKE) -C paleontology

#If we had an 'install' target, then we'd probably use it as such:
#First, it's expected that the user has already build the application
#	i.e. typical usage is make; make install
#We'd copy all of the files into a normal installation folder (this typically requires superuser)
#We'd also possibly include a symlink, e.g. in the /bin/ directory, to the actual executable

#We won't, but hypothetically, what if we could end up with a 'clean' file/folder?
#If we were worried about that, we'd add:
#.PHONY: clean
#Alternatively, we could have clean depend on another target that we know definitely won't
#ever exist
clean:
	$(MAKE) -C feathers clean
	$(MAKE) -C ornithology clean
	$(MAKE) -C paleontology clean
	@rm -f *.o
	@rm -fr lib
	@rm -f birdDemo
	@rm -f dinoDemo
	@echo "All done!"
