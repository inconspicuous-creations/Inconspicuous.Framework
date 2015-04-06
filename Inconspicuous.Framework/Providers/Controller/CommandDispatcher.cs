using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using UniRx;

namespace Inconspicuous.Framework {
	public class CommandDispatcher : ICommandDispatcher {
		private readonly IContainer container;
		private readonly Dictionary<Type, ICommandHandler> handlerMap;
		private readonly Dictionary<Type, ISubject<object>> observerMap;

		public CommandDispatcher(IContainer container) {
			this.container = container;
			this.handlerMap = new Dictionary<Type, ICommandHandler>();
			this.observerMap = new Dictionary<Type, ISubject<object>>();
		}

		public IObservable<object> Dispatch(ICommand command) {
			var commandHandler = default(ICommandHandler);
			if(!handlerMap.TryGetValue(command.GetType(), out commandHandler)) {
				commandHandler = ResolveHandlerForCommand(command);
			}
			return commandHandler.Handle(command).Cast<object, object>().Do(r => {
				var type = r.GetType();
				if(observerMap.ContainsKey(type)) {
					observerMap[type].OnNext(r);
				}
			});
		}

		public IObservable<TResult> Dispatch<TResult>(ICommand<TResult> command) where TResult : class {
			ICommandHandler commandHandler;
			if(!handlerMap.TryGetValue(command.GetType(), out commandHandler)) {
				commandHandler = ResolveHandlerForCommand(command);
			}
			return commandHandler.Handle(command).Cast<object, TResult>().Do(r => {
				if(observerMap.ContainsKey(typeof(TResult))) {
					observerMap[typeof(TResult)].OnNext(r);
				}
			});
		}

		public IObservable<TResult> AsObservable<TResult>() where TResult : class {
			if(!observerMap.ContainsKey(typeof(TResult))) {
				observerMap[typeof(TResult)] = new Subject<object>();
			}
			return observerMap[typeof(TResult)].Cast<object, TResult>();
		}

		private ICommandHandler ResolveHandlerForCommand(ICommand command) {
			var resultType = command.GetType().GetInterface(typeof(ICommand<>).Name).GetGenericArguments()[0];
			var handler = container.Resolve(typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), resultType)) as ICommandHandler;
			handlerMap.Add(command.GetType(), handler);
			return handler;
		}
	}
}
