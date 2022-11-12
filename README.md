# pubsub
Publish subscribe pattern exercise 

Pubsub.console contains the mail runnable program that shows how the generic LocalBus can be used

Pubsub.Infrastructure contains the implementation of the pub-sub pattern 
 - Supports multiple subscribers per Topic (type) - allows inheritance 
 - Supports additional content filtering 

Pubsub.Orders contains the business rules for each subscriber

Pubsub.Tests contains the tests for the LocalBus 

Tests for the Pubsub.Orders project have been ommited sinse the majority of it's behavior is just mocking a Demo workflow:

OrderPublisher 
 - generates fake orders - important are the number of items, and the hub destination

OrderPacker 
 - packs orders for delivery - changes the order number of items to a units occupied in truck property

PackageDispatcher 
 - this loads the packages in the truck per capacity.
 - this will subscribe also using content filtering on the bus to load to specific trucks for different coutries 
