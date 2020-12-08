# TapeDriveStream
This is a solution to read a binary file sequentially. Providing also, a custom
way to structure the data. You can define your own structure by extending the
base class. This lib already provides classes for reading some primitives like
int32, int64, and .Net 128bit-decimals.

The reading and writting are buffered, lowering IO accesses.