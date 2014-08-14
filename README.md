Inconspicuous.Framework
=======================

Inconspicuous.Framework is an MVVM framework for Unity3D/C#. It has the following dependencies:

* DryIoc (https://bitbucket.org/dadhi/dryioc)
* UniRx (https://github.com/neuecc/UniRx)

## Overview

Inconspicuous.Framework provides a software-centric and architecturally [SOLID](http://en.wikipedia.org/wiki/SOLID_(object-oriented_design)) framework for Unity3D/C# by combining a number of modern open-source solutions and libraries. It is assumed that you have decent knowledge of Unity3D, C# and [Rx](https://rx.codeplex.com/), as well as familiarity with the concepts of DI and MVVM. The big image that explains it all:

### Views

The View is a component that should be quite familiar to most Unity3D developers. Views inherit from ObservableMonoBehaviour, which again inherit from the regular MonoBehaviour, meaning they are attachable to any game object. Views are the "outer-most" layer of your program that the user interfaces with. They generally connect with the user input (keys, buttons, mouse, touch screen) and respond to changes in the program by displaying fancy animations, text or sound.

### Mediators and ViewModels

The Mediator is a thin layer between the view and the deeper layers of the program. The View is often subject to a lot of changes, and the purpose of the mediator is to isolate these changes so they don't propagate and cause bugs deeper into the "core" of the program. It is optional, but highly recommended.

A ViewModel is a "reactive" model that typically consists of a set of observable properties and signals. It is used by the mediator, and can be helpful in coordinating the input/output of multiple views that display the same information or operate on the same model. There is no base class for the ViewModel, but the framework provides the Property<T> and Signal<T> helpers, to aid the ViewModel creation process. Just like the Mediator, the ViewModel is also optional.

### ContextViews and Contexts

The Context is the main entry point that takes care of all interface-to-implementation bindings and can optionally run some startup logic. The ContextView is simply a view that initializes a Context at the start of the program. The ContextView should be a root game object named `_<Name>ContextView` that contains all other objects in the scene. CustomContextView or MainContextView is a general-purpose ContextView that allows you to specify any context to Initialize through the inspector.

CustomContextView also allows you to specify any number of subcontexts to initialize as dependencies (ie. before the main Context initializes). If a Context is going to be used as a subcontext, it should be defined in a different scene, be part of the build pipeline and have the `[Scene("<Name>")]` attribute specified. If the dependency graph of the application requires some implementation that can't be found in the main context, it will defer the search to any of its subcontexts.

By the default, the ContextView (or any of it's children) are not mediated. View mediation can be performed by executing the following once all required mediators are registered with the context:

```
container.Resolve<IViewMediationBinder>().Mediate(contextView);
```

### Commands, CommandHandlers and the CommandDispatcher

Commands are mainly used for asynchronous or non-guaranteed actions. The three components have the following responsibilities:

* __Command__: Contains the fields/arguments required to execute this command.
* __CommandHandler__: Executes the given command.
* __CommandDispatcher__: Executes the given command by retrieving the appropriate CommandHandler for the Command, if it exists.

This separation of concerns has the following benefits:

* Commands can be easily serialized.
* When the same command is executed in different contexts, it may be handled differently (eg. for client/server architectures) or not at all (eg. mocking during development).
* By using the decorator pattern, you can easily add replay functionality, network synchronization and/or other useful features.

## License

Code is released under [the MIT license](LICENSE.md).
