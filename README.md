Implementation of the AhoCorasick search algorithm

The base class here is AhoCorasick<T, R> - an abstract class which performs a
search on an IEnumerable of type T objects, returning a type R object on
completion.  AhoCorasickString is the usual string search which is a subclass of
AhoCorasick with T of type char and R of type string.

I'll describe the AhoCorasickString class here.  It's a very thin layer over
AhoCorasick so you should be able to easily understand AhoCorasick if you
understand AhoCorasickString.  AhoCorasickString has one public uninherited
method.  Create() takes a list of strings which will be searched for.  It
creates a root node, converts the words into a List of Lists of char and passes
them to the base Install along with the original strings to be returned on
completions. That root node is returned.

Once the root node is built with Create() it can be queried with a string to be
searched using LocateParts() inherited from AhoCorasick.  LocateParts takes a
string to be searched and a boolean indicating whether they need to be sorted by
location.  The default is non-sorted.  Sorting guarantees they are sorted by
position but slows down performance a wee bit.  LocateParts returns an
IEnumerable which returns a sequence of AcResult<String> each of which has a
value located and a position where it was found.

The base class is abstract so must always be subclassed.  The two required
overrides are FactoryCreate() which creates an AhoCorasick derived object of
your derived class and Index which, given an object of type T returns an index
for that type.