using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using MugenInjection;
using UniRx;
using Container = MugenInjection.MugenInjector;

namespace Inconspicuous.Framework {
	[Export(typeof(ICommandDispatcher))]
	public class CommandDispatcher : ICommandDispatcher {
		private readonly Container container;
		private Dictionary<Type, ICommandHandler> handlerMap;
		private Dictionary<Type, ISubject<object>> observerMap;

		public CommandDispatcher(Container container) {
			this.container = container;
			handlerMap = new Dictionary<Type, ICommandHandler>();
			observerMap = new Dictionary<Type, ISubject<object>>();
		}

		public IObservable<IResult> Dispatch(ICommand command) {
			ICommandHandler commandHandler;
			if(!handlerMap.TryGetValue(command.GetType(), out commandHandler)) {
				commandHandler = ResolveHandlerForCommand(command);
			}
			return commandHandler.Handle(command).Do(r => {
				var type = r.GetType();
				if(observerMap.ContainsKey(type)) {
					observerMap[type].OnNext(r);
				}
			});
		}

		public IObservable<TResult> Dispatch<TResult>(ICommand<TResult> command) where TResult : class, IResult {
			ICommandHandler commandHandler;
			if(!handlerMap.TryGetValue(command.GetType(), out commandHandler)) {
				commandHandler = ResolveHandlerForCommand(command);
			}
			return commandHandler.Handle(command).Cast<IResult, TResult>().Do(r => {
				if(observerMap.ContainsKey(typeof(TResult))) {
					observerMap[typeof(TResult)].OnNext(r);
				}
			});
		}

		public IObservable<TResult> AsObservable<TResult>() where TResult : class, IResult {
			if(!observerMap.ContainsKey(typeof(TResult))) {
				observerMap[typeof(TResult)] = new FastSubject<object>();
			}
			return observerMap[typeof(TResult)].Cast<object, TResult>();
		}

		private ICommandHandler ResolveHandlerForCommand(ICommand command) {
			var resultType = command.GetType().GetInterface(typeof(ICommand<>).Name).GetGenericArguments()[0];
			var handler = container.Get(typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), resultType)) as ICommandHandler;
			handlerMap.Add(command.GetType(), handler);
			return handler;
		}
	}
}
