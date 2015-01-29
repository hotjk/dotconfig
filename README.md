## DotConfig

* DtConfig is a simple implementation of External Configuration Store pattern.
* Node tree is a tree structure and each node contains some configuration items.
* Map the nodes to client, will give client the rights to access the configuration items in nodes.
* Client request configuration items with simple pattern.
* All transmission between client and server is encrypt with RSA.
* Settings.Web is the configuration server site.
* Setting.Client.Web is the configuration client site for demonstration.
* Modify the connection string in Web.Config for Settings.Web project.
* The tables will be create automatically when first time run Settings.Web.

## Other Components

* Sequence is an simple integer id generator with MYSQL.
* Tree is a common tree structure implementation, include MySQL persistence, and UI to maintain tree node.
