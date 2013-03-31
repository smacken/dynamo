## Dynamo

A thought experiment into a couple of things:
- DLR with ruby. Creating a ruby bridge and bootstrapping with a rubyhost as a gateway between ruby and .Net
- MEF plugin architecture. Providing a way for functionality to be delivered via ruby without knowledge that it's ruby
- Push based hub and spoke.

Ruby host
I would like for it to detect and load into scope any ruby files to be bridged. Also because they are 
going to be used in the .Net world there is a little bootstrapping to make lives a little easier.
Unmangling the names means that when you combine with dynamic syntatic sugar and combining duck typing
with interfaces on the .Net side you can have ruby classes deliver functionality seemlessly. 

Convention based plugin architecture using System.Composition means that .Net components can be so loosly coupled that they dont even care if the functionality happens to care from another .Net module, or be it ruby.

Currying. Create a SignalR push curry that can be combined for ruby classes matching hubs to deliver using sockets. A hub and spoke architecture allows push based comms between nodes.