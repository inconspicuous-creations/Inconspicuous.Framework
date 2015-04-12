#pragma warning disable 0649

using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public partial class ContextConfiguration {
		private static IContextConfiguration defaultContextConfiguration;

		public static IContextConfiguration Default {
			get {
				if(defaultContextConfiguration == null) {
					defaultContextConfiguration = new DefaultContextConfiguration();
				}
				return defaultContextConfiguration;
			}
		}

		private class DefaultContextConfiguration : IContextConfiguration {
			public void Configure(IContainer container) {
				container.Register<ICommandDispatcher, CommandDispatcher>(Reuse.Singleton);
				container.Register<IContextScheduler, ContextScheduler>(Reuse.Singleton);
				container.Register<ILevelManager, LevelManager>(Reuse.Singleton);
				container.Register<IViewMediationBinder, ViewMediationBinder>(Reuse.Singleton);
				container.Register<IFactory<Func<PrefabGameObjectData, GameObject>>, PrefabGameObjectFactory>(Reuse.Singleton);
				container.Register<IFactory<Func<GameObjectData, GameObject>>, GameObjectFactory>(Reuse.Singleton);
				container.Register<ICommandHandler<MacroCommand, ICollection<object>>, MacroCommandHandler>(Reuse.Singleton);
				container.Register<ICommandHandler<LoadSceneCommand, IContextView>, LoadSceneCommandHandler>(Reuse.Singleton);
				container.Register<ICommandHandler<QuitApplicationCommand, Unit>, QuitApplicationCommandHandler>(Reuse.Singleton);
				container.Register<ICommandHandler<RestartSceneCommand, IContextView>, RestartSceneCommandHandler>(Reuse.Singleton);
				container.Register<ICommandHandler<StartCommand, Unit>, StartCommandHandler>(Reuse.Singleton);
			}
		}
	}
}
