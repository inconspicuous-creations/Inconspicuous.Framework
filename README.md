Inconspicuous.Framework
=======================

## Overview

Inconspicuous.Framework provides a code-centric and architecturally [SOLID](http://en.wikipedia.org/wiki/SOLID_(object-oriented_design)) framework for Unity3D/C# by combining a number of modern patterns and solutions. It is assumed that you have decent knowledge of Unity3D, C# and [Rx](https://rx.codeplex.com/), as well as familiarity with the concepts of IoC, DI and MVCVM.

The library has been tested and confirmed to work with Unity 4.6+ on PC, Mac and IOS (not tested with IL2CPP).

Example project can be found here: [Inconspicuous.Framework.Example](https://github.com/inconspicuous-creations/Inconspicuous.Framework.Example)

### Key Features

* DI container with support for open generics and decorators that works with AOT-only devices.
* View mediation and view models to separate view logic and business logic.
* Support for multiple contexts in one scene and/or loading sub-contexts from external scenes.
* Rx-powered command system that allow type-safe processing of asynchronous events.

### Architecture

![Diagram](/diagram.png?raw=true)

## Table of Contents

* [Views](#views)
* [Mediators and ViewModels](#mediators-and-viewmodels)
* [Context and ContextViews](#contexts-and-contextviews)
  * [Context Configuration](#context-configuration)
  * [View Mediation](#view-mediation)
* [Commands, CommandHandlers and the CommandDispatcher](#commands-commandHandlers-and-the-commandDispatcher)
  * [Macro Commands](#macro-commands)

## Documentation

### Views

The View is a component that should be quite familiar to most Unity3D developers. Views inherit MonoBehaviour, meaning they are attachable to any game object. Views are the "outer-most" layer of your program that the user interfaces with. They generally connect with the user input (keys, buttons, mouse, touch screen) and respond to changes in the program by displaying fancy animations, text or sound. Just like MonoBehaviours, Views can't have constructors, and as such must rely on method-injection using the `[Inject]`-attribute.

```csharp
public class PanelView : View {
	public Subject<Unit> CloseSubject { get; private set; }

	[Inject]
	public void Construct() {
		this.CloseSubject = new Subject<Unit>();
	}
	
	public void Update() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			CloseSubject.OnNext();
		}
	}

	public void SetVisible(bool visible) {
		if(visible) {
			// Do some fancy show animation.
		} else {
			// Do some fancy hide animation.
		}
	}
}
```

### Mediators and ViewModels

The Mediator is a thin layer between the view and the deeper layers of the program. The View is often subject to a lot of changes, and the purpose of the mediator is to isolate these changes so they don't propagate and cause bugs deeper into the "core" of the program. This example shows it usage in conjunction with a view model:

```csharp
[Export(typeof(IMediator<PanelView>))] // Auto-wiring, MEF-style.
[PartCreationPolicy(CreationPolicy.NonShared)]
public class PanelMediator : Mediator<PanelView> {
	private readonly PanelViewModel panelViewModel;

	public PanelMediator(PanelViewModel panelViewModel) {
		this.panelViewModel = panelViewModel;
	}  

	public override void Mediate(PanelView view) {
		panelViewModel.AsObservable("Active", () => panelViewModel.Active)
			.Subscribe(x => view.SetVisible(x)).AddTo(this);
		view.CloseSignal
			.Subscribe(_ => panelViewModel.Active = false).AddTo(this);
	}
}
```

A ViewModel is a reactive model that implements `INotifyPropertyChanged` and one or more `ObservableCollection<T>`. The ViewModel is optional, but can be very helpful in coordinating the input/output of multiple views that display the same information or operate on the same model.

```csharp
[Export]
public PanelViewModel : ViewModel {
	private bool active;

	public bool Active {
		get { return active; }
		set { SetProperty<bool>(ref active, value, "Active"); }
	}
}
```
	
### Contexts and ContextViews

The Context is the main entry point that takes care of all interface-to-implementation bindings and can optionally run some startup logic. The ContextView is simply a view that initializes a Context at the start of the program. The ContextView should be a root game object named `_<Name>ContextView` that contains all other objects in the scene. CustomContextView and MainContextView are general-purpose ContextView that allows you to specify any context to initialize through the inspector.

```csharp
[Scene("Test")]
public class TestContext : Context {
	public TestContext(IContextView contextView, params Context[] subContexts)
		: base(contextView, subContexts) {
		ContextConfiguration.Default.Configure(container);
	}

	public override void Start() {
		container.Resolve<IViewMediationBinder>().Mediate(contextView);
		container.Resolve<ICommandDispatcher>().Dispatch(new StartCommand()).Subscribe();
	}
}
```

CustomContextView also allows you to specify any number of sub-contexts to initialize as dependencies (ie. before the main Context initializes). If a Context is going to be used as a sub-context, it should be defined in a different scene, be part of the build pipeline and have the `[Scene("<Name>")]` attribute specified. If the dependency graph of the application requires some implementation that can't be found in the main context, it will defer the search to any of its sub-contexts. You can not have more than one sub-context of the same type.

#### Context Configuration

The context is configured by adding bindings to the `IContainer` object. Basic usage looks like this: 

```csharp
container.Register<IService, MyWebService>(Reuse.Singleton);
```

You can also use MEF-style `Export`-attributes that auto-register with all contexts.

```csharp
[Export]
public class TestViewModel { ... }
```

##### Default Context Configuration

The following will register all of Inconspicuous.Framework's default providers with the context, as seen in the example above.

```csharp
ContextConfiguration.Default.Configure(container);
```

#### View Mediation

By the default, the ContextView (or any of it's children) are not mediated. View mediation can be performed by executing the following once all required mediators are registered with the context:

```csharp
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
* By using the decorator pattern, you can easily add replay functionality, network synchronization or other features on top of existing handlers or the dispatcher.

```csharp
public class OpenPanelCommand : ICommand<Unit> { }

[Export(typeof(ICommandHandler<Unit>))]
public class OpenPanelCommandHandler : CommandHandler<OpenPanelCommand, Unit> {
	private readonly PanelViewModel panelViewModel;
  
	public OpenPanelCommandHandler(PanelViewModel panelViewModel) {
		this.panelViewModel = panelViewModel;
	}
  
	public override IObservable<Unit> Handle(OpenPanelCommand command) {
		return Observable.Defer(observer => {
			panelViewModel.Active = true;
			return Observable.Return(Unit.Default);
		});
	}
}
```

Commands can also return results. Both CommandHandler and CommandDispatcher are designed in such a way that this process is completely type-safe and takes full advantage of Rx by returning an `IObservable<TResult>`. The framework includes some common commands that are useful for just about every type of program:

```csharp
commandDispatcher.Dispatch(new LoadSceneCommand { SceneName = "Test" }).Subscribe();
commandDispatcher.Dispatch(new RestartSceneCommand()).Subscribe();
commandDispatcher.Dispatch(new QuitApplicationCommand()).Subscribe();
```

#### MacroCommands

A common use case for commands is to execute multiple sub-commands in succession, either in parallel (all commands are started immediately) or serially (the consequent command is not executed before the previous has finished). To help with this, the framework includes a MacroCommand and MacroCommandHandler. A MacroCommand is simply a list of Commands and a type that specifies whether the MacroCommand should execute in parallel or in sequence. In both cases, all results are aggregated and returned once all sub-commands have completed.

```csharp
commandDispatcher.Dispatch(new MacroCommand(MacroCommandType.Sequence) {
	new LoginCommand { Email = "john@doe.com", Password = "johndoe" },
	new OpenPanelCommand()
}).Subscribe();
```

## Credits

Inconspicuous.Framework was created by Inconspicuous AS (http://www.inconspicuous.no).

### Dependencies

This framework requires the following library (not included):

* UniRx (https://github.com/neuecc/UniRx)

## License

Code is released under [the MIT license](LICENSE.md).
