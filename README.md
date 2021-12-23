# @bluepjs/vm

Virtual Machine for execution @bluepjs blueprints

## Installation
```
npm install -s @bluepjs/vm
```

# Ussage

VM can be used both in browser and nodejs side in same way.

VM doesn't run by itself, store or manage libraries or any states - it designed to be integrated into other projects and managing of libraries/modules/etc should be done outside.

VM doesn't care much about validity of provided libraries and doesn't check libraries integrity - this part should managed by library management software.

Multiple VM instances can be lauched with different configurations independently.

```
const { Vm } = require('@bluepjs/vm')

const vm = new Vm()
// to enable debug
// const vm = new Vm(true)

// ... loading libraries from database/network/fs/...
const libraries = loadLibrariesFunction()
vm.updateLibraries(libraries)
```

## IDE

VM libraries should be managed by outside app. VM provides all information about itself (libraries, types, nodes, actors and modules) for IDE with `ideData()` method.

```
// ...
const ideData = vm.ideData()
// ...
```

Provided information also includes stringified functions to be runned in IDE for nice integration.

Official IDE is https://github.com/bluep-js/vue3-ide

## Actors

Actors are "outside" object, controlled by VM.

Depending on task Actors can be websockets rooms, chatbot event or IoT device for nodejs apps, same as 3d object on scene for browser webgl applications.

VM can be runned without Actors, if it's required.

Use `actor` module `addActor`/`removeActor` methods to manage VM actors

Actors should realize `./src/module/actor/abstract` interface. There is no strict checking for classes inheritance, so VM may crash if interface realization is not correct.

For easy integration `AbstractActor` class is expoted by module:

```
const { AbstractActor } = require('@bluepjs/vm')

class MyActor extends AbstractActor {}
```

Actors may have state, methods and events.

## Types

@bluepjs is "pseudo-typed" programming system.

VM doesn't do any types check and working fully in "javascript apps style".

But IDE should check for libraries integrity and provide easy way for user to recognize different types of variables and data, so Types are provided by VM for IDE only (and includes color value for type in IDE).

Base Types types are:

 - `basic/execute` - special type to make nodes execution flow
 - `basic/boolean` - booleans
 - `basic/string` - strings
 - `basic/number` - integers
 - `basic/float` - floats
 - `basic/datetime` - `dayjs` datetime object
 - `basic/object` - javascript `Object`
 - `basic/template` - currently not implemented
 - `basic/date` - currently not implemented
 - `basic/time` - currently not implemented

Additional types

 - `bluep/object` - currently not implemented
 - `bluep/class` - currently not implemented
 - `bluep/function` - currently not implemented
 - `bluep/struct` - base for all `structs` types
 - `bluep/enum` - base for all `enums` types

Additional Types can be added by Modules and should be automanaged by IDE

## Nodes

Node is a single block of VM operaion.

There is set of "default programming nodes" coming with VM and VM can be extended with new developed Nodes to use inside VM and IDE.

Use `registerNode` VM method to add Node to VM.

For easy integration `AbstractNode` class is exported by module:

```
const { AbstractNode } = require('@bluepjs/vm')

class MyNode extends AbstractNode {}

// ....

vm.registerNode(MyNode) // param is class, not object of class
```

## Modules

VM designed to be extended with 3-rd party modules.

Module can register multiple Nodes or start/check required "servers" (ex: express) like "global Actors" on `init` and `libraryUpdate` methods and provide reactions to "serves" events. Modules also includes functions to be runned in IDE.

Use `addModule` VM method to add Module to VM.

For easy integration `AbstractModule` class is exported by module:

```
const { AbstractModule } = require('@bluepjs/vm')

class MyModule extends AbstractModule {}

// ....

vm.addModule(MyModule) // param is class, not object of class
```

### Examples

There are 2 modules coming with VM by default: `actor` and `cron` - check this modules source code for deeper examples

## Console

VM use environment `console` object by default, but i can be overriden with `console({ log: Function, error: Function })` VM method

# Examples

Check:

 - `./src/module/cron.js`
 - `./src/module/actor.js`
 - https://github.com/bluep-js/example

# Documentation (under development)

https://bluepjs.readthedocs.io/en/latest/

# Roadmap

VM is developed together with IDE and next steps are:

 - Multiple libraries support (currentlu only `Default` library content is working)
 - Libraries import/export
 - Arrays support
 - Type Templates support
 - Classes and OOP support

# Changelog

## 0.1.1

Fixed errors on empty libraries
