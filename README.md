Inconspicuous.Framework
=======================

## Overview

Inconspicuous.Framework provides a code-centric and architecturally [SOLID](http://en.wikipedia.org/wiki/SOLID_(object-oriented_design)) framework for Unity3D/C# by combining a number of modern patterns and solutions. It is assumed that you have decent knowledge of Unity3D, C# and [Rx](https://rx.codeplex.com/), as well as familiarity with the concepts of IoC, DI and MVCVM. The library has been tested and confirmed to work on PC, Mac and IOS.

### Key Features

* DI container with decorator support and MEF-style annotations that works with AOT-devices.
* View mediation and view models to separate view logic and business logic.
* Support for multiple contexts in one scene and/or loading sub-contexts from external scenes.
* Rx-powered command system that allow type-safe processing of asynchronous events and results.

### Architecture

![Diagram](/diagram.png?raw=true)

## Details

### Views

The View is a component that should be quite familiar to most Unity3D developers. Views inherit from ObservableMonoBehaviour, which again inherit from the regular MonoBehaviour, meaning they are attachable to any game object. Views are the "outer-most" layer of your program that the user interfaces with. They generally connect with the user input (keys, buttons, mouse, touch screen) and respond to changes in the program by displaying fancy animations, text or sound. Just like MonoBehaviours, Views can't have constructors, and as such must rely on method-injection using the `[Inject]`-attribute.

```
public class PanelView : View {
	public Signal<Unit> CloseSignal { get; private set; }

	[Inject]
	public void Construct(Signal<Unit> closeSignal) {
		CloseSignal = closeSignal;
		UpdateAsObservable()
			.Subscribe(_ => {
				if(Input.GetKeyDown(KeyCode.Escape)) {
					CloseSignal.Dispatch();
				}
			});
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

```
[Export(typeof(IMediator<PanelView>))] // Auto-wiring, MEF-style.
[PartCreationPolicy(CreationPolicy.NonShared)]
public class PanelMediator : Mediator<PanelView> {
	private readonly PanelViewModel panelViewModel;

	public PanelMediator(PanelViewModel panelViewModel) {
		this.panelViewModel = panelViewModel;
	}  

	public override void Mediate(PanelView view) {
		panelViewModel.ActiveChanged
			.Subscribe(active => view.SetVisible(active)).DisposeWith(this);
		view.CloseSignal
			.Subscribe(_ => panelViewModel.Active = false).DisposeWith(this);
	}
}
```

A ViewModel is a "reactive" model that typically consists of a set of observable properties and signals. The ViewModel is optional, but can be very helpful in coordinating the input/output of multiple views that display the same information or operate on the same model. There is no base class for the ViewModel, but the framework provides the Property\<T\> and Signal\<T\> helpers, to aid the ViewModel creation process.

```
[Export]
public PanelViewModel {
	private Property<bool> active;

	public PanelViewModel() {
		active = new Property<bool>();
	}

	public bool Active {
		get { return active.Value; }
		set { active.Value = value; }
	}

	public IObservable<bool> ActiveChanged {
		get { return active; }
	}
}
```

#### Signal\<T\> and Property\<T\>

* __Signal\<T\>__ is an open generic that inherits IObservable\<T\>, as well as containing a `Dispatch()`-method that fires the signal with a given value.
* __Property\<T\>__ is basically a Signal\<T\> that remembers its last value and only fires a signal if the value changes.
* __CollectionProperty\<T\>__ is a generic ICollection\<T\> that is observable using `AddAsObservable()`, `RemoveAsObservable()` and `ClearAsObservable()`.
	
### Contexts and ContextViews

The Context is the main entry point that takes care of all interface-to-implementation bindings and can optionally run some startup logic. The ContextView is simply a view that initializes a Context at the start of the program. The ContextView should be a root game object named `_<Name>ContextView` that contains all other objects in the scene. CustomContextView or MainContextView is a general-purpose ContextView that allows you to specify any context to initialize through the inspector.

```
[Scene("Test")]
public class TestContext : Context {
	public TestContext(IContextView contextView, params Context[] subContexts)
		: base(contextView, subContexts) {
		container.Resolve<IViewMediationBinder>().Mediate(contextView);
	}

	public override void Start() {
		container.Resolve<ICommandDispatcher>().Dispatch(new StartCommand());
	}
}
```

CustomContextView also allows you to specify any number of sub-contexts to initialize as dependencies (ie. before the main Context initializes). If a Context is going to be used as a sub-context, it should be defined in a different scene, be part of the build pipeline and have the `[Scene("<Name>")]` attribute specified. If the dependency graph of the application requires some implementation that can't be found in the main context, it will defer the search to any of its sub-contexts.

By the default, the ContextView (or any of it's children) are not mediated. View mediation can be performed by executing the following once all required mediators are registered with the context, as seen in the example above:

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
* By using the decorator pattern, you can easily add replay functionality, network synchronization or other features on top of existing handlers or the dispatcher.

```
public class OpenPanelCommand : ICommand<NullResult> { }

[Export(typeof(ICommandHandler<NullResult>))]
public class OpenPanelCommandHandler : CommandHandler<OpenPanelCommand, NullResult> {
	private readonly PanelViewModel panelViewModel;
  
	public OpenPanelCommandHandler(PanelViewModel panelViewModel) {
		this.panelViewModel = panelViewModel;
	}
  
	public override IObservable<NullResult> Handle(OpenPanelCommand command) {
		panelViewModel.Active = true;
		return Observable.Return(NullResult.Default);
	}
}
```

Commands can also return results. Both CommandHandler and CommandDispatcher are designed in such a way that this process is completely type-safe and takes full advantage of Rx by returning an `IObservable<TResult>`. The framework includes some common commands that are useful for just about every type of program:

```
commandDispatcher.Dispatch(new LoadSceneCommand { SceneName = "Test" });
commandDispatcher.Dispatch(new RestartSceneCommand());
commandDispatcher.Dispatch(new QuitApplicationCommand());
```

#### MacroCommands

A common use case for commands is to execute multiple sub-commands in succession, either in parallel (all commands are started immediately) or serially (the consequent command is not executed before the previous has finished). To help with this, the framework includes a MacroCommand and MacroCommandHandler. A MacroCommand is simply a list of Commands and a type that specifies whether the MacroCommand should execute in parallel or in sequence. In both cases, all results are aggregated and returned once all sub-commands have completed.

```
commandDispatcher.Dispatch(new MacroCommand(MacroCommandType.Sequence) {
	new LoginCommand { Email = "john@doe.com", Password = "johndoe" },
	new OpenPanelCommand()
});
```

## Credits

Inconspicuous.Framework was created by Inconspicuous AS (http://www.inconspicuous.no).

### Dependencies

This framework include parts of the following open-source libraries. You may have to extract the *.unitypackage to copy the required files to the correct directories.

* UniRx (https://github.com/neuecc/UniRx)

## License

Code is released under [the MIT license](LICENSE.md).
